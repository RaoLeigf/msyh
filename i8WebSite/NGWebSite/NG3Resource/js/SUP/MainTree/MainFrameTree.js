function myIsIE() {
    if (!!window.ActiveXObject || "ActiveXObject" in window)
        return true;
    else
        return false;
}

function TranTitle(title) {

    var userAgent = navigator.userAgent;
    var reIE = new RegExp("MSIE (\\d+\\.\\d+);");
    reIE.test(userAgent);
    var fIEVersion = parseFloat(RegExp["$1"]);

    var height = '15px';
    var width = title.length * 15;
    var left = title.length * 7.0 + 2;
    if (myIsIE()) {
        width = title.length * 17;
        left = title.length * 7.5;
    }
    if (!myIsIE()) {
        title = "<div style='-o-transform:rotate(90deg);-webkit-transform: rotate(90deg);-moz-transform:rotate(90deg);filter:progid:DXImageTransform.Microsoft.BasicImage(rotation=180);width:" + width + "px;height:" + height + ";left:" + left + "px !important;position: relative;padding: 8px 8px 8px 8px;'>"
                    + title.split('').join('<br>') + "</div>";
    }
    else {
        if (fIEVersion <= 9) {
            title = title.split('').join('<br>');
        } else {
            title = "<div style='transform:rotate(90deg);-ms-transform: rotate(90deg);width:" + width + "px;height:" + height + ";left:" + left + "px !important;position: relative;padding: 8px 8px 8px 8px;'>"
                        + title.split('').join('<br>') + "</div>";
        }
    }
    return title;
};

function setCookie(c_name, value, expiredays)
{
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + expiredays);
    document.cookie = c_name + "=" + escape(value) +
        ((expiredays==null)?"":";expires="+exdate.toGMTString())
}

function getCookie(c_name) {
    if (document.cookie.length > 0) {
        c_start = document.cookie.indexOf(c_name + "=");
        if (c_start != -1) {
            c_start = c_start + c_name.length + 1;
            c_end = document.cookie.indexOf(";", c_start);
            if (c_end == -1) c_end = document.cookie.length;
            return unescape(document.cookie.substring(c_start,c_end))
        }
    }
    return "";
}


function OpenFunctionWeb(url, rightkey, title, moduleno, suite) {
    var port = document.location.port;
    var hostname = document.location.host;
    port = port == "" ? "80" : port;
    if (url == 'I6PProjectBIMManager') {
        Ext.MessageBox.alert('提示', 'web版不支持打开winform界面');
        return;
    }
    Ext.Ajax.request({
        url: C_ROOT + "/Portal.mvc/CheckUrlForPortal",
        params: {
            hostname: hostname,
            port: port,
            url: url,
            nodeinfo: {
                'url': url,
                'rightkey': rightkey,
                'opentype': 'myfunction',
                'moduleno': moduleno,
                'suite': suite
            }
        },
        success: function (response) {
            var resp = Ext.JSON.decode(response.responseText)
            if (resp.allowopen == "true") {
                window.parent.$OpenTab(title, resp.url);
            } else {
                Ext.Msg.alert("提示", resp.errmsg);
            }
        }
    });
}


//系统功能树
Ext.define('Ext.ng.sysFuncTree', {
    extend: 'Ext.tab.Panel',
    alias: 'widget.ngSysFuncTree',
    tabPosition: 'left',
    hasRightClickMenu: false, // 右键菜单开关 true/false
    isSystemUser: false,//是否是系统管理员
    currentTree: '', // panel有多个套件，currentTree记录当前显示的treepanel
    hasCheckBox: false, // 节点带复选框的开关 true/false
    hasDbClickListener: false, // 双击节点打开对应页面的开关 true/false
    hasRightControl: true,//是否要加权限控制false不加/true加
    hasSelection: false, //是否有更换图标的监听事件
    //menu:null,
    //treeFilter: 'select * from fg3_menu where code in (select code from fg_floatmenu_manager_out)',//构建系统功能树的节点过滤条件
    treeFilter: '',
    tabBar: {
        width: 30
    },
    initComponent: function () {
        var me = this;
        this.callParent();
        me.treeFieldsItems = [{ name: 'text', type: 'string' },
                                { name: 'my', type: 'string' }//我的自定义属性
        ]
        if (me.hasCheckBox == true) {
            me.treeFieldsItems.push({ name: 'checked', type: 'boolean', values: false });
        }
        me.itemMenu = Ext.create('Ext.menu.Menu', {
            //selectedNode:null,
            items: [
                {
                    text: '添加到快捷菜单',
                    handler: function () {
                        var node = me.itemMenu.selectedNode;
                        var param = { 'str': node.raw.text, 'managername': node.raw.managername, 'moduleno': node.raw.moduleno, 'id': node.raw.code, 'url': node.raw.url, 'suite': node.raw.suite, 'rightkey': node.raw.rightkey, 'busphid': node.raw.busphid };
                        if (window.external.IsInWebBrowser == undefined) {
                            window.ShortcutsRefrech();
                        } else {
                            window.external.AddShortCutItem(JSON.stringify(param));
                        }
                        Ext.Ajax.request({
                            url: C_ROOT + 'SUP/ShortcutMenu/AddShortcutMenu',
                            params: {
                                'name': node.raw.text,
                                'originalcode': node.raw.code,
                                'itemurl': node.raw.url,
                                'busphid': node.raw.busphid
                            },
                            async: false
                            //,
                            //success: function (response) {
                            //    if (!Ext.isEmpty(response.responseText)) {
                            //        window.external.MessageShow(response.responseText);
                            //    }
                            //}
                        });
                    }
                }, {
                    text: '添加到我的桌面',
                    hidden: me.isSystemUser,
                    handler: function () {
                        var node = me.itemMenu.selectedNode;
                        Ext.Ajax.request({
                            url: C_ROOT + 'SUP/MyDesktopSet/AddMyDesktopNodeEx',
                            params: {
                                json: { 'text': node.raw.text, 'managername': node.raw.managername, 'moduleno': node.raw.moduleno, 'code': node.raw.code, 'url': node.raw.url, 'suite': node.raw.suite, 'rightkey': node.raw.rightkey, 'rightname': node.raw.rightname, 'busphid': node.raw.busphid },
                                groupname: node.parentNode.raw.text
                            },
                            async: false,
                            success: function (response) {
                                if (window.external.IsInWebBrowser == undefined) {
                                    return;
                                }
                                if (Ext.isEmpty(response.responseText)) {
                                    window.external.RefreshMyDesktop();
                                } else {
                                    window.external.MessageShow(response.responseText);
                                }
                            }
                        });

                    }
                }, '-',
                {
                    text: '全部展开',
                    handler: function () {
                        var node = me.itemMenu.selectedNode;
                        var root = findRoot(node)
                        expandTree(root);
                    }
                },
                {
                    text: '全部折叠',
                    handler: function () {
                        var node = me.itemMenu.selectedNode;
                        var root = findRoot(node)
                        collapseTree(root);
                    }
                }
            ]
        });
        me.containMenu = Ext.create('Ext.menu.Menu', {
            //selectedNode:null,
            items: [
                {
                    text: '全部展开',
                    handler: function () {
                        var node = me.containMenu.selectedNode;
                        var root = findRoot(node)
                        expandTree(root);
                    }
                },
                {
                    text: '全部折叠',
                    handler: function () {
                        var node = me.containMenu.selectedNode;
                        var root = findRoot(node)
                        collapseTree(root);
                    }
                }
            ]
        });
        me.loadData();
        me.tabBar.on('change', function (tabBar, tab, card, eOpts) {
            tabBar.needsScroll = false;//禁止tabBar乱滚动，抽风
        });
        ////沉默页面的函数
        //StoreloadMarsk = new Ext.LoadMask(document.body, {
        //    msg: '加载中',
        //    disabled: false
        //});
        //me.StoreloadMarsk = StoreloadMarsk;
    },
    loadData: function (hasRightControl) {
        var me = this;
        var arr = [];
        //me.flag = hasRightControl;//是否控制权限的开关
        Ext.Ajax.request({
            url: C_ROOT + 'SUP/MainTree/GetSuiteList?flag=' + me.hasRightControl + '&treeFilter=' + me.treeFilter,
            async: false,
            success: function (res, opts) {
                if (res.responseText.length > 0) {
                    var suites = Ext.JSON.decode(res.responseText);
                    for (var i = 0; i < suites.length; i++) {
                        var title = TranTitle(suites[i].Name);
                        var menu = Ext.create('Ext.ng.TreePanel', {
                            autoLoad: false,
                            height: 600,
                            split: true,
                            suiteName: suites[i].Name,
                            border: 0,
                            bodyStyle: 'border-left:0',//改为这个        
                            //title: suites[i].Name,//title,
                            title: title,
                            suite: suites[i].Code,
                            layout: 'fit',
                            //treeFields: [{ name: 'text', type: 'string' },
                            //    { name: 'my', type: 'string' }//我的自定义属性
                            //],
                            treeFields: me.treeFieldsItems,
                            treeFilter: me.treeFilter,
                            url: C_ROOT + 'SUP/MainTree/LoadMenu?flag=' + me.hasRightControl + '&treeFilter=' + me.treeFilter, //?'+treeFilter+'=me.treeFilter''HR/EmpInfoList/LoadMenu'
                            listeners: {
                                'afterrender': function (treepanel, e) {
                                    //if (treepanel.treeFilter == '' || treepanel.treeFilter == null) {
                                    //    return;
                                    //}
                                    //var root = treepanel.getRootNode();
                                    //collapseTree(root);
                                },
                                'expand': function () {
                                    var me = this;
                                    Ext.apply(this.getStore().proxy.extraParams, { suite: me.suite });//, flag: me.findParentByType('ngSysFuncTree').hasRightControl, treeFilter: me.treeFilter 
                                    this.getRootNode().expand();
                                },
                                'activate': function () {
                                    var me = this;
                                    Ext.apply(this.getStore().proxy.extraParams, { suite: me.suite });//, flag: me.findParentByType('ngSysFuncTree').hasRightControl, treeFilter: me.treeFilter 
                                    //this.getRootNode().expand();//有些电脑会全部出发这个事件，达不到懒加载目的
                                },
                                'checkchange': function (node, checked) {
                                    setChildNodeChecked(node, checked);
                                },
                                'itemdblclick': function (view, rcd, item, idx, event, eOpts) {
                                    if (rcd.raw.leaf) {
                                        //WF.Center.openTab(rcd.raw.text, rcd.raw.url);
                                        if (me.hasDbClickListener == true) {
                                            var param = { 'str': rcd.raw.text, 'rightname': rcd.raw.rightname, 'managername': rcd.raw.managername, 'moduleno': rcd.raw.moduleno, 'id': rcd.raw.code, 'url': rcd.raw.url, 'suite': rcd.raw.suite, 'rightkey': rcd.raw.rightkey, 'functionname': rcd.raw.functionname };
                                            if (window.external.IsInWebBrowser == undefined) {
                                                if (rcd.raw.url.indexOf("exe") != -1) {
                                                    var managerName = rcd.raw.managername;
                                                    var rightkey = rcd.raw.rightkey;
                                                    var errmsg;
                                                    if (managerName == '' || managerName == null) {
                                                        errmsg = "WEB版无法打开PB菜单！";
                                                    }
                                                    else {
                                                        errmsg = "WEB版无法打开WinForm菜单！";
                                                    }
                                                    Ext.MessageBox.alert('提示', errmsg);
                                                } else {
                                                    OpenFunctionWeb(rcd.raw.url, rcd.raw.rightkey, rcd.raw.text, rcd.raw.moduleno, rcd.raw.suite);
                                                }
                                            } else {
                                                window.external.OpenFunction(rcd.raw.url, JSON.stringify(param));
                                            }
                                        }
                                    }
                                }
                            }
                        });
                        if (me.hasRightClickMenu == true) {
                            menu.addListener('itemcontextmenu', function (view, rec, node, index, e) {
                                e.stopEvent();
                                var ngSysFuncTree = view.findParentByType('ngSysFuncTree');
                                if (rec.data.leaf == true) {
                                    ngSysFuncTree.itemMenu.selectedNode = rec;
                                    ngSysFuncTree.itemMenu.showAt(e.getXY());
                                } else {
                                    ngSysFuncTree.containMenu.selectedNode = rec;
                                    ngSysFuncTree.containMenu.showAt(e.getXY());
                                }
                                return false;
                            });
                            menu.addListener('containercontextmenu', function (treeP, e, eOpts) {
                                e.stopEvent();
                                var ngSysFuncTree = this.findParentByType('ngSysFuncTree');
                                ngSysFuncTree.containMenu.selectedNode = treeP.store.data.items[0];
                                ngSysFuncTree.containMenu.showAt(e.getXY());
                                return false;
                            })
                        }
                        if (me.hasSelection == true) {
                            menu.addListener('selectionchange', function (view, selected, eOpts) {
                                if (selected.length == 0) return;
                                else {
                                    if (modified) {
                                        Ext.MessageBox.confirm('提示', '功能图标有修改，是否要保存？', function (btn) {
                                            if (btn == 'yes') {
                                                var menuGrids = "[";
                                                var gridDatas = grid.getStore().data;
                                                for (var i = 0; i < gridDatas.length; i++) {
                                                    menuGrids += '{"busphid":"' + gridDatas.items[i].data.BusPhid
                                                        + '","name":"' + gridDatas.items[i].data.FuncIconName
                                                        + '","iconid":"' + gridDatas.items[i].data.FuncIconId + '"}';
                                                    if (i != gridDatas.length - 1) {
                                                        menuGrids += ',';
                                                    }
                                                }
                                                menuGrids += "]";
                                                Ext.Ajax.request({
                                                    params: { 'menuGrids': menuGrids },
                                                    url: '../SUP/FuncIconManager/Save',
                                                    success: function (response) {
                                                        if (response.responseText == "True") {
                                                            Ext.MessageBox.alert('提示', '保存成功！');
                                                            gridStore.load({ params: { "code": cur_menuCode, "suite": sysFuncTree.currentTree.suiteName } });
                                                        } else if (response.responseText == "False") {
                                                            Ext.MessageBox.alert('提示', '没有做任何修改。');
                                                        }
                                                    }
                                                });
                                            }
                                            modified = false;
                                            cur_menuCode = selected[0].raw.code;
                                            gridStore.load({ params: { "code": cur_menuCode, "suite": view.view.panel.suiteName } });
                                        });
                                    } else {
                                        modified = false;
                                        cur_menuCode = selected[0].raw.code;
                                        gridStore.load({ params: { "code": cur_menuCode, "suite": view.view.panel.suiteName } });
                                    }
                                }
                            });
                        }
                        arr.push(menu);
                    }

                    me.suites = arr; //第一次加载把arr存在suites中
                    me.add(arr);
                    if (arr.length > 0) {
                        var cookieSuite = getCookie($appinfo.userID);
                        var cookieSuiteNum = 0;
                        if (cookieSuite != null && cookieSuite != "") {
                            for (var i = 0; i < arr.length; i++) {
                                if (arr[i].suite == cookieSuite) {
                                    cookieSuiteNum = i;
                                    break;
                                }
                            }
                        }
                        Ext.apply(arr[cookieSuiteNum].getStore().proxy.extraParams, { suite: arr[cookieSuiteNum].suite });//, flag: me.hasRightControl, treeFilter: me.treeFilter 
                        arr[cookieSuiteNum].getRootNode().expand();//加载第一个
                        me.currentTree = arr[cookieSuiteNum];
                    }
                    me.setActiveTab(me.items.items[cookieSuiteNum]);
                }
            }
        });// 画出左侧菜单树 
    },
    //height: 300,
    //width: 300,
    itemId: 'sysFuncTree',
    dockedItems: [{
        xtype: 'toolbar',
        //width: 300,
        //height: 26,
        height: 30,
        dock: 'top',
        layout: 'border',
        minWidth: 100,
        items: [
            {
                region: 'center',
                xtype: 'textfield',
                itemId: 'query',
                name: 'queryname',
                emptyText: '搜索内容',
                enableKeyEvents: true,
                listeners: {
                    'keydown': function (el, e, eOpts) {
                        if (e.getKey() == e.ENTER) {
                            var condition = this.ownerCt.queryById('query').getValue();
                            //alert(condition);
                            if (condition == '' || condition == null) {
                                //alert('请输入搜索内容');
                                return;
                            };
                            var sysFuncTree = this.findParentByType('ngSysFuncTree');//找到根节点
                            var itemsLength = sysFuncTree.items.items.length;
                            sysFuncTree.setActiveTab(sysFuncTree.items.items[itemsLength - 1]);
                            sysFuncTree.removeAll();
                            var orgtitle = "搜索结果";


                            var qmenu = Ext.create('Ext.ng.TreePanel', {
                                autoLoad: false,
                                height: 600,
                                split: true,
                                border: false,
                                text: "搜索结果",
                                title: TranTitle(orgtitle),
                                layout: 'fit',
                                treeFields: sysFuncTree.treeFieldsItems,
                                hasDbClickListener: sysFuncTree.hasDbClickListener,
                                //treeFields: [{ name: 'text', type: 'string' },
                                //    { name: 'my', type: 'string' }//我的自定义属性
                                //],
                                url: C_ROOT + 'SUP/MainTree/Query?flag=' + sysFuncTree.hasRightControl + '&treeFilter=' + sysFuncTree.treeFilter,
                                listeners: {
                                    'afterrender': function () {
                                    },
                                    'expand': function () {
                                        var me = this;
                                        Ext.apply(this.getStore().proxy.extraParams, { suite: me.suite });//, flag: sysFuncTree.hasRightControl, treeFilter: sysFuncTree.treeFilter 
                                        this.getRootNode().expand();
                                    },
                                    'activate': function () {
                                        var me = this;
                                        Ext.apply(this.getStore().proxy.extraParams, { suite: me.suite });//, flag: sysFuncTree.hasRightControl, treeFilter: sysFuncTree.treeFilter 
                                        //this.getRootNode().expand();//有些电脑会全部出发这个事件，达不到懒加载目的
                                    },
                                    'checkchange': function (node, checked) {
                                        setChildNodeChecked(node, checked);
                                    },
                                    'itemdblclick': function (view, rcd, item, idx, event, eOpts) {
                                        if (rcd.raw.leaf) {
                                            if (this.hasDbClickListener == true) {
                                                //WF.Center.openTab(rcd.raw.text, rcd.raw.url);
                                                var param = { 'str': rcd.raw.text, 'rightname': rcd.raw.rightname, 'managername': rcd.raw.managername, 'moduleno': rcd.raw.moduleno, 'id': rcd.raw.code, 'url': rcd.raw.url, 'suite': rcd.raw.suite, 'rightkey': rcd.raw.rightkey, 'functionname': rcd.raw.functionname };
                                                if (window.external.IsInWebBrowser == undefined) {
                                                    if (rcd.raw.url.indexOf("exe") != -1) {
                                                        var managerName = rcd.raw.managername;
                                                        var rightkey = rcd.raw.rightkey;
                                                        var errmsg;
                                                        if (managerName == '' || managerName == null) {
                                                            errmsg = "WEB版无法打开PB菜单！";
                                                        }
                                                        else {
                                                            errmsg = "WEB版无法打开WinForm菜单！";
                                                        }
                                                        Ext.MessageBox.alert('提示', errmsg);
                                                    } else {
                                                        //window.parent.$OpenTab(rcd.raw.text, rcd.raw.url, JSON.stringify(param));
                                                        OpenFunctionWeb(rcd.raw.url, rcd.raw.rightkey, rcd.raw.text, rcd.raw.moduleno, rcd.raw.suite);
                                                    }
                                                } else {
                                                    window.external.OpenFunction(rcd.raw.url, JSON.stringify(param));
                                                }
                                            }
                                        }
                                    }
                                }
                            });
                            if (sysFuncTree.hasRightClickMenu == true) {
                                qmenu.addListener('itemcontextmenu', function (view, rec, node, index, e) {
                                    e.stopEvent();
                                    var ngSysFuncTree = view.findParentByType('ngSysFuncTree');
                                    if (rec.data.leaf == true) {
                                        ngSysFuncTree.itemMenu.selectedNode = rec;
                                        ngSysFuncTree.itemMenu.showAt(e.getXY());
                                    } else {
                                        ngSysFuncTree.containMenu.selectedNode = rec;
                                        ngSysFuncTree.containMenu.showAt(e.getXY());
                                    }
                                    return false;
                                });
                                qmenu.addListener('containercontextmenu', function (treeP, e, eOpts) {
                                    e.stopEvent();
                                    var ngSysFuncTree = this.findParentByType('ngSysFuncTree');
                                    ngSysFuncTree.containMenu.selectedNode = treeP.store.data.items[0];
                                    ngSysFuncTree.containMenu.showAt(e.getXY());
                                    return false;
                                })
                            }
                            if (sysFuncTree.hasSelection == true) {
                                qmenu.addListener('selectionchange', function (view, selected, eOpts) {
                                    if (selected.length == 0) return;
                                    else {
                                        if (modified) {
                                            Ext.MessageBox.confirm('提示', '功能图标有修改，是否要保存？', function (btn) {
                                                if (btn == 'yes') {
                                                    Save();
                                                }
                                                modified = false;
                                                cur_menuCode = selected[0].raw.code;
                                                gridStore.load({ params: { "code": cur_menuCode, "suite": view.view.panel.suiteName } });
                                            });
                                        } else {
                                            modified = false;
                                            cur_menuCode = selected[0].raw.code;
                                            gridStore.load({ params: { "code": cur_menuCode, "suite": view.view.panel.suiteName } });
                                        }
                                    }
                                });
                            }
                            sysFuncTree.add(qmenu);
                            Ext.apply(qmenu.getStore().proxy.extraParams, { condition: condition });//, flag: sysFuncTree.hasRightControl, treeFilter: sysFuncTree.treeFilter 
                            qmenu.getRootNode().expand();//加载第一个
                            sysFuncTree.currentTree = qmenu;
                            return false;
                        }
                    }
                }
                //minWidth: 80             
            }, {
                region: 'east',
                //width: 40,
                tooltip: '搜索',
                iconCls: 'icon-Query',
                handler: function () {
                    var condition = this.ownerCt.queryById('query').getValue();
                    //alert(condition);
                    if (condition == '' || condition == null) {
                        //alert('请输入搜索内容');
                        return;
                    };
                    var sysFuncTree = this.findParentByType('ngSysFuncTree');//找到根节点
                    var itemsLength = sysFuncTree.items.items.length;
                    sysFuncTree.setActiveTab(sysFuncTree.items.items[itemsLength - 1]);
                    sysFuncTree.removeAll();
                    var orgtitle = "搜索结果";
                    var qmenu = Ext.create('Ext.ng.TreePanel', {
                        autoLoad: false,
                        height: 600,
                        split: true,
                        border: false,
                        text: "搜索结果",
                        title: TranTitle(orgtitle),
                        layout: 'fit',
                        treeFields: sysFuncTree.treeFieldsItems,
                        hasDbClickListener: sysFuncTree.hasDbClickListener,
                        url: C_ROOT + 'SUP/MainTree/Query?flag=' + sysFuncTree.hasRightControl + '&treeFilter=' + sysFuncTree.treeFilter,
                        listeners: {
                            'afterrender': function () {
                            },
                            'expand': function () {
                                var me = this;
                                Ext.apply(this.getStore().proxy.extraParams, { suite: me.suite });//, flag: sysFuncTree.hasRightControl, treeFilter: sysFuncTree.treeFilter 
                                this.getRootNode().expand();
                            },
                            'activate': function () {
                                var me = this;
                                Ext.apply(this.getStore().proxy.extraParams, { suite: me.suite });//, flag: sysFuncTree.hasRightControl, treeFilter: sysFuncTree.treeFilter 
                                //this.getRootNode().expand();//有些电脑会全部出发这个事件，达不到懒加载目的
                            },
                            'checkchange': function (node, checked) {
                                setChildNodeChecked(node, checked);
                            },
                            'itemdblclick': function (view, rcd, item, idx, event, eOpts) {
                                if (rcd.raw.leaf) {
                                    if (this.hasDbClickListener == true) {
                                        //WF.Center.openTab(rcd.raw.text, rcd.raw.url);
                                        var param = { 'str': rcd.raw.text, 'rightname': rcd.raw.rightname, 'managername': rcd.raw.managername, 'moduleno': rcd.raw.moduleno, 'id': rcd.raw.code, 'url': rcd.raw.url, 'suite': rcd.raw.suite, 'rightkey': rcd.raw.rightkey, 'functionname': rcd.raw.functionname };
                                        if (window.external.IsInWebBrowser == undefined) {
                                            if (rcd.raw.url.indexOf("exe") != -1) {
                                                var managerName = rcd.raw.managername;
                                                var rightkey = rcd.raw.rightkey;
                                                var errmsg;
                                                if (managerName == '' || managerName == null) {
                                                    errmsg = "WEB版无法打开PB菜单！";
                                                }
                                                else {
                                                    errmsg = "WEB版无法打开WinForm菜单！";
                                                }
                                                Ext.MessageBox.alert('提示', errmsg);
                                            } else {
                                                //window.parent.$OpenTab(rcd.raw.text, rcd.raw.url, JSON.stringify(param));
                                                OpenFunctionWeb(rcd.raw.url, rcd.raw.rightkey, rcd.raw.text, rcd.raw.moduleno, rcd.raw.suite);
                                            }
                                        } else {
                                            window.external.OpenFunction(rcd.raw.url, JSON.stringify(param));
                                        }
                                    }
                                }
                            }
                        }
                    });

                    if (sysFuncTree.hasRightClickMenu == true) {
                        qmenu.addListener('itemcontextmenu', function (view, rec, node, index, e) {
                            e.stopEvent();
                            var ngSysFuncTree = view.findParentByType('ngSysFuncTree');
                            if (rec.data.leaf == true) {
                                ngSysFuncTree.itemMenu.selectedNode = rec;
                                ngSysFuncTree.itemMenu.showAt(e.getXY());
                            } else {
                                ngSysFuncTree.containMenu.selectedNode = rec;
                                ngSysFuncTree.containMenu.showAt(e.getXY());
                            }
                            return false;
                        });
                        qmenu.addListener('containercontextmenu', function (treeP, e, eOpts) {
                            e.stopEvent();
                            var ngSysFuncTree = this.findParentByType('ngSysFuncTree');
                            ngSysFuncTree.containMenu.selectedNode = treeP.store.data.items[0];
                            ngSysFuncTree.containMenu.showAt(e.getXY());
                            return false;
                        })
                    }
                    if (sysFuncTree.hasSelection == true) {
                        qmenu.addListener('selectionchange', function (view, selected, eOpts) {
                            if (selected.length == 0) return;
                            else {
                                if (modified) {
                                    Ext.MessageBox.confirm('提示', '功能图标有修改，是否要保存？', function (btn) {
                                        if (btn == 'yes') {
                                            Save();
                                        }
                                        modified = false;
                                        cur_menuCode = selected[0].raw.code;
                                        gridStore.load({ params: { "code": cur_menuCode, "suite": view.view.panel.suiteName } });
                                    });
                                } else {
                                    modified = false;
                                    cur_menuCode = selected[0].raw.code;
                                    gridStore.load({ params: { "code": cur_menuCode, "suite": view.view.panel.suiteName } });
                                }
                            }
                        });
                    }
                    sysFuncTree.add(qmenu);
                    sysFuncTree.currentTree = qmenu;
                    Ext.apply(qmenu.getStore().proxy.extraParams, { condition: condition });//, flag: sysFuncTree.hasRightControl, treeFilter: sysFuncTree.treeFilter 
                    qmenu.getRootNode().expand();//加载第一个
                }
            }, {
                region: 'east',
                //width: 40,
                tooltip: '刷新',
                iconCls: 'icon-Refresh',
                anchor: '100%',
                handler: function () {
                    //alert('刷新');
                    var sysFuncTree = this.findParentByType('ngSysFuncTree');//找到根节点
                    //if (sysFuncTree.activeTab == null || sysFuncTree.activeTab.text =='搜索结果')
                    //{
                    this.ownerCt.queryById('query').setValue('');
                    //Ext.getCmp('query').setValue('');
                    //sysFuncTree.StoreloadMarsk.show();//loadData执行完hide
                    var itemsLength = sysFuncTree.items.items.length;
                    sysFuncTree.setActiveTab(sysFuncTree.items.items[itemsLength - 1]);
                    sysFuncTree.removeAll();
                    sysFuncTree.loadData();
                    //sysFuncTree.StoreloadMarsk.hide();
                    //}                    
                }
            }]
    }],
    listeners: {
        'afterrender': function (tabPanel, eOpts) {
            ////var result = Ext.getClass(tabPanel.tabBar.getLayout().overflowHandler).prototype.handleOverflow.apply(tabPanel.tabBar.getLayout().overflowHandler, arguments);
            ////向下滚动箭头长按计时器，每秒执行一次
            //var taskDown = {
            //    run: function () {
            //        var temp = tabPanel.tabBar.getLayout().overflowHandler;
            //        temp.scrollBy(20, false);
            //    },
            //    interval: 100 //1 second
            //}
            ////向下滚动箭头样式、鼠标覆盖样式、鼠标点击事件
            //var buttonDown = tabPanel.tabBar.body.createChild({
            //    cls: Ext.baseCSSPrefix + 'tab-scroll-arrow-down'
            //}, tabPanel.tabBar.body.child('.' + Ext.baseCSSPrefix + 'tab-scroll-arrow-down'));
            //buttonDown.addClsOnOver(Ext.baseCSSPrefix + 'tab-scroll-arrow-down-over');
            //buttonDown.on('click', function () {
            //    //tabPanel.tabBar.getLayout().overflowHandler.scrollBy(-20, false);
            //    var temp = tabPanel.tabBar.getLayout().overflowHandler;
            //    temp.scrollBy(20,false);
            //}, buttonDown);
            ////鼠标按下，开始计时
            //buttonDown.on('mousedown', function () {
            //    Ext.TaskManager.start(taskDown);
            //}, buttonDown);
            ////鼠标松开，停止计时
            //buttonDown.on('mouseup', function () {
            //    Ext.TaskManager.stop(taskDown);
            //}, buttonDown);
            ////向上滚动箭头长按计时器，每秒执行一次
            //var taskUp = {
            //    run: function () {
            //        var temp = tabPanel.tabBar.getLayout().overflowHandler;
            //        temp.scrollBy(-20, false);
            //    },
            //    interval: 100 //1 second
            //}
            //var buttonUp = tabPanel.tabBar.body.createChild({
            //    cls: Ext.baseCSSPrefix + 'tab-scroll-arrow-up'
            //}, tabPanel.tabBar.body.child('.' + Ext.baseCSSPrefix + 'tab-scroll-arrow-up'));
            //buttonUp.addClsOnOver(Ext.baseCSSPrefix + 'tab-scroll-arrow-up-over');
            //buttonUp.on('click', function () {
            //    //tabPanel.tabBar.getLayout().overflowHandler.scrollBy(-20, false);
            //    var temp = tabPanel.tabBar.getLayout().overflowHandler;
            //    temp.scrollBy(-20, false);
            //}, buttonUp);
            //buttonUp.on('mousedown', function () {
            //    Ext.TaskManager.start(taskUp);
            //}, buttonUp);
            //buttonUp.on('mouseup', function () {
            //    Ext.TaskManager.stop(taskUp);
            //}, buttonUp);
            ////找到包含原生滚动小箭头的div
            //var codeDiv = Ext.query("*[class=x-box-inner x-box-scroller-bottom]");
            ////往div插入自定义滚动小箭头
            ////codeDiv[1].innerHTML = '<div class="x-tab-tabmenu-right" ></div>';
            //if (codeDiv != null && codeDiv != '') {
            //    codeDiv[1].appendChild(buttonUp.dom);
            //    codeDiv[1].appendChild(buttonDown.dom);
            //}

            var parent = Ext.query("*[class=x-tab-bar-body x-tab-bar-body-default x-tab-bar-body-vertical x-tab-bar-body-default-vertical x-tab-bar-body-left x-tab-bar-body-default-left x-tab-bar-body-vertical-noborder x-tab-bar-body-default-vertical-noborder x-tab-bar-body-docked-left x-tab-bar-body-default-docked-left x-box-layout-ct x-tab-bar-body-default x-tab-bar-body-default-vertical x-tab-bar-body-default-left x-tab-bar-body-default-vertical-noborder x-tab-bar-body-default-docked-left]");
            //var leftChild = document.getElementById('tabbar - 1064 - body');
            if (parent != null & parent != '') {
                var topChild = Ext.query("*[class=x-box-inner x-box-scroller-top]");
                var bottomChild = Ext.query("*[class=x-box-inner x-box-scroller-bottom]");
                //通过panel找到scroller对象，修改滚动值
                var temp = tabPanel.tabBar.getLayout().overflowHandler;
                temp.scrollIncrement = 40;
                //topChild[i].children[0].scrollIncrement = 40;
                //bottomChild[i].children[0].scrollIncrement = 40;
                for (var i = 0; i < parent.length; i++) {
                    topChild[i].style.width = '34px';
                    bottomChild[i].style.width = '34px';
                    parent[i].removeChild(topChild[i]);
                    parent[i].removeChild(bottomChild[i]);
                    parent[i].appendChild(topChild[i]);
                    parent[i].appendChild(bottomChild[i]);
                }
            }
        },
        'tabchange': function (tabPanel, newCard, oldCard, eOpts) {
            //if (tabPanel.tabBar.getLayout().ownerContext) {
            //    alert(tabPanel.tabBar.getLayout().ownerContext.state.boxPlan.tooNarrow);
            //}
            tabPanel.currentTree = newCard;
            setCookie($appinfo.userID, newCard.suite);
            if (!newCard.loaded) {
                var me = newCard;
                Ext.apply(me.getStore().proxy.extraParams, { suite: me.suite });//,flag:tabPanel.hasRightControl,treeFilter:tabPanel.treeFilter
                me.getRootNode().expand();
                newCard.loaded = true;//已经加载
            }
        }
    }
});


//企业功能树
Ext.define('Ext.ng.enFuncTree', {
    extend: 'Ext.tab.Panel',
    alias: 'widget.ngEnFuncTree',
    tabPosition: 'left',
    hasRightClickMenu: true,
    isSystemUser: false,//是否是系统管理员
    currentTree: '',
    suites: [],
    hasSelection: false, //是否有更换图标的监听事件
    //menu:null,
    tabBar: {
        width: 30
    },
    initComponent: function () {
        var me = this;
        this.callParent();
        me.itemMenu = Ext.create('Ext.menu.Menu', {
            //selectedNode:null,
            items: [
                {
                    text: '添加到快捷菜单',
                    handler: function () {
                        var node = me.itemMenu.selectedNode;
                        var param = { 'str': node.raw.text, 'managername': node.raw.managername, 'moduleno': node.raw.moduleno, 'id': node.raw.code, 'url': node.raw.url, 'suite': node.raw.suite, 'rightkey': node.raw.rightkey, 'functionname': node.raw.functionname, 'busphid': node.raw.busphid };
                        //var param = { 'str': node.raw.text, 'rightname': '', 'managername': '', 'moduleno': '', 'id': node.raw.code, 'url': node.raw.url };

                        if (window.external.IsInWebBrowser == undefined) {
                            window.top.ShortcutsRefrech();
                        } else {
                            window.external.AddShortCutItem(JSON.stringify(param));
                        }
                        Ext.Ajax.request({
                            url: C_ROOT + 'SUP/ShortcutMenu/AddShortcutMenu',
                            params: {
                                'name': node.raw.text,
                                'originalcode': node.raw.code,
                                'itemurl': node.raw.url,
                                'busphid': node.raw.busphid
                            },
                            async: false
                            //,
                            //success: function (response) {
                            //    if (!Ext.isEmpty(response.responseText)) {
                            //        window.external.MessageShow(response.responseText);
                            //    }
                            //}
                        });

                    }
                }, {
                    text: '添加到我的桌面',
                    hidden: me.isSystemUser,
                    handler: function () {
                        var node = me.itemMenu.selectedNode;
                        Ext.Ajax.request({
                            url: C_ROOT + 'SUP/MyDesktopSet/AddMyDesktopNodeEx',
                            params: {
                                json: { 'text': node.raw.text, 'rightkey': node.raw.rightkey, 'managername': node.raw.managername, 'moduleno': node.raw.moduleno, 'code': node.raw.code, 'url': node.raw.url, 'suite': node.raw.suite, 'rightname': node.raw.rightname, 'busphid': node.raw.busphid },
                                groupname: node.parentNode.raw.text
                            },
                            async: false,
                            success: function (response) {
                                if (window.external.IsInWebBrowser == undefined) {
                                    return;
                                }
                                if (Ext.isEmpty(response.responseText)) {
                                    window.external.RefreshMyDesktop();
                                } else {
                                    window.external.MessageShow(response.responseText);
                                }
                            }
                        });
                    }
                }, '-',
                {
                    text: '全部展开',
                    handler: function () {
                        var node = me.itemMenu.selectedNode;
                        var root = findRoot(node)
                        expandTree(root);
                    }
                },
                {
                    text: '全部折叠',
                    handler: function () {
                        var node = me.itemMenu.selectedNode;
                        var root = findRoot(node)
                        collapseTree(root);
                    }
                }
            ]
        });
        me.containMenu = Ext.create('Ext.menu.Menu', {
            //selectedNode:null,
            items: [
                {
                    text: '全部展开',
                    handler: function () {
                        var node = me.containMenu.selectedNode;
                        var root = findRoot(node)
                        expandTree(root);
                    }
                },
                {
                    text: '全部折叠',
                    handler: function () {
                        var node = me.containMenu.selectedNode;
                        var root = findRoot(node)
                        collapseTree(root);
                    }
                }
            ]
        });
        me.loadData();
        me.tabBar.on('change', function (tabBar, tab, card, eOpts) {
            tabBar.needsScroll = false;//禁止tabBar乱滚动，抽风
        });
    },
    loadData: function () {
        var me = this;
        var arr = [];
        Ext.Ajax.request({
            url: C_ROOT + 'SUP/EnFuncTree/GetSuiteList',
            async: false,
            success: function (res, opts) {
                if (res.responseText.length > 0) {
                    var suites = Ext.JSON.decode(res.responseText);
                    if (suites.length == 0) {
                        me.html = '<strong>企业功能树为空，请先设置企业功能树!</strong>';
                        //me.title = '企业功能树为空，请先设置企业功能树!';
                    }
                    for (var i = 0; i < suites.length; i++) {
                        var menu = Ext.create('Ext.ng.TreePanel', {
                            autoLoad: false,
                            height: 600,
                            split: true,
                            border: false,
                            //title: suites[i].Name,//title,
                            suiteName: suites[i].Name,
                            title: TranTitle(suites[i].Name),
                            suite: suites[i].Code,//加载套件是根据套件的标示字段
                            layout: 'fit',
                            treeFields: [{ name: 'text', type: 'string' },
                                { name: 'originalsuite', type: 'string' },
                                { name: 'my', type: 'string' }//我的自定义属性
                            ],
                            url: C_ROOT + 'SUP/EnFuncTree/LoadMenu', //'HR/EmpInfoList/LoadMenu'
                            listeners: {
                                'afterrender': function () {

                                },
                                'expand': function () {
                                    var me = this;
                                    Ext.apply(this.getStore().proxy.extraParams, { suite: me.suite });
                                    this.getRootNode().expand();
                                },
                                'activate': function () {
                                    var me = this;
                                    Ext.apply(this.getStore().proxy.extraParams, { suite: me.suite });
                                    //this.getRootNode().expand();//有些电脑会全部出发这个事件，达不到懒加载目的
                                },
                                'itemdblclick': function (view, rcd, item, idx, event, eOpts) {
                                    if (rcd.raw.leaf) {
                                        //WF.Center.openTab(rcd.raw.text, rcd.raw.url);
                                        var param = { 'str': rcd.raw.text, 'rightname': rcd.raw.rightname, 'managername': rcd.raw.managername, 'moduleno': rcd.raw.moduleno, 'id': rcd.raw.code, 'url': rcd.raw.url, 'suite': rcd.data.originalsuite, 'rightkey': rcd.raw.rightkey, 'functionname': rcd.raw.functionname };
                                        if (window.external.IsInWebBrowser == undefined) {
                                            if (rcd.raw.url.indexOf("exe") != -1) {
                                                var managerName = rcd.raw.managername;
                                                var rightkey = rcd.raw.rightkey;
                                                var errmsg;
                                                if (managerName == '' || managerName == null) {
                                                    errmsg = "WEB版无法打开PB菜单！";
                                                }
                                                else {
                                                    errmsg = "WEB版无法打开WinForm菜单！";
                                                }
                                                Ext.MessageBox.alert('提示', errmsg);
                                            } else {
                                                //window.parent.$OpenTab(rcd.raw.text, rcd.raw.url, JSON.stringify(param));
                                                OpenFunctionWeb(rcd.raw.url, rcd.raw.rightkey, rcd.raw.text, rcd.raw.moduleno, rcd.data.originalsuite);
                                            }
                                        } else {
                                            window.external.OpenFunction(rcd.raw.url, JSON.stringify(param));
                                        }
                                    }
                                },
                                //'itemcontextmenu': function (view, rec, node, index, e) {
                                //    e.stopEvent();
                                //    var ngEnFuncTree = this.findParentByType('ngEnFuncTree');
                                //    ngEnFuncTree.itemMenu.selectedNode = rec;
                                //    ngEnFuncTree.itemMenu.showAt(e.getXY());
                                //    return false;
                                //},
                                //'containercontextmenu': function (treeP, e, eOpts) {
                                //    e.stopEvent();
                                //    var ngEnFuncTree = this.findParentByType('ngEnFuncTree');
                                //    ngEnFuncTree.containMenu.selectedNode = treeP.store.data.items[0];
                                //    ngEnFuncTree.containMenu.showAt(e.getXY());
                                //    return false;
                                //}
                            }
                        });
                        if (me.hasRightClickMenu == true) {
                            menu.addListener(
                                'itemcontextmenu', function (view, rec, node, index, e) {
                                    e.stopEvent();
                                    var ngEnFuncTree = view.findParentByType('ngEnFuncTree');
                                    if (rec.data.leaf == true) {
                                        ngEnFuncTree.itemMenu.selectedNode = rec;
                                        ngEnFuncTree.itemMenu.showAt(e.getXY());
                                    } else {
                                        ngEnFuncTree.containMenu.selectedNode = rec;
                                        ngEnFuncTree.containMenu.showAt(e.getXY());
                                    }
                                    return false;
                                });
                            menu.addListener('containercontextmenu', function (treeP, e, eOpts) {
                                e.stopEvent();
                                var ngEnFuncTree = this.findParentByType('ngEnFuncTree');
                                ngEnFuncTree.containMenu.selectedNode = treeP.store.data.items[0];
                                ngEnFuncTree.containMenu.showAt(e.getXY());
                                return false;
                            })
                        }
                        if (me.hasSelection == true) {
                            menu.addListener('selectionchange', function (view, selected, eOpts) {
                                if (selected.length == 0) return;
                                else {
                                    if (modified) {
                                        Ext.MessageBox.confirm('提示', '功能图标有修改，是否要保存？', function (btn) {
                                            if (btn == 'yes') {
                                                Save();
                                            }
                                            modified = false;
                                            cur_menuCode = selected[0].raw.code;
                                            gridStore.load({ params: { "code": cur_menuCode, "suite": view.view.panel.suiteName } });
                                        });
                                    } else {
                                        modified = false;
                                        cur_menuCode = selected[0].raw.code;
                                        gridStore.load({ params: { "code": cur_menuCode, "suite": view.view.panel.suiteName } });
                                    }
                                }
                            });
                        }
                        arr.push(menu);
                    }
                    me.suites = arr; //第一次加载把arr存在suites中
                    me.add(arr);
                    if (arr.length > 0) {
                        Ext.apply(arr[0].getStore().proxy.extraParams, { suite: arr[0].suite });
                        arr[0].getRootNode().expand();//加载第一个
                        me.currentTree = arr[0];
                    }

                    me.setActiveTab(me.items.items[0]);
                }
            }
        });// 画出左侧菜单树    
    },
    //height: 300,
    //width: 300,
    itemId: 'enFuncTree',
    dockedItems: [{
        xtype: 'toolbar',
        //width: 300,
        //height: 26,
        height: 30,
        dock: 'top',
        layout: 'border',
        minWidth: 100,
        items: [
            {
                region: 'center',
                xtype: 'textfield',
                itemId: 'query',
                name: 'queryname',
                emptyText: '搜索内容',
                enableKeyEvents: true,
                listeners: {
                    'keydown': function (el, e, eOpts) {
                        if (e.getKey() == e.ENTER) {
                            var condition = this.ownerCt.queryById('query').getValue();
                            //alert(condition);
                            if (condition == '' || condition == null) {
                                //alert('请输入搜索内容');
                                return;
                            };
                            var enFuncTree = this.findParentByType('ngEnFuncTree');//找到根节点
                            var itemsLength = enFuncTree.items.items.length;
                            enFuncTree.setActiveTab(enFuncTree.items.items[itemsLength - 1]);
                            enFuncTree.removeAll();
                            var orgtitle = "搜索结果";

                            var qmenu = Ext.create('Ext.ng.TreePanel', {
                                autoLoad: false,
                                height: 600,
                                split: true,
                                border: false,
                                text: "搜索结果",
                                title: TranTitle(orgtitle),
                                layout: 'fit',
                                treeFields: [{ name: 'text', type: 'string' },
                                     { name: 'originalsuite', type: 'string' },
                                    { name: 'my', type: 'string' }//我的自定义属性
                                ],
                                url: C_ROOT + 'SUP/EnFuncTree/Query',
                                listeners: {
                                    'afterrender': function () {
                                    },
                                    'expand': function () {
                                        var me = this;
                                        Ext.apply(this.getStore().proxy.extraParams, { suite: me.suite });
                                        this.getRootNode().expand();
                                    },
                                    'activate': function () {
                                        var me = this;
                                        Ext.apply(this.getStore().proxy.extraParams, { suite: me.suite });
                                        //this.getRootNode().expand();//有些电脑会全部出发这个事件，达不到懒加载目的
                                    },
                                    'itemdblclick': function (view, rcd, item, idx, event, eOpts) {
                                        if (rcd.raw.leaf) {
                                            //WF.Center.openTab(rcd.raw.text, rcd.raw.url);
                                            var param = { 'str': rcd.raw.text, 'rightname': rcd.raw.rightname, 'managername': rcd.raw.managername, 'moduleno': rcd.raw.moduleno, 'id': rcd.raw.code, 'url': rcd.raw.url, 'suite': rcd.data.originalsuite, 'rightkey': rcd.raw.rightkey, 'functionname': rcd.raw.functionname };
                                            if (window.external.IsInWebBrowser == undefined) {
                                                if (rcd.raw.url.indexOf("exe") != -1) {
                                                    var managerName = rcd.raw.managername;
                                                    var rightkey = rcd.raw.rightkey;
                                                    var errmsg;
                                                    if (managerName == '' || managerName == null) {
                                                        errmsg = "WEB版无法打开PB菜单！";
                                                    }
                                                    else {
                                                        errmsg = "WEB版无法打开WinForm菜单！";
                                                    }
                                                    Ext.MessageBox.alert('提示', errmsg);
                                                } else {
                                                    //window.parent.$OpenTab(rcd.raw.text, rcd.raw.url, JSON.stringify(param));
                                                    OpenFunctionWeb(rcd.raw.url, rcd.raw.rightkey, rcd.raw.text, rcd.raw.moduleno, rcd.data.originalsuite);
                                                }
                                            } else {
                                                window.external.OpenFunction(rcd.raw.url, JSON.stringify(param));
                                            }
                                        }
                                    }
                                }
                            });
                            if (enFuncTree.hasSelection == true) {
                                qmenu.addListener('selectionchange', function (view, selected, eOpts) {
                                    if (selected.length == 0) return;
                                    else {
                                        if (modified) {
                                            Ext.MessageBox.confirm('提示', '功能图标有修改，是否要保存？', function (btn) {
                                                if (btn == 'yes') {
                                                    Save();
                                                }
                                                modified = false;
                                                cur_menuCode = selected[0].raw.code;
                                                gridStore.load({ params: { "code": cur_menuCode, "suite": view.view.panel.suiteName } });
                                            });
                                        } else {
                                            modified = false;
                                            cur_menuCode = selected[0].raw.code;
                                            gridStore.load({ params: { "code": cur_menuCode, "suite": view.view.panel.suiteName } });
                                        }
                                    }
                                });
                            }
                            enFuncTree.add(qmenu);
                            Ext.apply(qmenu.getStore().proxy.extraParams, { condition: condition });
                            qmenu.getRootNode().expand();//加载第一个
                            enFuncTree.currentTree = qmenu;
                            return false;
                        }
                    }
                }
                //minWidth: 80             
            }, {
                region: 'east',
                //width: 40,
                tooltip: '搜索',
                iconCls: 'icon-Query',
                handler: function () {
                    var condition = this.ownerCt.queryById('query').getValue();
                    //alert(condition);
                    if (condition == '' || condition == null) {
                        //alert('请输入搜索内容');
                        return;
                    };
                    var enFuncTree = this.findParentByType('ngEnFuncTree');//找到根节点
                    var itemsLength = enFuncTree.items.items.length;
                    enFuncTree.setActiveTab(enFuncTree.items.items[itemsLength - 1]);
                    enFuncTree.removeAll();
                    var orgtitle = "搜索结果";
                    var qmenu = Ext.create('Ext.ng.TreePanel', {
                        autoLoad: false,
                        height: 600,
                        split: true,
                        border: false,
                        text: "搜索结果",
                        title: TranTitle(orgtitle),
                        layout: 'fit',
                        treeFields: [{ name: 'text', type: 'string' },
                             { name: 'originalsuite', type: 'string' },
                            { name: 'my', type: 'string' }//我的自定义属性
                        ],
                        url: C_ROOT + 'SUP/EnFuncTree/Query',
                        listeners: {
                            'afterrender': function () {
                            },
                            'expand': function () {
                                var me = this;
                                Ext.apply(this.getStore().proxy.extraParams, { suite: me.suite });
                                this.getRootNode().expand();
                            },
                            'activate': function () {
                                var me = this;
                                Ext.apply(this.getStore().proxy.extraParams, { suite: me.suite });
                                //this.getRootNode().expand();//有些电脑会全部出发这个事件，达不到懒加载目的
                            },
                            'itemdblclick': function (view, rcd, item, idx, event, eOpts) {
                                if (rcd.raw.leaf) {
                                    //WF.Center.openTab(rcd.raw.text, rcd.raw.url);
                                    var param = { 'str': rcd.raw.text, 'rightname': rcd.raw.rightname, 'managername': rcd.raw.managername, 'moduleno': rcd.raw.moduleno, 'id': rcd.raw.code, 'url': rcd.raw.url, 'suite': rcd.data.originalsuite, 'rightkey': rcd.raw.rightkey, 'functionname': rcd.raw.functionname };
                                    if (window.external.IsInWebBrowser == undefined) {
                                        if (rcd.raw.url.indexOf("exe") != -1) {
                                            var managerName = rcd.raw.managername;
                                            var rightkey = rcd.raw.rightkey;
                                            var errmsg;
                                            if (managerName == '' || managerName == null) {
                                                errmsg = "WEB版无法打开PB菜单！";
                                            }
                                            else {
                                                errmsg = "WEB版无法打开WinForm菜单！";
                                            }
                                            Ext.MessageBox.alert('提示', errmsg);
                                        } else {
                                            //window.parent.$OpenTab(rcd.raw.text, rcd.raw.url, JSON.stringify(param));
                                            OpenFunctionWeb(rcd.raw.url, rcd.raw.rightkey, rcd.raw.text, rcd.raw.moduleno, rcd.data.originalsuite);
                                        }
                                    } else {
                                        window.external.OpenFunction(rcd.raw.url, JSON.stringify(param));
                                    }
                                }
                            }
                        }
                    });
                    if (enFuncTree.hasSelection == true) {
                        qmenu.addListener('selectionchange', function (view, selected, eOpts) {
                            if (selected.length == 0) return;
                            else {
                                if (modified) {
                                    Ext.MessageBox.confirm('提示', '功能图标有修改，是否要保存？', function (btn) {
                                        if (btn == 'yes') {
                                            Save();
                                        }
                                        modified = false;
                                        cur_menuCode = selected[0].raw.code;
                                        gridStore.load({ params: { "code": cur_menuCode, "suite": view.view.panel.suiteName } });
                                    });
                                } else {
                                    modified = false;
                                    cur_menuCode = selected[0].raw.code;
                                    gridStore.load({ params: { "code": cur_menuCode, "suite": view.view.panel.suiteName } });
                                }
                            }
                        });
                    }
                    enFuncTree.add(qmenu);

                    Ext.apply(qmenu.getStore().proxy.extraParams, { condition: condition });
                    qmenu.getRootNode().expand();//加载第一个
                }
            }, {
                region: 'east',
                //width: 40,
                tooltip: '刷新',
                iconCls: 'icon-Refresh',
                anchor: '100%',
                handler: function () {
                    //alert('刷新');
                    this.ownerCt.queryById('query').setValue('');
                    //Ext.getCmp('query').setValue('');
                    var enFuncTree = this.findParentByType('ngEnFuncTree');//找到根节点
                    var itemsLength = enFuncTree.items.items.length;
                    enFuncTree.setActiveTab(enFuncTree.items.items[itemsLength - 1]);
                    enFuncTree.removeAll();
                    enFuncTree.loadData();
                }
            }]
    }],
    listeners: {
        'afterrender': function (tabPanel, eOpts) {
            var parent = Ext.query("*[class=x-tab-bar-body x-tab-bar-body-default x-tab-bar-body-vertical x-tab-bar-body-default-vertical x-tab-bar-body-left x-tab-bar-body-default-left x-tab-bar-body-vertical-noborder x-tab-bar-body-default-vertical-noborder x-tab-bar-body-docked-left x-tab-bar-body-default-docked-left x-box-layout-ct x-tab-bar-body-default x-tab-bar-body-default-vertical x-tab-bar-body-default-left x-tab-bar-body-default-vertical-noborder x-tab-bar-body-default-docked-left]");
            //var leftChild = document.getElementById('tabbar - 1064 - body');
            if (parent != null & parent != '') {
                for (var i = 0; i < parent.length; i++) {
                    var topChild = Ext.query("*[class=x-box-inner x-box-scroller-top]");
                    var bottomChild = Ext.query("*[class=x-box-inner x-box-scroller-bottom]");
                    topChild[i].style.width = '34px';
                    bottomChild[i].style.width = '34px';
                    parent[i].removeChild(topChild[i]);
                    parent[i].removeChild(bottomChild[i]);
                    parent[i].appendChild(topChild[i]);
                    parent[i].appendChild(bottomChild[i]);
                }
            }
        },
        'tabchange': function (tabPanel, newCard, oldCard, eOpts) {
            tabPanel.currentTree = newCard;
            if (!newCard.loaded) {
                var me = newCard;
                Ext.apply(me.getStore().proxy.extraParams, { suite: me.suite });
                me.getRootNode().expand();
                newCard.loaded = true;//已经加载
            }
        },
    }
});
//通过节点node找到树的根节点
function findRoot(node) {
    while (!node.isRoot()) {
        node = node.parentNode;
    }
    return node;
}

//展开根为root的树的所有节点
function expandTree(root) {
    if (root.hasChildNodes()) {
        root.expand();
        for (var i = 0; i < root.childNodes.length; i++) {
            expandTree(root.childNodes[i]);
        }
    }

}

//折叠根为root的树的所有节点
function collapseTree(root) {
    if (root.hasChildNodes()) {
        //root.collapse();
        for (var i = 0; i < root.childNodes.length; i++) {
            if (root.childNodes[i].hasChildNodes()) {
                root.childNodes[i].collapse();
                collapseTree(root.childNodes[i]);
            }

        }
    }

}




//我的功能树
Ext.define('Ext.ng.myFuncTree', {
    extend: 'Ext.ng.TreePanel',
    draggable: false,
    //extend: 'Ext.tab.Panel',
    autoLoad: false,
    hasRightClickMenu: true, // 右键菜单开关 true/false
    isSystemUser: false,
    hasToolbar: true, // 定位栏开关 true/false
    hasIndividualButton: false,// 是否有自定义我的功能树按钮 true/false 
    hasDbClickListener: true, // 双击节点打开对应页面的开关 true/false
    alias: 'widget.ngMyFuncTree',
    initComponent: function () {
        var me = this;
        me.itemMenu = Ext.create('Ext.menu.Menu', {
            items: [
                {
                    text: '自定义功能树',
                    //handler: function () {
                    //    var node = me.getSelectionModel().getSelection()[0];
                    //    var param = { 'str': node.raw.text, 'managername': node.raw.managername, 'moduleno': node.raw.moduleno, 'id': node.raw.originalcode, 'url': node.raw.url, 'suite': node.raw.suite };
                    //    window.external.AddShortCutItem(JSON.stringify(param));
                    //}
                    handler: function () {
                        $OpenTab("自定义", C_ROOT + 'SUP/MainTree/CustomMyFuncTree');
                    }
                }, {
                    text: '添加到我的桌面',
                    hidden: me.isSystemUser,
                    handler: function () {
                        var node = me.itemMenu.selectedNode;
                        if (node.parentNode.raw.text != '' && node.parentNode.raw.text != null && node.parentNode.raw.text != 'Root') {
                            var groupname = node.parentNode.raw.text;
                        } else {
                            var groupname = '自定义';
                        };
                        Ext.Ajax.request({
                            url: C_ROOT + 'SUP/MyDesktopSet/AddMyDesktopNodeEx',
                            params: {
                                json: { 'text': node.raw.text, 'managername': node.raw.managername, 'moduleno': node.raw.moduleno, 'code': node.raw.phid, 'url': node.raw.url, 'suite': node.raw.suite, 'rightkey': node.raw.rightkey, 'rightname': node.raw.rightname, 'busphid': node.raw.busphid },
                                groupname: groupname
                            },
                            async: false,
                            success: function (response) {
                                if (window.external.IsInWebBrowser == undefined) {
                                    return;
                                }
                                if (Ext.isEmpty(response.responseText)) {
                                    window.external.RefreshMyDesktop();
                                } else {
                                    window.external.MessageShow(response.responseText);
                                }
                            }
                        });
                    }
                }, '-',
                {
                    text: '全部展开',
                    handler: function () {
                        var node = me.itemMenu.selectedNode;
                        var root = findRoot(node)
                        expandTree(root);
                    }
                },
                {
                    text: '全部折叠',
                    handler: function () {
                        var node = me.itemMenu.selectedNode;
                        var root = findRoot(node)
                        collapseTree(root);
                    }
                }
                //{
                //    text: '全部展开',
                //    handler: function () {
                //        var root = me.menu.rootNode;
                //        expandTree(root);
                //    }
                //}, 
                //{
                //    text: '全部折叠',
                //    handler: function () {
                //        var root = me.menu.rootNode;
                //        collapseTree(root);
                //    }
                //}
            ]
        });
        me.containMenu = Ext.create('Ext.menu.Menu', {
            items: [
                {
                    text: '自定义功能树',
                    //handler: function () {
                    //    var node = me.getSelectionModel().getSelection()[0];
                    //    var param = { 'str': node.raw.text, 'managername': node.raw.managername, 'moduleno': node.raw.moduleno, 'id': node.raw.originalcode, 'url': node.raw.url, 'suite': node.raw.suite };
                    //    window.external.AddShortCutItem(JSON.stringify(param));
                    //}
                    handler: function () {
                        $OpenTab("自定义", C_ROOT + 'SUP/MainTree/CustomMyFuncTree');
                    }
                }, '-',
                {
                    text: '全部展开',
                    handler: function () {
                        var node = me.containMenu.selectedNode;
                        var root = findRoot(node)
                        expandTree(root);
                    }
                },
                {
                    text: '全部折叠',
                    handler: function () {
                        var node = me.containMenu.selectedNode;
                        var root = findRoot(node)
                        collapseTree(root);
                    }
                }
            ]
        });

        //根据hasToolbar参数判断是否添加自定义我的功能树按钮
        //根据hasToolbar
        if (me.hasToolbar == true && me.hasIndividualButton == true) {
            me.dockedItems = [{
                xtype: 'toolbar',
                dock: 'top',
                layout: 'border',
                minWidth: 100,
                //height: 26,
                height: 30,
                items: [{
                    itemId: 'add',
                    region: 'west',
                    //width: 45,
                    //text: '自定义',
                    tooltip: '自定义',
                    iconCls: 'add',
                    handler: function () {
                        var top = window.parent;
                        $OpenTab("自定义", C_ROOT + 'SUP/MainTree/CustomMyFuncTree');
                        // top.OpenTab(C_ROOT + 'Student/MyFuncTree2', '1', '自定义');
                    }
                }, {
                    region: 'center',
                    minWidth: 100,
                    grow: true,
                    xtype: 'textfield',
                    itemId: 'queryKey',
                    name: 'queryKey',
                    emptyText: '定位节点',
                    enableKeyEvents: true,
                    listeners: {
                        'keydown': function (el, e, eOpts) {
                            if (e.getKey() == e.ENTER) {
                                var toolbar = this.findParentByType('toolbar');//找到根节点
                                var condition = toolbar.queryById('queryKey').value;
                                if (condition == '' || condition == null) {
                                    //alert('请输入定位关键字');
                                    return;
                                }
                                var myFuncTree = this.findParentByType('ngMyFuncTree');
                                findNodeByFuzzy(myFuncTree, condition);
                                return false;
                            }
                        }
                    }
                },
            {
                //text: '定位',
                itemId: 'query',
                region: 'east',
                tooltip: '定位',
                iconCls: 'icon-Location',
                //width: 30,
                handler: function () {
                    var toolbar = this.findParentByType('toolbar');//找到根节点
                    var condition = toolbar.queryById('queryKey').value;
                    if (condition == '' || condition == null) {
                        //alert('请输入定位关键字');
                        return;
                    }
                    var myFuncTree = this.findParentByType('ngMyFuncTree');
                    findNodeByFuzzy(myFuncTree, condition);
                }
            }]
            }]
        } else if (me.hasToolbar == true) {
            me.dockedItems = [{
                xtype: 'toolbar',
                dock: 'top',
                layout: 'border',
                minWidth: 100,
                //height: 26,
                height: 30,
                items: [{
                    region: 'center',
                    minWidth: 100,
                    grow: true,
                    xtype: 'textfield',
                    itemId: 'queryKey',
                    name: 'queryKey',
                    emptyText: '定位节点',
                    enableKeyEvents: true,
                    listeners: {
                        'keydown': function (el, e, eOpts) {
                            if (e.getKey() == e.ENTER) {
                                var toolbar = this.findParentByType('toolbar');//找到根节点
                                var condition = toolbar.queryById('queryKey').value;
                                if (condition == '' || condition == null) {
                                    //alert('请输入定位关键字');
                                    return;
                                }
                                var myFuncTree = this.findParentByType('ngMyFuncTree');
                                findNodeByFuzzy(myFuncTree, condition);
                                return false;
                            }
                        }
                    }
                },
            {
                //text: '定位',
                itemId: 'query',
                region: 'east',
                tooltip: '定位',
                iconCls: 'icon-Location',
                //width: 30,
                handler: function () {
                    var toolbar = this.findParentByType('toolbar');//找到根节点
                    var condition = toolbar.queryById('queryKey').value;
                    if (condition == '' || condition == null) {
                        //alert('请输入定位关键字');
                        return;
                    }
                    var myFuncTree = this.findParentByType('ngMyFuncTree');
                    findNodeByFuzzy(myFuncTree, condition);
                }
            }]
            }]
        }
        if (me.hasRightClickMenu == true) {
            me.addListener('itemcontextmenu', function (view, rec, node, index, e) {
                e.stopEvent();
                var myFuncTree = view.findParentByType('ngMyFuncTree');
                var root = view.findParentByType('ngMyFuncTree').getRootNode();
                if (rec.data.leaf == true) {
                    myFuncTree.itemMenu.rootNode = root;
                    myFuncTree.itemMenu.selectedNode = rec;
                    myFuncTree.itemMenu.showAt(e.getXY());
                } else {
                    myFuncTree.containMenu.rootNode = root;
                    myFuncTree.containMenu.selectedNode = rec;
                    myFuncTree.containMenu.showAt(e.getXY());
                }

                return false;
            });
            me.addListener('containercontextmenu', function (treeP, e, eOpts) {
                e.stopEvent();
                var ngMyFuncTree = this;
                var root = this.getRootNode();
                this.containMenu.rootNode = root;
                ngMyFuncTree.containMenu.selectedNode = treeP.store.data.items[0];
                ngMyFuncTree.containMenu.showAt(e.getXY());
                return false;
            })
        }
        this.callParent();
    },
    height: 300,
    width: 300,
    split: true,
    border: false,
    rootVisible: false,
    layout: 'fit',
    treeFields: [{ name: 'text', type: 'string' },
        { name: 'phid', type: 'int' },
        { name: 'originalcode', type: 'string' },
        { name: 'name', type: 'string' },
        { name: 'originalid', type: 'string' },
        { name: 'pid', type: 'string' },
        { name: 'url', type: 'string' },
        { name: 'userid', type: 'int' },
    ],
    url: C_ROOT + 'SUP/MyFuncTree/LoadMyFuncTree',
    //dockedItems:me.docitems,
    //    [{
    //    xtype: 'toolbar',
    //    dock: 'top',
    //    layout: 'border',
    //    minWidth: 200,
    //    height: 26,
    //    items: [{
    //        itemId: 'add',
    //        region: 'west',
    //        //width: 45,
    //        //text: '自定义',
    //        tooltip: '自定义',
    //        iconCls: 'add',
    //        handler: function () {
    //            var top = window.parent;
    //            $OpenTab("自定义", C_ROOT + 'SUP/MainTree/CustomMyFuncTree');
    //           // top.OpenTab(C_ROOT + 'Student/MyFuncTree2', '1', '自定义');
    //        }
    //    }, {
    //        region: 'center',
    //        minWidth: 100,
    //        grow: true,
    //        xtype: 'textfield',
    //        itemId: 'queryKey',
    //        name: 'queryKey',
    //        emptyText: '定位节点',
    //        enableKeyEvents: true,
    //        listeners: {
    //            'keydown': function (el, e, eOpts) {
    //                if (e.getKey() == e.ENTER) {
    //                    var toolbar = this.findParentByType('toolbar');//找到根节点
    //                    var condition = toolbar.items.items[1].value;
    //                    if (condition == '' || condition == null) {
    //                        //alert('请输入定位关键字');
    //                        return;
    //                    }
    //                    var myFuncTree = this.findParentByType('ngMyFuncTree');
    //                    findNodeByFuzzy(myFuncTree, condition);
    //                    return false;
    //                }
    //            }
    //        }
    //    },
    //{
    //    //text: '定位',
    //    itemId: 'query',
    //    region: 'east',
    //    tooltip: '定位',
    //    iconCls: 'icon-Location',
    //    //width: 30,
    //    handler: function () {
    //        var toolbar = this.findParentByType('toolbar');//找到根节点
    //        var condition = toolbar.items.items[1].value;
    //        if (condition == '' || condition == null) {
    //            //alert('请输入定位关键字');
    //            return;
    //        }
    //        var myFuncTree = this.findParentByType('ngMyFuncTree');
    //        findNodeByFuzzy(myFuncTree, condition);
    //    }
    //}]
    //}],
    refreshData: function () {
        var me = this;
        me.getRootNode().removeAll();
        me.getStore().setProxy(
            {
                type: 'ajax',
                url: C_ROOT + 'SUP/MyFuncTree/LoadMyFuncTree',
            }
        );
        me.getStore().load();
        me.getView().refresh();
    },
    listeners: {
        'afterrender': function () {
            this.getRootNode().expand();
            if (window.external.IsInWebBrowser == undefined) {
                return;
            }
            window.external.EnableDockBtn();
        },
        'itemclick': function (view, rcd, item, idx, event, eOpts) {
        },
        'itemdblclick': function (view, rcd, item, idx, event, eOpts) {
            if (this.hasDbClickListener == true) {
                if (rcd.raw.leaf) {
                    var url;
                    var param;
                    if (rcd.raw.originalcode == '') {
                        if (window.external.IsInWebBrowser == undefined) {
                            Ext.MessageBox.alert('提示', "web版暂不支持导入网址与文件功能");
                            return;
                        }
                        if (rcd.raw.url.substring(0, 7) == 'http://') {
                            url = 'WebBrowseIndividualManager№,№Caption№=№' + rcd.raw.name + '№,№Url№=№' + rcd.raw.url + rcd.raw.urlparm;
                            param = { 'str': rcd.raw.name, 'rightname': rcd.raw.rightname, 'managername': rcd.raw.managername, 'moduleno': rcd.raw.moduleno, 'id': rcd.raw.code, 'url': rcd.raw.url, 'suite': rcd.raw.suite, 'rightkey': rcd.raw.rightkey, 'functionname': rcd.raw.functionname };
                        } else {
                            url = 'LocalSoft' + rcd.raw.url + '№,№' + rcd.raw.urlparm;
                            param = { 'str': rcd.raw.name, 'rightname': rcd.raw.rightname, 'managername': rcd.raw.managername, 'moduleno': rcd.raw.moduleno, 'id': rcd.raw.code, 'url': rcd.raw.url, 'suite': rcd.raw.suite, 'rightkey': rcd.raw.rightkey, 'functionname': rcd.raw.functionname };
                        }
                    } else {
                        url = rcd.raw.url;
                        param = { 'str': rcd.raw.name, 'rightname': rcd.raw.rightname, 'managername': rcd.raw.managername, 'moduleno': rcd.raw.moduleno, 'id': rcd.raw.code, 'url': rcd.raw.url, 'suite': rcd.raw.suite, 'rightkey': rcd.raw.rightkey, 'functionname': rcd.raw.functionname, 'opentype': 'myfunction' };
                    }
                    if (window.external.IsInWebBrowser == undefined) {
                        if (rcd.raw.url.indexOf("exe") != -1) {
                            var managerName = rcd.raw.managername;
                            var rightkey = rcd.raw.rightkey;
                            var errmsg;
                            if (managerName == '' || managerName == null) {
                                errmsg = "WEB版无法打开PB菜单！";
                            }
                            else {
                                errmsg = "WEB版无法打开WinForm菜单！";
                            }
                            Ext.MessageBox.alert('提示', errmsg);
                        } else {
                            //window.parent.$OpenTab(rcd.raw.text, rcd.raw.url, JSON.stringify(param));
                            OpenFunctionWeb(rcd.raw.url, rcd.raw.rightkey, rcd.raw.text, rcd.raw.moduleno, rcd.raw.suite);
                        }
                    } else {
                        window.external.OpenFunction(rcd.raw.url, JSON.stringify(param));
                    }
                }
            }
        },
        'checkchange': function (node, checked) {
            setChildNodeChecked(node, checked); //同时选择下级
        }

    }
});

//我的功能树中节点定位函数，在myFuncTree中调用
function findNodeByFuzzy(tree, value) {
    if (value == "") { return; }
    var me = tree, index = -1;
    var firstFind = false;
    if (isNaN(me.nodeIndex) || me.nodeIndex == null || me.value != value) {
        me.nodeIndex = -1;
        me.value = value;
    }
    var findNode = tree.getRootNode().findChildBy(function (node) {
        index++;
        //if (!node.data.root && index > me.nodeIndex && (node.data.text.indexOf(value) > -1 || node.data.bopomofo.indexOf(value.toUpperCase()) > -1)) {
        if (!node.data.root && index > me.nodeIndex && (node.data.text.indexOf(value) > -1)) {
            return true;
        }
    }, null, true);
    me.nodeIndex = index;
    if (findNode) {
        tree.selectPath(findNode.getPath());
    }
    else {
        if (firstFind) {
            Ext.MessageBox.alert('', '没有匹配的树节点.');
        }
        me.nodeIndex = -1;
    }
};

//自定义toolbar的按钮样式控制函数，在FuncTreebar中调用
function RemoveCls(toolbar1, toolbar2) {
    for (var i = 0; i < toolbar1.items.length; i++) {
        toolbar1.items.items[i].removeCls('x-btn-default-toolbar-small-myfocus');
    }
    if (toolbar2 != null) {
        for (var i = 0; i < toolbar2.items.length; i++) {
            toolbar2.items.items[i].removeCls('x-btn-default-toolbar-small-myfocus');
        }
    }

};

//tabpanel右键菜单
//Ext.define('Ext.ng.TabMenu', {
//    extend: 'Ext.ux.TabCloseMenu',
//    alias: 'widget.ngFuncTabPanel',
//    myItems: [],
//    createMenu: function () {
//        var me = this;
//        if (!me.menu) {
//            var items = me.myItems;          
//            me.menu = Ext.create('Ext.menu.Menu', {
//                items: items,
//                listeners: {
//                    hide: me.onHideMenu,
//                    scope: me
//                }
//            });
//        }
//        return me.menu;
//    },
//    onContextMenu : function(event, target){
//        var me = this,
//            menu = me.createMenu(),
//            disableAll = true,
//            disableOthers = true,
//            tab = me.tabBar.getChildByElement(target),
//            index = me.tabBar.items.indexOf(tab);
//        me.item = me.tabPanel.getComponent(index);    
//        event.preventDefault();
//        me.fireEvent('beforemenu', menu, me.item, me);
//        menu.showAt(event.getXY());
//}
//});

//主菜单上方两个tabpanel
Ext.define('Ext.ng.FuncTabPanel', {
    extend: 'Ext.tab.Panel',
    //height: 23,
    height: 28,
    alias: 'widget.ngFuncTabPanel', //别名,可通过设置xtype构建,或者通过Ext.widget()方法构建 
    hasCtl: '',//是否有右键菜单
    optionItems: '',//改tab所有默认项目
    items: '',//用户现有项目
    brother: null,//同等级tab
    containerFather: '',//父容器
    itemId: '',
    tabItems: [],//右键菜单的所有项目
    menus: [],
    tabMenu: [],    
    existItemsId: [],
    tabBar:{
        height: 30,
    },
    tabScrollArrow: false,//自定义,一个页面多个该控件只能有一个为true
    isDockControl: null,
    isSystemUser:false,
    ////tabmenu按钮    
    //plugins: [{
    //    ptype: 'tabscrollermenu',
    //    maxText: 500,
    //    pageSize: 50
    //}],
    initComponent: function () {
        var me = this;       
        var existItemsId = this.existItemsId;
        var myItem = [];
        var menus = [];       
         if (me.usertype == 'SYSTEM') {
             me.hidden = true;
             me.isSystemUser = true;
        }       
        if (me.hasCtl == true) {
            
            //先遍历用户已经添加的管理对象，添加到tabpanel和menu里
            for (var i = 0; i < existItemsId.length; i++) {
                var j = 0;
                if (existItemsId[i] == 'hideTab') {
                    continue;
                }
                while (j < me.optionItems.length && existItemsId[i] != me.optionItems[j].itemId) {
                    j++;
                }             
                myItem.push({
                    xtype: 'panel',
                    title: me.optionItems[j].text,
                    draggable:false,
                    name: me.optionItems[j].text,
                    itemId: me.optionItems[j].itemId
                });

                menus.push({
                    text: me.optionItems[j].text,
                    checked: true,
                    //barItem: tmpBtn,
                    myTabPanel: me,
                    itemId: me.optionItems[j].itemId,
                    checkHandler: this.onItemClick,
                    draggable: false
                });
            };
            myItem.push({
                title: 'hideTab',
                text: 'hideTab',
                name: 'hideTab',
                hidden: true,
                itemId: 'hideTab',
                draggable:false
            });
            me.items = myItem;
            //再遍历系统所有管理对象，把用户没有添加的管理对象添加到menu里
            for (var i = 0; i < me.optionItems.length; i++) {
                var j = 0;
                while (j < existItemsId.length && me.optionItems[i].itemId != existItemsId[j]) {
                    j++;
                }              
                if (j == existItemsId.length) {
                    var tmpBtn = me.optionItems[i];
                    menus.push({
                        text: me.optionItems[i].text,
                        checked: false,
                        //barItem: tmpBtn,
                        myTabPanel: me,
                        itemId: me.optionItems[i].itemId,
                        checkHandler: this.onItemClick,
                        draggable: false
                    });
                }
            }            
            //往menu里添加保存按钮
            menus.push({
                text: '保存',
                //xtype: 'button',
                //tooltip: '保存',
                //width: 80,
                draggable:false,
                iconCls: 'icon-save',
                myTabPanel: me,
                myMenus: menus,
                hideOnClick: true,
                handler: function () {
                    var bar = this.myTabPanel;
                    this.myMenus.shift();
                    var saveData = [];
                    for (var i = 0; i < bar.items.items.length; i++) {
                        saveData[i] = bar.items.items[i].itemId;
                    }
                    data = Ext.encode(saveData);
                    Ext.Ajax.request({
                        url: C_ROOT + 'SUP/MenuConfig/Save',
                        params: {
                            data: data,
                            isDockControl: me.isDockControl
                            //userid: userid
                        },
                        success: function (response) {
                            var text = response.responseText;
                            //alert(text);
                            tabMenu.fatherTab.existItemsId = Ext.decode(data);
                            //tabMenu.hide();
                            if (window.external.IsInWebBrowser != undefined) {
                                window.external.RefreshLeftFrame();
                            } else {
                                tabMenu.show();
                                tabMenu.hide();
                            }
                            //me.close();
                        }
                    });
                }
            });

            //添加管理对象的菜单
            tabMenu = Ext.create('Ext.menu.Menu', {
                fatherTab: me,
                items: menus,
                draggable: false,
                listeners: {
                    //禁止管理对象a标签拖动
                    'afterrender': function (tabPanel, eOpts) {
                        var div = document.getElementsByTagName('a');
                        for (var i = 0; i < div.length; i++) {
                            document.getElementsByTagName('a')[i].draggable = false;
                        }
                    },
                    'hide': function (menu, eOpts) {
                        //menu.fatherTab.removeAll();
                        //bar.items.items[i].itemId
                        for (var i = menu.fatherTab.items.items.length - 1; i > -1; i--) {
                            if (menu.fatherTab.items.items[i].itemId != 'hideTab') {
                                menu.fatherTab.remove(menu.fatherTab.items.items[i]);
                            }
                        }
                        var bar = menu.fatherTab;
                        for (var i = 0; i < menu.fatherTab.existItemsId.length; i++) {
                            if (menu.fatherTab.existItemsId[i] == 'hideTab') {
                                continue;
                            }
                            var k = 0;
                            while (menu.fatherTab.existItemsId[i] != bar.optionItems[k].itemId && k < bar.optionItems.length) {
                                k++;
                            }
                            var btn = bar.optionItems[k];
                            bar.add(btn);
                        }
                        for (var i = 0; i < menu.items.items.length - 1; i++) {
                            var k = 0;
                            while (menu.items.items[i].initialConfig.itemId != menu.fatherTab.existItemsId[k] && k < menu.fatherTab.existItemsId.length) {
                                k++;
                            }
                            if (k == menu.fatherTab.existItemsId.length) {
                                menu.items.items[i].setChecked(false, true);
                            } else {
                                menu.items.items[i].setChecked(true, true);
                            }
                        }
                    }
                }
            });
            me.tabMenu = tabMenu;
            ////添加管理对象按钮及其点击事件
            //var tabBar = {
            //    //scrollerCls: '',
            //    height: 40,
            //    //autoScroll: true,
            //    items: [
            //    { xtype: 'tbfill' },
            //        {
            //            xtype: 'button',
            //            iconCls: 'add',
            //            listeners: {
            //                'click': function (t, e, eOpts) {
            //                    //tabMenu.showAt(149, 32, 2);
            //                    e.stopEvent();
            //                    tabMenu.showAt(e.getXY());
            //                    return false;
            //                }
            //            }
            //        }
            //    ]
            //};
            //me.tabBar = tabBar;
        } //自定义按钮      

        this.callParent();
        

        //第二行tab的右键菜单监听
        //if (me.hasCtl == true) {
        //    this.on(
        //    'afterrender', function (tabpanel, eOpts) {
        //        this.tabBar.getEl().on(
        //        'contextmenu', function (e, t, eOpts) {
        //            e.stopEvent();
        //            tabMenu.showAt(e.getXY());
        //            return false;
        //        }
        //   )
        //    }
        //  )
        //}

        me.tabBar.on('change', function (tabBar, tab, card, eOpts) {
            tabBar.needsScroll = false;//禁止tabBar乱滚动，抽风
        });

        if (me.tabScrollArrow) {
        me.on('afterrender', function (tabPanel, eOpts) {
            var parent = Ext.query("*[class=x-tab-bar-body x-tab-bar-body-default x-tab-bar-body-horizontal x-tab-bar-body-default-horizontal x-tab-bar-body-top x-tab-bar-body-default-top x-tab-bar-body-horizontal-noborder x-tab-bar-body-default-horizontal-noborder x-tab-bar-body-docked-top x-tab-bar-body-default-docked-top x-box-layout-ct x-tab-bar-body-default x-tab-bar-body-default-horizontal x-tab-bar-body-default-top x-tab-bar-body-default-horizontal-noborder x-tab-bar-body-default-docked-top]");
                   
            if (parent != null & parent != '') {
                for (var i = 0; i < parent.length; i++) {
                    var leftChild = Ext.query("*[class=x-box-inner x-box-scroller-left]");
                    var rightChild = Ext.query("*[class=x-box-inner x-box-scroller-right]");
                    leftChild[i].style.height = '34px';
                    rightChild[i].style.height = '34px';
                    parent[i].removeChild(leftChild[i]);
                    parent[i].removeChild(rightChild[i]);
                    parent[i].appendChild(leftChild[i]);
                    parent[i].appendChild(rightChild[i]);
                    
                }
            }
           

            //var temp = tabPanel.tabBar.getLayout().overflowHandler;
            //if (temp != '' && temp != null) {
            //    temp.scrollBy(-200, false);
            //}
            //if (tabPanel.items.items[0] != null & tabPanel.items.items[0] != '') {
            //    tabPanel.setActiveTab(tabPanel.items.items[0]);
            //    tabPanel.brother.setActiveTab(tabPanel.brother.items.items[0]);
            //}
            
        })
    }
    },
    onItemClick: function (menuItem, checkflg) {
        if (checkflg) {
            var bar = menuItem.myTabPanel;
            var i = 0;
            while (menuItem.itemId != bar.optionItems[i].itemId) {
                i++;
            }
            var btn = bar.optionItems[i];
            bar.add(btn);           
        }
        else {
            var bar = menuItem.myTabPanel;
            //var bar = this;
            var i = 0;
            while (menuItem.itemId != bar.items.items[i].itemId) {
                i++;
            }
            var btn = bar.items.items[i];
            bar.remove(btn);
        }
    },
    listeners: {
        'afterrender': function (tabPanel, eOpts) {
            var panelBgc = Ext.query("*[class=x-panel-body x-panel-body-default x-column-layout-ct x-panel-body-default]");
            panelBgc[0].style.backgroundColor = '#F0F0F0';
        },
        'tabchange': function (tabPanel, newCard, oldCard, eOpts) {
            //没有load过得项目在if中load，load过的直接加载出来
            if (!newCard.loaded) {
                if (newCard.itemId == 'hideTab') {
                    return;
                }
                if (tabPanel.brother != null && tabPanel.brother != '') {
                    tabPanel.brother.setActiveTab('hideTab');
                }                            
                var designContainer = tabPanel.containerFather;
                //在切换到系统功能树tab页是才初始化系统功能树，懒加载
                //其他tab的items情况类似
                if (newCard.itemId == 'sysFuncTree') {
                    var sysFuncTree = Ext.create('Ext.ng.sysFuncTree', {
                        region: 'west',
                        itemId: 'sysFuncTree',
                        hasRightClickMenu: true,
                        hasCheckBox: false,
                        hasDbClickListener: true,
                        hasRightControl: true,
                        isSystemUser: tabPanel.isSystemUser
                        //StoreloadMarsk: tabPanel.StoreloadMarsk
                    });//新建系统功能树
                    designContainer.add(sysFuncTree);
                } else if (newCard.itemId == 'enFuncTree') {
                    var enFuncTree = Ext.create('Ext.ng.enFuncTree', {
                        region: 'west',
                        itemId: 'enFuncTree',
                        hasRightClickMenu: true,
                        isSystemUser: tabPanel.isSystemUser
                    });//新建企业功能树
                    designContainer.add(enFuncTree);
                } else if (newCard.itemId == 'myFuncTree') {
                    var myFuncTree = Ext.create('Ext.ng.myFuncTree', {
                        region: 'west',
                        itemId: 'myFuncTree',
                        hasRightClickMenu: true,
                        hasToolbar: true,
                        hasIndividualButton: true,
                        hasDbClickListener: true,
                        loaded: true,
                        isSystemUser: tabPanel.isSystemUser
                    });//新建我的功能树
                    designContainer.add(myFuncTree);
                } else if (newCard.itemId == 'enFuncTreeFake') {
                    var enFuncTreeFake = Ext.create('Ext.panel.Panel', {
                        region: 'west',
                        itemId: 'enFuncTreeFake',
                        title:'未启用企业功能'
                    });//未启用企业功能树
                    designContainer.add(enFuncTreeFake);
                } else if (newCard.itemId == 'tabPageOrg') {
                    var tabPageOrg = Ext.create('Ext.ng.OrgGuideTree', {
                        region: 'west',
                        itemId: 'tabPageOrg'
                    });//新建组织功能树
                    designContainer.add(tabPageOrg);
                } else if (newCard.itemId == 'tabPageCollege') {
                    var tabPageCollege = Ext.create('Ext.ng.ComGuideTree', {
                        region: 'west',
                        itemId: 'tabPageCollege'
                    })
                    designContainer.add(tabPageCollege);
                } else if (newCard.itemId == 'tabPageSupplyer') {                   
                    var tabPageSupplyer = Ext.create('Ext.ng.supplyTree', {
                            region: 'west',
                            itemId: 'tabPageSupplyer'
                        })
                    designContainer.add(tabPageSupplyer);
                } else if (newCard.itemId == 'TabPageContractManage') {                    
                    if (tabPanel.productId == 'i6s' || tabPanel.productId == 'i6') {
                    var TabPageContractManage = Ext.create('Ext.ng.cntFunTreei6s', {
                        region: 'west',
                        itemId: 'TabPageContractManage'
                    })
                } else {
                    var TabPageContractManage = Ext.create('Ext.ng.cntFunTree', {
                        region: 'west',
                        itemId: 'TabPageContractManage'
                    })
                    }
                    designContainer.add(TabPageContractManage);
                } else if (newCard.itemId == 'tabPageCustomer') {
                    var tabPageCustomer = Ext.create('Ext.ng.customTree', {
                        region: 'west',
                        itemId: 'tabPageCustomer'
                    })
                    designContainer.add(tabPageCustomer);
                } else if (newCard.itemId == 'tabPageWmDocTree') {
                    var tabPageWmDocTree = Ext.create('Ext.ng.WMDocGuideTree', {
                        region: 'west',
                        itemId: 'tabPageWmDocTree'
                    });//新建组织功能树
                    designContainer.add(tabPageWmDocTree);
                } else if (newCard.itemId == 'tabPageProject') {
                    var tabPageProject = Ext.create('Ext.projectMenuTree', {
                        region: 'west',
                        itemId: 'tabPageProject'
                });//新建项目树
                    designContainer.add(tabPageProject);
                } else if (newCard.itemId == 'tabPageNavigation') {
                    var tabPageNavigation = Ext.create('Ext.ng.tabPageNavigation', {
                        region: 'west',
                        itemId: 'tabPageNavigation'
                    });//新建功能导航
                    designContainer.add(tabPageNavigation);
                } else if (newCard.itemId == 'tabPageReportWareHouseTree') {
                    var tabPageReportWareHouseTree = Ext.create('Ext.ng.RWReportGuideTree', {
                        region: 'west',
                        itemId: 'tabPageReportWareHouseTree'
                    });//新建功能导航
                    designContainer.add(tabPageReportWareHouseTree);
                }

                designContainer.getLayout().setActiveItem(designContainer.queryById(newCard.itemId));
                newCard.loaded = true;
            } else {
                if (newCard.itemId == 'hideTab') {
                    return;
                }
                tabPanel.brother.setActiveTab('hideTab');
                var designContainer = tabPanel.containerFather;
                designContainer.getLayout().setActiveItem(designContainer.queryById(newCard.itemId));
            }   
        }
    }
});

//收缩左侧winform界面后，隐藏主界面的功能树panel
Ext.define('Ext.ng.leftHiddenPanel', {
    extend: 'Ext.tab.Panel',
    alias: 'widget.ngleftHiddenPanel',
    tabPosition: 'left',
    isSystemUser: false,
    tabBar: {
        width: 32,
        //height: 32
    },
    initComponent: function () {
        var me = this;
        
        var myItem = [];
        myItem.push({
            layout: 'fit',
        xtype: 'panel',
        title: 'hideTab',
            hidden : true,
            itemId: 'hideTab'
        });
        LoadEnFuncTreeRight = me.LoadEnFuncTreeRight;
        existItemsId = me.existItemsId;     

        var myFuncTree = Ext.create('Ext.ng.myFuncTree', {
            region: 'west',
            title: TranTitle('我的功能'),
            id: 'leftPanelMyFuncTree',
            itemId: 'myFuncTree',
            hasRightClickMenu: true,
            hasToolbar: true,
            hasIndividualButton: true,
            hasDbClickListener: true,
            loaded: false,
            isSystemUser: me.isSystemUser
        });//新建我的功能树

        myItem.push(myFuncTree);
        if (LoadEnFuncTreeRight.substr(0, 1) == 1) {
            if (LoadEnFuncTreeRight.substr(1, 1) == 1) {
                myItem.push(
                    {
                        layout: 'fit',
                    title: TranTitle('企业功能'),
                    itemId: 'enFuncTree'
                    }
                    //Ext.create('Ext.ng.enFuncTree', {
                    //    region: 'west',
                    //    title: TranTitle('企业功能树'),
                    //    itemId: 'enFuncTree'
                    //})
                )
            }
            if (LoadEnFuncTreeRight.substr(2, 1) == 1) {
                myItem.push(
                    {
                        layout: 'fit',
                    title: TranTitle('系统功能'),           
                    itemId: 'sysFuncTree'
                    }
                    //Ext.create('Ext.ng.sysFuncTree', {
                    //    region: 'west',
                    //    title: TranTitle('系统功能树'),
                    //    itemId: 'sysFuncTree'
                    //})
                    //新建系统功能树
                )
            }
        }
        else {
            if (LoadEnFuncTreeRight.substr(1, 1) == 1) {
                myItem.push({
                    layout: 'fit',
                    title: TranTitle('企业功能'), 
                    itemId: 'enFuncTreeFake'
                })
            };
            myItem.push(
                {
                    layout: 'fit',
                title: TranTitle('系统功能'),
                itemId: 'sysFuncTree'
                }
                //Ext.create('Ext.ng.sysFuncTree', {
                //    region: 'west',
                //    title: TranTitle('系统功能树'),
                //    itemId: 'sysFuncTree'
                //})
            )
        }
        for (var i = 0; i < existItemsId.length; i++) {
            var j = 0;
            if (existItemsId[i] == 'hideTab') {
                continue;
            }
            while (j < me.optionItems.length && existItemsId[i] != me.optionItems[j].itemId) {
                j++;
            }

            //me.add(me.optionItems[j]);
            myItem.push({
                layout: 'fit',
                xtype: 'panel',
                title: TranTitle(me.optionItems[j].text),
                //name: me.optionItems[j].text,
                itemId: me.optionItems[j].itemId
            });
        }
        //myItem.push({
        //    //layout: 'fit',
        //    //xtype: 'panel',
        //    //title: '',
        //    //hidden: true,
        //    itemId: 'hideTab'
        //});
        me.items = myItem;
        me.callParent();

        me.tabBar.on('change', function (tabBar, tab, card, eOpts) {
            tabBar.needsScroll = false;//禁止tabBar乱滚动，抽风
        });
    },
    listeners: {
        'tabchange': function (tabPanel, newCard, oldCard, eOpts) {
            //if (newCard.id.indexOf('singleOption') == 0 && !newCard.haveLoad) {
            //    newCard.haveLoad = true;
            //    var singleOption = Ext.create('EmpinfoList.view.Grid');
            //    singleOption.loadGridData();
            //    newCard.add(singleOption);
            //}
            //if (newCard.itemId == 'hideTab') {
            //    return;
            //}
            if (!newCard.loaded) {
                if (newCard.itemId == 'sysFuncTree') {
                    var sysFuncTree = Ext.create('Ext.ng.sysFuncTree', {
                        region: 'west',
                        itemId: 'sysFuncTree',
                        hasRightClickMenu: true,
                        hasCheckBox: false,
                        hasDbClickListener: true,
                        isSystemUser: tabPanel.isSystemUser
                    })//新建系统功能树
                    newCard.add(sysFuncTree);
                } else if (newCard.itemId == 'enFuncTree') {
                    var enFuncTree = Ext.create('Ext.ng.enFuncTree', {
                        region: 'west',
                        itemId: 'enFuncTree',
                        isSystemUser: tabPanel.isSystemUser
                    });//新建企业功能树
                    newCard.add(enFuncTree);
                } else if (newCard.itemId == 'enFuncTreeFake') {
                    var enFuncTreeFake = Ext.create('Ext.panel.Panel', {
                        region: 'west',
                        itemId: 'enFuncTreeFake',
                        title: '未启用企业功能'
                    });//未启用企业功能树
                    newCard.add(enFuncTreeFake);
                } else if (newCard.itemId == 'tabPageOrg') {
                    var tabPageOrg = Ext.create('Ext.ng.OrgGuideTree', {
                        region: 'west',
                        itemId: 'tabPageOrg'
                    });//新建组织功能树
                    newCard.add(tabPageOrg);
                } else if (newCard.itemId == 'myFuncTree') {
                    var myFuncTree = Ext.create('Ext.ng.myFuncTree', {
                        region: 'west',
                        itemId: 'myFuncTree',
                        hasRightClickMenu: true,
                        hasToolbar: true,
                        hasIndividualButton: true,
                        hasDbClickListener: true,
                        loaded: true
                    })
                    newCard.add(myFuncTree);
                } else if (newCard.itemId == 'tabPageCollege') {
                    var tabPageCollege = Ext.create('Ext.ng.ComGuideTree', {
                        region: 'west',
                        itemId: 'tabPageCollege'
                    })
                    newCard.add(tabPageCollege);
                } else if (newCard.itemId == 'tabPageSupplyer') {
                    var tabPageSupplyer = Ext.create('Ext.ng.supplyTree', {
                        region: 'west',
                        itemId: 'tabPageSupplyer'
                    })
                    newCard.add(tabPageSupplyer);
                } else if (newCard.itemId == 'TabPageContractManage') {
                    if (tabPanel.productId == 'i6s' || tabPanel.productId == 'i6') {
                        var TabPageContractManage = Ext.create('Ext.ng.cntFunTreei6s', {
                            region: 'west',
                            itemId: 'TabPageContractManage'
                        })
                    } else {
                        var TabPageContractManage = Ext.create('Ext.ng.cntFunTree', {
                            region: 'west',
                            itemId: 'TabPageContractManage'
                        })
                    }
                    newCard.add(TabPageContractManage);
                } else if (newCard.itemId == 'tabPageCustomer') {
                    var tabPageCustomer = Ext.create('Ext.ng.customTree', {
                        region: 'west',
                        itemId: 'tabPageCustomer'
                    })
                    newCard.add(tabPageCustomer);
                } else if (newCard.itemId == 'tabPageWmDocTree') {
                    var tabPageWmDocTree = Ext.create('Ext.ng.WMDocGuideTree', {
                        region: 'west',
                        itemId: 'tabPageWmDocTree'
                    });//新建组织功能树
                    newCard.add(tabPageWmDocTree);
                } else if (newCard.itemId == 'tabPageProject') {
                    var tabPageProject = Ext.create('Ext.projectMenuTree', {
                        region: 'west',
                        itemId: 'tabPageProject'
                    });//新建组织功能树
                    newCard.add(tabPageProject);
                } else if (newCard.itemId == 'tabPageNavigation') {
                    var tabPageNavigation = Ext.create('Ext.ng.tabPageNavigation', {
                        region: 'west',
                        itemId: 'tabPageNavigation'
                    });//新建功能导航
                    newCard.add(tabPageNavigation);
                } else if (newCard.itemId == 'tabPageReportWareHouseTree') {
                    var tabPageReportWareHouseTree = Ext.create('Ext.ng.RWReportGuideTree', {
                        region: 'west',
                        itemId: 'tabPageReportWareHouseTree'
                    });//新建功能导航
                    newCard.add(tabPageReportWareHouseTree);
                }

                newCard.loaded = true;
            }
            if (newCard.itemId != 'hideTab' && window.external.IsInWebBrowser != undefined) {
                window.external.PopDockingControl();
            }
            //window.external.DockingControl();
        },
        'afterrender': function (tabPanel, eOpts) {
            var ob = {
                height: 0,
                invalid: false,
                width: 33,
                x: 0,
                y: 0
            };
            tabPanel.tabBar.items.items[0].lastBox = ob;
        }
    }
});


//组织
Ext.define('Ext.ng.OrgGuideTree', {
    extend: 'Ext.tree.TreePanel',
    alias: 'widget.ngOrgGuideTree',
    animate: true,
    collapsible: false,
    useArrows: true,
    rootVisible: false,
    align: 'center',
    autoScroll: true,
    hideHeaders: true,
    selectMode: 'Single',
    nodeIndex: -1,
    region: 'center', //默认center
    columns: [
         {
            text: '组织名称',
            flex: 1,
            xtype: 'treecolumn',
            dataIndex: 'Text',
            hidden: false,
            hideable: false,
            align: 'left',
            renderer: function (value, metaData, record) {
                var text = value;
                if (record.data.OCode == $appinfo.ocode) {
                    text = text + '<img height="13px" width="13px" src="' + C_ROOT + 'NG3Resource/icons/Location.gif"/>';
                }
                return text;
            }
        }
    ],
    initComponent: function () {
        var me = this;
        Ext.define('itemmodel', {
            extend: 'Ext.data.Model',
            fields: [
                {
                    name: 'PhId',
                    type: 'string',
                    mapping: 'PhId'
                }, {
                    name: 'RelatId',
                    type: 'string',
                    mapping: 'RelatId'
                }, {
                    name: 'OCode',
                    type: 'string',
                    mapping: 'OCode'
                }, {
                    name: 'ParentOrg',
                    type: 'string',
                    mapping: 'ParentOrg'
                }, {
                    name: 'RelaIndex',
                    type: 'string',
                    mapping: 'RelaIndex'
                }, {
                    name: 'RelId',
                    type: 'string',
                    mapping: 'RelId'
                }, {
                    name: 'OrderType',
                    type: 'string',
                    mapping: 'OrderType'
                }, {
                    name: 'OrgId',
                    type: 'string',
                    mapping: 'OrgId'
                }, {
                    name: 'ParentOrgId',
                    type: 'string',
                    mapping: 'ParentOrgId'
                }, {
                    name: 'NgRecordVer',
                    type: 'int',
                    mapping: 'NgRecordVer'
                }, {
                    name: 'OName',
                    type: 'string',
                    mapping: 'OName'
                }, {
                    name: 'Text',
                    type: 'string',
                    mapping: 'text'
                }
            ]
        });
        var itemStore = Ext.create('Ext.data.TreeStore', {
            model: 'itemmodel',
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: C_ROOT + 'DMC/Org/OrgRelat/GetUserLoginOrg',
                extraParams: {
                    parentphid: '',
                    searchkey: ''
                }
            },
            listeners: {
                'beforeload': function (store, opration, eOpts) {
                    //store.proxy.extraParams.parentphid = opration.id;
                }
            }
        });
        me.store = itemStore;
        me.dockedItems = [
            {
                xtype: 'toolbar',
                //width: 300,
                //height: 26,
				height: 30,
                dock: 'top',
                layout: 'border',
                minWidth: 200,
                items: [
                    {
                        region: 'center',
                        xtype: 'textfield',
                        itemId: 'query',
                        name: 'queryname',
                        emptyText: '搜索内容',
                        id: 'DMC_ORG_query'
                        //minWidth: 80
                    }, {
                        region: 'east',
                        width: 40,
                        text: '搜索',
                        handler: function () {
                            var value = Ext.getCmp('DMC_ORG_query').getValue();
                            if (value == '') {
                                return;
                            }
                            var index = 0;
                            var firstFind = me.nodeIndex == -1;
                            var findNode = me.getRootNode().findChildBy(function (node) {
                                index++;
                                if (!node.data.root && index > me.nodeIndex && (node.data.Text.indexOf(value) > -1)) {
                                    return true;
                                }
                            }, null, true);
                            me.nodeIndex = index;
                            if (findNode) {
                                me.selectPath(findNode.getPath());
                            } else {
                                me.nodeIndex = -1;
                                if (firstFind) {
                                    //Ext.MessageBox.alert('', '没有匹配的树节点.');
                                } else {
                                    arguments.callee.apply(this, arguments);
                                }
                            }
                        }
                    }, {
                        region: 'east',
                        width: 40,
                        text: '刷新',
                        anchor: '100%',
                        handler: function () {
                            me.refreshData();
                            Ext.getCmp('DMC_ORG_query').setValue("");
                        }
                    }
                ]
            }
        ];
        me.loadData = function () {
            itemStore.load();
        };
        me.refreshData = function () {
            itemStore.load();
        }
        //me.StoreloadMarsk = new Ext.LoadMask(document.body, {
        //        msg: '加载中',
        //            disabled: false
        //        });
        me.callParent();
        var view = me.getView();
        view.toggleOnDblClick = false;
        me.addCls(me.autoWidthCls);
    },
    listeners: {
        'itemdblclick': function (item, record, it, index, e, eOpts) {
            if ($appinfo.ocode != record.data.OCode) {
                if (window.external.IsInWebBrowser == undefined) {
                    var myMask = new Ext.LoadMask(document.body, { msg: "切换中..." });
                    myMask.show();
                    Ext.Ajax.request({
                        params: { 'ocode': record.get("OCode"), 'orgid': record.get("OrgId"), "ocodeName": record.get("OName") },
                        url: C_ROOT + 'SUP/Login/ChangeOrg',
                        success: function (response) {
                            myMask.hide();
                            if (response.responseText) {
                                window.top.location.reload();//刷新整个web框架
                                //window.location.reload();
                            }
                        }
                    });
                } else {
                    var istra = window.external.SetOcode(record.data.OCode, record.data.OName); if (istra) {
                        $appinfo.ocode = record.data.OCode;
                        $appinfo.orgID = record.data.OrgId;
                        this.getView().refresh(true);
                        refreshTree();
                    }
                }
            }
        }
    }
});

///同事树
Ext.define('Ext.ng.ComGuideTree', {
    extend: 'Ext.tree.TreePanel',
    alias: 'widget.ngComGuideTree',
    animate: true,
    collapsible: false,
    useArrows: true,
    rootVisible: false,
    align: 'center',
    autoScroll: true,
    hideHeaders: true,
    selectMode: 'Single',
    nodeIndex: -1,
    flag: 'my',
    region: 'center', //默认center
    columns: [
        {
            text: '物理主键',
            flex: 0,
            sortable: false,
            dataIndex: 'PhId',
            hideable: false,
            hidden: true
        }, {
            text: '关系代码',
            flex: 0,
            dataIndex: 'RelatId',
            sortable: false,
            hideable: false,
            hidden: true
        }, {
            text: '组织名称',
            flex: 1,
            xtype: 'treecolumn',
            dataIndex: 'Text',
            hidden: false,
            hideable: false,
            align: 'left'
        }, {
            text: '组织代码',
            flex: 0,
            dataIndex: 'OCode',
            hidden: true,
            hideable: false,
            sortable: false,
            align: 'left'
        }, {
            text: '上级组织代码',
            flex: 0,
            dataIndex: 'ParentOrg',
            hidden: true,
            hideable: false
        }, {
            text: '关联号',
            flex: 0,
            dataIndex: 'RelaIndex',
            hidden: true,
            hideable: false
        }, {
            text: '订货单位',
            flex: 0,
            dataIndex: 'OrderType',
            hidden: true,
            hideable: false
        }, {
            text: '组织主键',
            flex: 0,
            dataIndex: 'OrgId',
            hidden: true,
            hideable: false
        }, {
            text: '上级组织主键',
            flex: 0,
            dataIndex: 'ParentOrgId',
            hidden: true,
            hideable: false
        }
    ],
    initComponent: function () {
        var me = this;
        Ext.define('itemmodel', {
            extend: 'Ext.data.Model',
            fields: [
                {
                    name: 'PhId',
                    type: 'string',
                    mapping: 'PhId'
                }, {
                    name: 'RelatId',
                    type: 'string',
                    mapping: 'RelatId'
                }, {
                    name: 'OCode',
                    type: 'string',
                    mapping: 'OCode'
                }, {
                    name: 'ParentOrg',
                    type: 'string',
                    mapping: 'ParentOrg'
                }, {
                    name: 'RelaIndex',
                    type: 'string',
                    mapping: 'RelaIndex'
                }, {
                    name: 'RelId',
                    type: 'string',
                    mapping: 'RelId'
                }, {
                    name: 'OrderType',
                    type: 'string',
                    mapping: 'OrderType'
                }, {
                    name: 'OrgId',
                    type: 'string',
                    mapping: 'OrgId'
                }, {
                    name: 'ParentOrgId',
                    type: 'string',
                    mapping: 'ParentOrgId'
                }, {
                    name: 'NgRecordVer',
                    type: 'int',
                    mapping: 'NgRecordVer'
                }, {
                    name: 'OName',
                    type: 'string',
                    mapping: 'OName'
                }, {
                    name: 'Text',
                    type: 'string',
                    mapping: 'text'
                }
            ]
        });
        var itemStore = Ext.create('Ext.data.TreeStore', {
            model: 'itemmodel',
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: C_ROOT + 'HR/Emp/MyWorkMate/GetUserComTree?type=my',
                extraParams: {
                    parentphid: '',
                    searchkey: ''
                }
            },
            listeners: {
                'beforeload': function (store, opration, eOpts) {
                    store.proxy.extraParams.parentphid = opration.id;
                }
            }
        });
        me.store = itemStore;
        me.dockedItems = [
                        {
                            xtype: 'toolbar',
                            //width: 300,
                            //height: 26,
							height: 30,
                            dock: 'top',
                            layout: 'border',
                            minWidth: 200,
                            items: [
                                {
                                    region: 'center',
                                    xtype: 'textfield',
                                    itemId: 'query',
                                    name: 'queryname',
                                    emptyText: '搜索内容',
                                    listeners: {
                                    }
                                }, {
                                    region: 'east',
                                    //width: 10,
                                    text: '搜索',
                                    anchor: '100%',
                                    handler: function () {
                                        var condition = this.ownerCt.queryById('query').getValue();
                                        if (condition == null) {
                                            condition = "";
                                        };
                                        itemStore.proxy.url = C_ROOT + 'HR/Emp/MyWorkMate/GetUserComTree?type=' + me.flag + "&empno=" + condition;
                                        itemStore.load();
                                    }
                                }
                            ]
                        }
                    , {
                        xtype: 'toolbar',
                        width: 300,
                        height: 30,
                        dock: 'top',
                        //layout: 'border',
                        minWidth: 200,
                        enableOverFlow: true,
                        layout: {
                            overflowHandler: 'Menu'
                        },
                        items: [
                            {
                                xtype: 'button',
                                region: 'east',                                
                                text: '设置',
                                anchor: '100%',
                                handler: function () {
                                    $OpenTab("自定义", C_ROOT + 'HR/Emp/MyWorkMate/MyWorkMateList');
                                }
                            }, {
                                xtype: 'button',
                                region: 'east',                                
                                text: '我的',
                                handler: function () {
                                    itemStore.proxy.url = C_ROOT + 'HR/Emp/MyWorkMate/GetUserComTree?type=my'
                                    itemStore.load();
                                    me.flag = "my";
                                }
                            }, {
                                xtype: 'button',
                                region: 'east',                               
                                text: '刷新',
                                anchor: '100%',
                                handler: function () {
                                    itemStore.proxy.url = C_ROOT + 'HR/Emp/MyWorkMate/GetUserComTree?type=' + me.flag;
                                    itemStore.load();
                                }
                            },
                            {
                                xtype: 'button',
                                region: 'east',                                
                                text: '部门',
                                anchor: '100%',
                                handler: function () {
                                    itemStore.proxy.url = C_ROOT + 'HR/Emp/MyWorkMate/GetUserComTree?type=dept'
                                    itemStore.load();
                                    me.flag = "dept";
                                }
                            }, {
                                xtype: 'button',
                                region: 'east',                                
                                text: '全部',
                                handler: function () {
                                    itemStore.proxy.url = C_ROOT + 'HR/Emp/MyWorkMate/GetUserComTree?type=all'
                                    itemStore.load();
                                    me.flag = "all";
                                }
                            }, {
                                xtype: 'button',
                                region: 'east',                               
                                text: '在线',
                                anchor: '100%',
                                handler: function () {
                                    itemStore.proxy.url = C_ROOT + 'HR/Emp/MyWorkMate/GetUserComTree?type=online'
                                    itemStore.load();
                                    me.flag = "online";
                                }
                            }
                        ]
                    }
        ];
        me.loadData = function () {
            itemStore.load();
        };
        me.refreshData = function () {
            itemStore.load();
        }
        me.callParent();
    },
    listeners:
        {
            'itemcontextmenu': function (t, record, item, index, e, opt) {
                e.preventDefault();
                Ext.create('Ext.menu.Menu', {
                    width: 100,
                    height: 100,
                    margin: '0,0,10,0',
                    items: [
                        {
                            text: '发邮件',
                            listeners: {
                                click: function (a, b, c, d) {
                                    var phid = record.data.PhId;
                                    Ext.Ajax.request({
                                        params: {
                                            'id': phid
                                        },
                                        url: C_ROOT + 'HR/Emp/HrEpmMain/GetEmail',
                                        success: function (response) {
                                            var resp = Ext.JSON.decode(response.responseText);
                                            if (resp.Status === "success") {
                                                var email = resp.Msg;
                                                if (email != "") {
                                                    $OpenTab("发邮件", C_ROOT + 'WebMail/MailEdit.aspx?asMailTo=' + email + '&asAttachs=');
                                                }
                                                else {
                                                    Ext.MessageBox.alert('提示', "选中同事没有设置邮件地址!");
                                                }
                                            } else {
                                                Ext.MessageBox.alert('提示', resp.Msg);
                                            }
                                        }
                                    });
                                }
                            }
                        },
                        {
                            text: '发消息自由呼',
                            listeners: {
                                click: function (a, b, c, d) {
                                    var nameNo = record.data.Text;
                                    nameNo = nameNo.substr(0, nameNo.length - 1);
                                    var name = nameNo.split('(')[0];
                                    var no = nameNo.split('(')[1];
                                    $OpenTab("消息自由呼", C_ROOT + 'PCS/i6.Presentation.Msg/NFCTabItems.aspx?empno=' + no + '&empname=' + encodeURI(encodeURI(name)) + '&tabIndex=-1&company=true');
                                }
                            }
                        },
                        {
                            text: '同事自定义设置',
                            listeners:
                                {
                                    click: function (a, b, c, d) {
                                        $OpenTab("自定义", C_ROOT + 'HR/Emp/MyWorkMate/MyWorkMateList');
                                    }
                                }
                        }
                    ]
                }).showAt(e.getXY());
            }
        }
});

//供应商
Ext.define('Ext.ng.supplyTree', {
    extend: 'Ext.ng.TreePanel',
    autoLoad: false,
    alias: 'widget.ngSupplyTree',
    menu: null,//右键菜单，在初始化函数传入
    initComponent: function () {
        var me = this;
        this.callParent();
        me.menu = Ext.create('Ext.menu.Menu', {
            //selectedNode:null,
            items: [
                {
                    text: '供应商设置',
                    handler: function () {
                        Ext.Ajax.request({
                            async: false,
                            params: null,
                            url: C_ROOT + 'DMC/Enterprise/SupplyFile/IsHaveRight',
                            success: function (response) {
                                if (response.responseText == "True") {
                                    $OpenTab("自定义", C_ROOT + '/DMC/Enterprise/SupplyFile/SupplyFileList');
                                }
                                else {
                                    Ext.MessageBox.alert("提示", "您没有供应商信息权限，请联系管理员授权");
                                }
                            }
                        });
                    }
                }, '-',
                {
                    text: '全部展开',
                    handler: function () {
                        var root = me.menu.rootNode;
                        expandTree(root);
                    }
                },
                {
                    text: '全部折叠',
                    handler: function () {
                        var root = me.menu.rootNode;
                        collapseTree(root);
                    }
                }
            ]
        });
    },
    height: 300,
    width: 300,
    split: true,
    border: false,
    rootVisible: false,
    layout: 'fit',
    treeFields: [{ name: 'text', type: 'string' },
    { name: 'PhId', type: 'string' },
    { name: 'originalcode', type: 'string' },
        { name: 'name', type: 'string' },
        { name: 'originalid', type: 'string' },
        { name: 'pid', type: 'string' },
        { name: 'url', type: 'string' },
        { name: 'userid', type: 'int' }
    ],
    url: C_ROOT + 'DMC/Enterprise/SupplyFile/GetCustomSupplyData?type=all',
    dockedItems: [{
        xtype: 'toolbar',
        dock: 'top',
        layout: 'border',
        minWidth: 200,
        //height: 26,
		height: 30,
        items: [{
            itemId: 'add',
            region: 'west',
            width: 60,
            text: '自定义',
            handler: function () {
                var top = window.parent;
                //Ext.Ajax.request({
                //    async: false,
                //    params: null,
                //    url: C_ROOT + 'DMC/Enterprise/SupplyFile/IsHaveRight',
                //    success: function (response) {
                //        if (response.responseText == "True") {
                //            $OpenTab("自定义", C_ROOT + 'DMC/Enterprise/SupplyConfig/SupplyConfigList');
                //        }
                //        else {
                //            Ext.MessageBox.alert("提示", "您没有供应商信息权限，请联系管理员授权");
                //        }
                //    }
                //});
                $OpenTab("自定义供应商树设置", C_ROOT + 'DMC/Enterprise/SupplyConfig/SupplyConfigList');
            }
        }, {
            itemId: 'area',
            region: 'west',
            width: 40,
            text: '地区',
            handler: function () {
                var supplyTree = this.findParentByType('ngSupplyTree');
                supplyTree.store.proxy.url = C_ROOT + 'DMC/Enterprise/SupplyFile/GetCustomSupplyData?type=regionid&&condition=' + supplyTree.queryById('queryKey').getValue();
                supplyTree.getStore().load();
            }
        }, {
            itemId: 'type',
            region: 'west',
            width: 40, text: '类型',
            handler: function () {
                var supplyTree = this.findParentByType('ngSupplyTree');
                supplyTree.store.proxy.url = C_ROOT + 'DMC/Enterprise/SupplyFile/GetCustomSupplyData?type=suppclassid&&condition=' + supplyTree.queryById('queryKey').getValue();
                supplyTree.getStore().load();
            }
        }, '->',
        {
            itemId: 'refresh',
            region: 'west',
            width: 40,
            text: '刷新',
            handler: function () {
                var supplyTree = this.findParentByType('ngSupplyTree');
                supplyTree.store.proxy.url = C_ROOT + 'DMC/Enterprise/SupplyFile/GetCustomSupplyData?type=all';
                supplyTree.getStore().load();
            }

        }
        ]
    }, {
        xtype: 'toolbar',
        dock: 'top',
        layout: 'border',
        minWidth: 200,
        height: 26,
        items: [{
            region: 'center',
            minWidth: 180,
            grow: true,
            xtype: 'textfield',
            itemId: 'queryKey',
            name: 'queryKey',
            emptyText: ''
        }, {
            text: '搜索',
            itemId: 'query',
            region: 'east',
            width: 40,
            handler: function () {
                var condition = this.ownerCt.queryById('queryKey').getValue();
                if (condition == '' || condition == null) {
                return;
            }
                var supplyTree = this.findParentByType('ngSupplyTree');
                supplyTree.store.proxy.url = C_ROOT + 'DMC/Enterprise/SupplyFile/GetPortalTreeData?type=all&&condition=' + supplyTree.queryById('queryKey').getValue();
                supplyTree.getStore().load();
            }
        }]
    }],
    listeners: {
        'afterrender': function () {
            this.getRootNode().expand();
        },
        'itemclick': function (view, rcd, item, idx, event, eOpts) {
        },
        'itemdblclick': function (view, rcd, item, idx, event, eOpts) {
            //var param = { 'str': rcd.raw.text, 'rightname': '', 'managername': rcd.raw.managername, 'moduleno': rcd.raw.moduleno, 'id': rcd.raw.code, 'url': rcd.raw.url, 'suite': rcd.raw.suite };
            //window.external.OpenFunction(rcd.raw.url, JSON.stringify(param));
            $OpenTab("供应商信息", C_ROOT + '/DMC/Enterprise/SupplyFile/SupplyFileEdit?otype=view&id=' + rcd.raw.PhId);
        },
        'checkchange': function (node, checked) {
            setChildNodeChecked(node, checked); //同时选择下级
        },
        'itemcontextmenu': function (view, rec, node, index, e) {
            e.stopEvent();
            var root = this.getRootNode();
            this.menu.rootNode = root;
            this.menu.showAt(e.getXY());
            return false;
        },
        'containercontextmenu': function (treeP, e, eOpts) {
            e.stopEvent();
            var root = this.getRootNode();
            this.menu.rootNode = root;
            this.menu.showAt(e.getXY());
            return false;
        }
    }
})

//合同树
Ext.define('Ext.ng.cntFunTree', {
    extend: 'Ext.ng.TreePanel',
    autoLoad: false,
    alias: 'widget.ngCntFunTree',
    initComponent: function () {
        var me = this;
        me.treeFields = [
            { name: 'PhId', type: 'string', mapping: 'phid_cnt' },
            { name: 'cnt_type', type: 'string', mapping: 'cnt_type' },
            { name: 'phid_pc', type: 'string', mapping: 'phid_pc' },
            { name: 'phid_cnt', type: 'string', mapping: 'phid_cnt' },
            { name: 'ptypename', type: 'string', mapping: 'ptypename' },
            { name: 'pproname', type: 'string', mapping: 'pproname' },
            { name: 'pnode', type: 'string', mapping: 'pnode' },
            { name: 'typename', type: 'string', mapping: 'typename' },
            { name: 'proname', type: 'string', mapping: 'proname' },
            { name: 'node', type: 'string', mapping: 'node' },
            { name: 'text', type: 'string', mapping: 'text' },
        ];
        me.callParent();
        me.menu = Ext.create('Ext.menu.Menu', {
            items: [{
                text: '合同卡片',
                handler: function () {
                    var node = me.getSelectionModel().getSelection()[0];
                    $OpenTab("合同", "/PMS/PCM/CntM/CntMEdit?otype=view&model=1&id=" + node.data.PhId);
                }
            }, {
                text: '合同新增',
                handler: function () {
                    $OpenTab("合同", "/PMS/PCM/CntM/CntMEdit?otype=add&model=1&cnttype=-1");
                }
            }]
        });
    },
    root: {
        expanded: true,
        text: "合同",
        typename: 'root',
        proname: 'root',
        node: 'root'
    },
    height: 300,
    width: 300,
    split: true,
    border: false,
    rootVisible: true,
    layout: 'fit',
    url: C_ROOT + 'PMS/PCM/CntFunTree/LoadCntFunTree',
    loadData: function (showtype) {
        Ext.apply(this.getStore().proxy.extraParams, { 'showtype': showtype });
        this.store.load();
    },
    dockedItems: [{
        xtype: 'toolbar',
        dock: 'top',
        layout: 'border',
        minWidth: 200,
        //height: 25,
		height: 30,
        items: [{
            itemId: 'add',
            region: 'west',
            //width: 45,
            text: '自定义',
            handler: function () {
                $OpenTab("自定义", C_ROOT + 'PMS/PCM/CntFunTree/CntFunTreeList')
            }
        }, {
            itemId: 'type',
            region: 'east',
            //width: 45,
            text: '类型',
            handler: function () {
                var cntFunTree = this.findParentByType('ngCntFunTree');
                cntFunTree.loadData("type");
            }
        }, {
            itemId: 'proj',
            region: 'east',
            //width: 45,
            text: '项目',
            handler: function () {
                var cntFunTree = this.findParentByType('ngCntFunTree');
                cntFunTree.loadData("proj");
            }
        }, {
            itemId: 'refresh',
            region: 'east',
            //width: 45,
            text: '刷新',
            handler: function () {
                var cntFunTree = this.findParentByType('ngCntFunTree');
                cntFunTree.loadData("");
            }
        }]
    }, {
        xtype: 'toolbar',
        dock: 'top',
        layout: 'border',
        minWidth: 200,
        height: 25,
        items: [{
            region: 'west',
            minWidth: 100,
            grow: true,
            xtype: 'textfield',
            itemId: 'queryKey',
            name: 'queryKey',
            emptyText: '定位节点'
        }, {
            text: '定位',
            itemId: 'query',
            region: 'east',
            //width: 30,
            handler: function () {
                var toolbar = this.findParentByType('toolbar');//找到根节点
                var condition = toolbar.items.items[4].value;
                if (condition == '' || condition == null) {
                    //alert('请输入定位关键字');
                    return;
                }
                var cntFunTree = this.findParentByType('ngCntFunTree');
                findNodeByFuzzy(cntFunTree, condition);
            }
        }]
    }],
    listeners: {
        'itemcontextmenu': function (view, rec, node, index, e) {
            e.stopEvent();
            var root = this.getRootNode();
            this.menu.rootNode = root;
            this.menu.showAt(e.getXY());
            return false;
        }
    }
});

//客户树
Ext.define('Ext.ng.customTree', {
    extend: 'Ext.ng.TreePanel',
    autoLoad: false,
    alias: 'widget.ngCustomTree',
    menu: null, //右键菜单，在初始化函数传入
    initComponent: function() {
        var me = this;
        this.callParent();
        me.menu = Ext.create('Ext.menu.Menu', {
            //selectedNode:null,
            items: [
                {
                    text: '客户设置',
                    handler: function() {
                        Ext.Ajax.request({
                            async: false,
                            params: null,
                            url: C_ROOT + 'DMC/Enterprise/CustomFile/IsHaveRight',
                            success: function (response) {
                                if (response.responseText == "True") {
                                    $OpenTab("自定义", C_ROOT + '/DMC/Enterprise/CustomFile/CustomFileList');
                                }
                                else {
                                    Ext.MessageBox.alert("提示", "您没有客户信息权限，请联系管理员授权");
                                }
                            }
                        });
                    }
                }, '-',
                {
                    text: '全部展开',
                    handler: function() {
                        var root = me.menu.rootNode;
                        expandTree(root);
                    }
                },
                {
                    text: '全部折叠',
                    handler: function() {
                        var root = me.menu.rootNode;
                        collapseTree(root);
                    }
                }
            ]
        });
    },
    height: 300,
    width: 300,
    split: true,
    border: false,
    rootVisible: false,
    layout: 'fit',
    treeFields: [
        { name: 'text', type: 'string' },
        { name: 'PhId', type: 'string' },
        { name: 'originalcode', type: 'string' },
        { name: 'name', type: 'string' },
        { name: 'originalid', type: 'string' },
        { name: 'pid', type: 'string' },
        { name: 'url', type: 'string' },
        { name: 'userid', type: 'int' }
    ],
    url: C_ROOT + 'DMC/Enterprise/CustomFile/GetCustomSupplyData?type=all',
    dockedItems: [
        {
            xtype: 'toolbar',
            dock: 'top',
            layout: 'border',
            minWidth: 200,
            //height: 26,
			height: 30,
            items: [
                {
                    itemId: 'add',
                    region: 'west',
                    width: 60,
                    text: '自定义',
                    handler: function() {
                        var top = window.parent;
                        //Ext.Ajax.request({
                        //    async: false,
                        //    params: null,
                        //    url: C_ROOT + 'DMC/Enterprise/CustomFile/IsHaveRight',
                        //    success: function (response) {
                        //        if (response.responseText == "True") {
                        //            $OpenTab("自定义", C_ROOT + '/DMC/Enterprise/CustomConfig/CustomConfigList');
                        //        }
                        //        else {
                        //            Ext.MessageBox.alert("提示", "您没有客户信息权限，请联系管理员授权");
                        //        }

                        //    }
                        //});
                        $OpenTab("自定义客户树设置", C_ROOT + '/DMC/Enterprise/CustomConfig/CustomConfigList');
                    }
                }, {
                    itemId: 'area',
                    region: 'west',
                    width: 40,
                    text: '地区',
                    handler: function() {
                        var supplyTree = this.findParentByType('ngCustomTree');
                        supplyTree.store.proxy.url = C_ROOT + 'DMC/Enterprise/CustomFile/GetCustomSupplyData?type=regionid&&condition=' + supplyTree.queryById('queryKey').getValue();
                        supplyTree.getStore().load();
                    }
                }, {
                    itemId: 'type',
                    region: 'west',
                    width: 40,
                    text: '类型',
                    handler: function() {
                        var supplyTree = this.findParentByType('ngCustomTree');
                        supplyTree.store.proxy.url = C_ROOT + 'DMC/Enterprise/CustomFile/GetCustomSupplyData?type=suppclassid&&condition=' + supplyTree.queryById('queryKey').getValue();
                        supplyTree.getStore().load();
                    }
                }, '->',
                {
                    itemId: 'refresh',
                    region: 'west',
                    width: 40,
                    text: '刷新',
                    handler: function() {
                        var supplyTree = this.findParentByType('ngCustomTree');
                        supplyTree.store.proxy.url = C_ROOT + 'DMC/Enterprise/CustomFile/GetCustomSupplyData?type=all';
                        supplyTree.getStore().load();
                    }

                }
            ]
        }, {
            xtype: 'toolbar',
            dock: 'top',
            layout: 'border',
            minWidth: 200,
            height: 26,
            items: [
                {
                    region: 'center',
                    minWidth: 180,
                    grow: true,
                    xtype: 'textfield',
                    itemId: 'queryKey',
                    name: 'queryKey',
                    emptyText: ''
                },
                {
                    text: '搜索',
                    itemId: 'query',
                    region: 'east',
                    width: 40,
                    handler: function() {
                        var condition = this.ownerCt.queryById('queryKey').getValue();
                        if (condition == '' || condition == null) {
                            return;
                        }
                        var supplyTree = this.findParentByType('ngCustomTree');
                        supplyTree.store.proxy.url = C_ROOT + 'DMC/Enterprise/CustomFile/GetPortalTreeData?type=all&&condition=' + supplyTree.queryById('queryKey').getValue();
                        supplyTree.getStore().load();
                    }
                }]
            }
    ],
    listeners: {
        'afterrender': function() {
            this.getRootNode().expand();
        },
        'itemclick': function(view, rcd, item, idx, event, eOpts) {
        },
        'itemdblclick': function(view, rcd, item, idx, event, eOpts) {
            //var param = { 'str': rcd.raw.text, 'rightname': '', 'managername': rcd.raw.managername, 'moduleno': rcd.raw.moduleno, 'id': rcd.raw.code, 'url': rcd.raw.url, 'suite': rcd.raw.suite };
            //window.external.OpenFunction(rcd.raw.url, JSON.stringify(param));
            $OpenTab("客户信息", C_ROOT + '/DMC/Enterprise/CustomFile/CustomFileEdit?otype=view&id=' + rcd.raw.PhId);
        },
        'checkchange': function(node, checked) {
            setChildNodeChecked(node, checked); //同时选择下级
        },
        'itemcontextmenu': function(view, rec, node, index, e) {
            e.stopEvent();
            var root = this.getRootNode();
            this.menu.rootNode = root;
            this.menu.showAt(e.getXY());
            return false;
        },
        'containercontextmenu': function(treeP, e, eOpts) {
            e.stopEvent();
            var root = this.getRootNode();
            this.menu.rootNode = root;
            this.menu.showAt(e.getXY());
            return false;
        }
    }
});

//文档树
Ext.define('Ext.ng.WMDocTree', {
    extend: 'Ext.tree.TreePanel',
    alias: 'widget.ngDocTree',
    region: 'center',
    autoScroll: true,
    animate: true,
    collapsible: false,
    useArrows: true,
    rootVisible: false,
    align: 'center',
    hideHeaders: true,
    selectMode: 'Single',
    nodeIndex: -1,
    type: 'MY',
    filter: '',
    columns: [
        {
            text: '物理主键',
            flex: 0,
            sortable: false,
            dataIndex: 'PhId',
            hideable: false,
            hidden: true
        }, {
            text: '代码',
            flex: 0,
            dataIndex: 'CNo',
            sortable: false,
            hideable: false,
            hidden: true
        }, {
            text: '名称',
            flex: 1,
            xtype: 'treecolumn',
            dataIndex: 'Text',
            hidden: false,
            hideable: false,
            align: 'left'
        }, {
            text: '名称',
            flex: 0,
            dataIndex: 'CName',
            hidden: true,
            hideable: false,
            sortable: false,
            align: 'left'
        }
    ],
    initComponent: function () {
        var me = this;
        Ext.define('model', {
            extend: 'Ext.data.Model',
            fields: [
                {
                    name: 'PhId',
                    type: 'string',
                    mapping: 'PhId'
                }, {
                    name: 'CNo',
                    type: 'string',
                    mapping: 'CNo'
                }, {
                    name: 'CName',
                    type: 'string',
                    mapping: 'CName'
                }, {
                    name: 'Text',
                    type: 'string',
                    mapping: 'text'
                }, {
                    name: 'IsDoc',
                    type: 'string',
                    mapping: 'IsDoc'
                }, {
                    name: 'DocType',
                    type: 'string',
                    mapping: 'DocType'
                }, {
                    name: 'Doclibid',
                    type: 'string',
                    mapping: 'Doclibid'
                }, {
                    name: 'WbsId',
                    type: 'string',
                    mapping: 'WbsId'
                }
            ]
        });
        var store = Ext.create('Ext.data.TreeStore', {
            model: 'model',
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: C_ROOT + 'WM/Doc/Document/GetDocumentForGuid'
            }
        });
        store.on('beforeload', function (store) {
            if (me.type) {
                Ext.apply(store.proxy.extraParams, { 'type': me.type });
            }
            if (me.filter || me.filter == '') {
                Ext.apply(store.proxy.extraParams, { 'filter': me.filter });
            }
        });
        me.store = store;
        me.dockedItems = [
            {
                xtype: 'toolbar',
                //height: 26,
				height: 30,
                dock: 'top',
                layout: 'border',
                minWidth: 200,
                items: [
                    {
                        region: 'center',
                        xtype: 'textfield',
                        id: 'WMDocTree_query',
                        name: 'queryname',
                        emptyText: '搜索内容'
                    }, {
                        region: 'east',
                        width: 40,
                        text: '搜索',
                        handler: function () {
                            var searchfilter = me.down('#WMDocTree_query');
                            me.filter = searchfilter.value;
                            store.load();
                        }
                    }, {
                        region: 'east',
                        width: 40,
                        text: '刷新',
                        anchor: '100%',
                        handler: function () {
                            var searchfilter = me.down('#WMDocTree_query');
                            searchfilter.setValue('');
                            me.filter = '';
                            store.load();
                        }
                    }
                ]
            }
        ];
        me.callParent();
    },
    listeners: {
        'itemdblclick': function (item, record, it, index, e, eOpts) {
            if (record.data.DocType == "1") {
                var id = record.data.PhId;
                $OpenTab('企业文档-查看', C_ROOT + 'WM/Doc/Document/DocumentEdit?otype=view&id=' + id);
            } else if (record.data.DocType == "2") {
                $OpenTab('项目文档库', C_ROOT + 'WM/Doc/ProjectDocument/ProjectDocumentListFromGuid?phid=' + record.data.PhId + '&doclibid=' + record.data.Doclibid + '&wbsid=' + record.data.WbsId);
            }
        }
    }
});
Ext.define('Ext.ng.WMDocGuideTree', {
    extend: 'Ext.form.Panel',
    alias: 'widget.ngDocGuideTree',
    animate: true,
    collapsible: false,
    useArrows: true,
    rootVisible: false,
    align: 'center',
    autoScroll: false,
    hideHeaders: true,
    selectMode: 'Single',
    nodeIndex: -1,
    region: 'center', //默认center
    layout: 'border',
    initComponent: function () {
        var me = this;
        var treepanel = Ext.create('Ext.ng.WMDocTree');
        var toptoolbar = Ext.create('Ext.ng.Toolbar', {
            region: 'north',
            showArrowBtn: false,
            ngbuttons: [{
                text: '自定义',
                handler: function () {
                    var searchfilter = treepanel.down('#WMDocTree_query');
                    searchfilter.setValue('');
                    treepanel.filter = '';
                    treepanel.type = 'MY';
                    treepanel.store.load();
                }
            }, {
                text: '类型',
                handler: function () {
                    var searchfilter = treepanel.down('#WMDocTree_query');
                    searchfilter.setValue('');
                    treepanel.filter = '';
                    treepanel.type = 'LIB';
                    treepanel.store.load();
                }
            }, {
                text: '全部',
                handler: function () {
                    var searchfilter = treepanel.down('#WMDocTree_query');
                    searchfilter.setValue('');
                    treepanel.filter = '';
                    treepanel.type = 'ALL';
                    treepanel.store.load();
                }
            }, '->', {
                text: '自定义菜单',
                handler: function () {
                    $OpenTab('企业文档-查看', C_ROOT + 'WM/Doc/Document/DocumentUserConfig');
                }
            }]
        });
        me.items = [toptoolbar, treepanel];
        me.loadData = function () {
            treepanel.store.load();
        };
        me.refreshData = function () {
            treepanel.store.load();
        };
        me.callParent();
    }
});
//项目树
var projectMenuTreeFields = [{ name: 'id', type: 'System.String', mapping: 'id' },
                { name: 'pid', type: 'string', mapping: 'pid' },
                { name: 'PhId', type: 'string', mapping: 'PhId' },
                { name: 'PhIdType', type: 'string', mapping: 'PhIdType' },
                { name: 'ProjectId', type: 'string', mapping: 'ProjectId' },
                { name: 'ProjectParentId', type: 'string', mapping: 'ProjectParentId' },
                { name: 'text', type: 'string', mapping: 'text' },
                { name: 'IsCustomTree', type: 'string', mapping: 'IsCustomTree' },
                { name: 'SortNum', type: 'string', mapping: 'SortNum' },
                { name: 'Seq', type: 'string', mapping: 'Seq' },
                { name: 'Visible', type: 'string', mapping: 'Visible' },
                { name: 'ProjectName', type: 'string', mapping: 'ProjectName' }];

Ext.define('Ext.projectMenuTree', {
    extend: 'Ext.ng.TreePanel',
    alias: 'widget.projectMenuTree',
    region: 'west',
    width: 250,
    frame: false,
    border: false,
    split: true,
    scroll: 'both',
    rootVisible: true,
    treeFields: projectMenuTreeFields,
    url: C_ROOT + 'PMS/PC/ProjectTable/BuildProjectMenuTree?iscustom=1', //自定义取数
    menu: null,//右键菜单，在初始化函数传入
    initRightMenus: [],  //默认的右键菜单
    menuitems: [],   //右键菜单项
    buildRightMenu: function () {
        var me = this;
        me.menuitems = [];
        var templength = me.initRightMenus.length;
        me.menuitems.push(me.initRightMenus[templength - 1]);
        Ext.Ajax.request({
            url: C_ROOT + 'PMS/PC/ProjectTable/LoadCustomRightMenu',
            success: function (response) {
                //取自定义右键菜单数据
                var resp = Ext.JSON.decode(response.responseText);
                if (resp != '' && resp.children.length > 0) {
                    for (var i = resp.children.length - 1; i > -1; i--) {
                        if (resp.children[i].MenuId == 'separateline') {
                            me.menuitems.unshift('-');
                            continue;
                        }
                        for (var j = me.initRightMenus.length - 2; j > -1; j--) {
                            if (me.initRightMenus[j].itemId == resp.children[i].MenuId) {
                                me.menuitems.unshift(me.initRightMenus[j]);
                                break;
                            }
                        }
                    }
                }

                if (resp != '' && resp.children.length == 0) {//如果一条数据都没，默认建立所有menu
                    for (var j = me.initRightMenus.length - 2; j > -1; j--) {
                        me.menuitems.unshift(me.initRightMenus[j]);
                    }
                }

                me.menu = Ext.create('Ext.menu.Menu', {
                    //selectedNode:null,
                    parentTree: me,
                    items: me.menuitems
                });

            }
        });
    },
    isExpanded: false,   //确保节点完全展开、渲染完毕才能再次加载（否则可能导致快速多次加载节点错乱）
    isClickBar: false,   //点击按钮加载及时刷新树view
    initComponent: function () {
        var me = this;
        me.callParent();
        var root = me.getRootNode();
        root.data.text = '项目';
        root.expand();

        //防止树渲染错乱，搜索后及时刷新树
        me.store.on('load', function () {
            if(me.isClickBar) {
                me.view.refresh();
                me.isClickBar = false;
            }
            me.isExpanded = true;
        });
        me.store.on('beforeload', function () {
            if(me.isExpanded) {
                me.isExpanded = false;
                return true;
        }
            return false;
    });

        //构建右键菜单
        var product;
        Ext.Ajax.request({
            async: false, //同步请求
                url : C_ROOT + 'PMS/PC/ProjectTable/GetProduct',//获取产品名
                success: function (response) {
                product = response.responseText;
        }
    });
        //if (product == 'i8') {
        //    me.initRightMenus.push({
        //        itemId: 'projinfo',
        //        text: '项目信息',
        //        handler: function (a, b, c, d) {
        //            var pMenuTree = this.ownerCt.parentTree;
        //            var data = pMenuTree.getSelectionModel().getSelection();
        //            if (data.length > 0) {
        //                var node = data[0];
        //                var id = node.data.ProjectId;
        //                if (product == 'i8') {
        //                    $OpenTab('项目信息-查看', C_ROOT + 'PMS/PC/ProjectTable/ProjectTableEdit?otype=view&id=' + id);
        //                }
        //                else
        //                {
        //                    $OpenTab('项目信息-查看', C_ROOT + 'PMS/EntPC/PmsProjectInfo/ProjectInfoEdit?otype=view&id=' + id);
        //                }
        //            }
        //        }
        //    });
        //}
        //else {
        //    me.initRightMenus.push({
        //        itemId: 'projinfo',
        //        text: '项目信息',
        //        handler: function (a, b, c, d) {
        //            var pMenuTree = this.ownerCt.parentTree;
        //            var data = pMenuTree.getSelectionModel().getSelection();
        //            if (data.length > 0) {
        //                var node = data[0];
        //                var id = node.data.ProjectId;
        //                $OpenTab('项目信息-查看', C_ROOT + 'PMS/EntPC/PmsProjectInfo/ProjectInfoEdit?otype=view&id=' + id);
        //            }
        //        }
        //    });
        //}
        me.initRightMenus.push({
                itemId: 'projinfo',
            text: '项目信息',
                handler: function (a, b, c, d) {
                var pMenuTree = this.ownerCt.parentTree;
                var data = pMenuTree.getSelectionModel().getSelection();
                if (data.length > 0) {
                    var node = data[0];
                    var id = node.data.ProjectId;
                    if (id == "") return;
                    if (product == 'i8') {
                        $OpenTab('项目信息-查看', C_ROOT + 'PMS/PC/ProjectTable/ProjectTableEdit?otype=view&id=' +id);
                    }
                    else {
                        $OpenTab('项目信息-查看', C_ROOT + 'PMS/EntPC/PmsProjectInfo/ProjectInfoEdit?otype=view&id=' +id);
                }
            }
        }
    });

    if (product == 'i8') {
        //设置默认项目
            me.initRightMenus.push({
                    itemId: 'setdefault',
                text: '设置默认项目',
                    handler: function () {
                    var pMenuTree = this.ownerCt.parentTree;
                    var data = pMenuTree.getSelectionModel().getSelection();
                    if (data.length > 0) {
                        var node = data[0];
                        var id = node.data.ProjectId;
                        if (id == "") return;
                        Ext.Ajax.request({
                                url : C_ROOT + 'PMS/PC/ProjectTable/SetDefaultPC?pcid=' +id,
                            async: false, //同步请求
                                success: function (response) {
                                if (response.responseText == 'True') {
                                    pMenuTree.isClickBar = true;
                                    pMenuTree.refreshData();
                            }

                        }
                    });

                }
            }
    });

        //取消当前默认项目
            me.initRightMenus.push({
                    itemId: 'caceldefault',
                    text: '取消当前默认项目',
                    handler: function () {
                    var pMenuTree = this.ownerCt.parentTree;
                    var data = pMenuTree.getSelectionModel().getSelection();
                    if (data.length > 0) {
                        var node = data[0];
                        var id = node.data.ProjectId;
                        if (id == "") return;
                        Ext.Ajax.request({
                                url : C_ROOT + 'PMS/PC/ProjectTable/SetDefaultPC?pcid=0',
                            async: false, //同步请求
                                success: function (response) {
                                if (response.responseText == 'True') {
                                    pMenuTree.isClickBar = true;
                                    pMenuTree.refreshData();
                            }

                        }
                    });

                }
            }
    });
        }
        me.initRightMenus.push({
                itemId: 'projprocess',
            text: '项目进度',
                handler: function () {
                var pMenuTree = this.ownerCt.parentTree;
                var data = pMenuTree.getSelectionModel().getSelection();
                if (data.length > 0) {
                    var node = data[0];
                    var id = node.data.ProjectId;
                    if (id == "") return;
                    $OpenTab('进度计划', C_ROOT + 'Schedules/SpmNetPlan/ESchedule?pcid=' +id);
            }
        }
    });
    if(product=='i8')
            me.initRightMenus.push({
                    itemId: 'checkliverecord',
                    text: '现场检查记录',
                    handler: function () {
                    var pMenuTree = this.ownerCt.parentTree;
                    var data = pMenuTree.getSelectionModel().getSelection();
                    if (data.length > 0) {
                        var node = data[0];
                        var id = node.data.ProjectId;
                        if (id == "") return;
                        $OpenTab('现场检查记录WBS', C_ROOT + 'PMS/PMS/AqChkM/AqChkMList?pcid=' +id);
                }
            }
    });
if (product == 'i8')
            me.initRightMenus.push({
                    itemId: 'unpassadjust',
                    text: '不符合项整改单',
                    handler: function () {
                    var pMenuTree = this.ownerCt.parentTree;
                    var data = pMenuTree.getSelectionModel().getSelection();
                    if (data.length > 0) {
                        var node = data[0];
                        var id = node.data.ProjectId;
                        if (id == "") return;
                        $OpenTab('不符合项整改单', C_ROOT + 'PMS/PMS/AqCorrM/AqCorrMList?pcid=' +id);
                }
            }
    });
if (product == 'i8')
            me.initRightMenus.push({
                    itemId: 'emergence',
                text: '应急预案',
                    handler: function () {
                    var pMenuTree = this.ownerCt.parentTree;
                    var data = pMenuTree.getSelectionModel().getSelection();
                    if (data.length > 0) {
                        var node = data[0];
                        var id = node.data.ProjectId;
                        $OpenTab('应急预案', C_ROOT + 'SUP/pform0000600004List?pcid=' +id);
                }
            }
    });
if (product == 'i8')
            me.initRightMenus.push({
                itemId: 'wbs',
                text: '维护WBS',
                    handler: function () {
                    var pMenuTree = this.ownerCt.parentTree;
                    var data = pMenuTree.getSelectionModel().getSelection();
                    if (data.length > 0) {
                        var node = data[0];
                        var id = node.data.ProjectId;
                        $OpenTab('项目WBS', C_ROOT + 'PMS/BasicData/WBS/WbsList?pcid=' +id);
                }
            }
    });
if (product == 'i8')
            me.initRightMenus.push({
                itemId: 'cbs',
                text: '维护CBS',
                    handler: function () {
                    var pMenuTree = this.ownerCt.parentTree;
                    var data = pMenuTree.getSelectionModel().getSelection();
                    if (data.length > 0) {
                        var node = data[0];
                        var id = node.data.ProjectId;
                        $OpenTab('项目CBS', C_ROOT + 'PMS/BasicData/Cbs/PcCbsList?pcid=' +id);
                }
            }
    });
me.initRightMenus.push({
        itemId: 'customproject',
        text: '项目自定义菜单',
                handler: function () {
                var pMenuTree = this.ownerCt.parentTree;
                var bar = pMenuTree.queryById('projmenudef');
                    bar.handler();
}
    });
    me.initRightMenus.push({
            itemId: 'custommenu',
            text: '右键菜单自定义',
                handler: function () {
                $OpenTab('项目右键菜单自定义设置', C_ROOT + 'PMS/PC/ProjectTable/CustomRightMenuView');
    }
    });

        me.buildRightMenu();
        },
            tbar: [
                {
                    itemId: 'setting', text: '设置', iconCls: "icon-Setup"
                    ,
                        menu: {
                            items: [{
                            itemId: 'projmenudef', text: '项目菜单自定义'
                            , handler: function () {
                                $OpenTab('自定义项目菜单树', C_ROOT + 'PMS/PC/ProjectTable/CustomProjectView');
                        }
                    }]
                }
        },
                '->',
                {
                        itemId: 'getall', text: '全部', handler: function () {
                        var pMenuTree = this.findParentByType('projectMenuTree');
                            pMenuTree.isClickBar = true;
                            pMenuTree.refreshData();
                }
        },
                {
                    itemId: 'refresh', text: '刷新', iconCls: "icon-Refresh"
                    , handler: function () {
                        var pMenuTree = this.findParentByType('projectMenuTree');
                        pMenuTree.isClickBar = true;
                        pMenuTree.refreshData();
                }
}
]
,
    dockedItems: [
        {
        xtype: 'toolbar',
        dock: 'top',
        layout: 'border',
        minWidth: 200,
        height: 26,
            items: [{
            region: 'center',
            minWidth: 150,
            grow: true,
            xtype: 'textfield',
            itemId: 'searchKey',
            name: 'searchKey',
            emptyText: '搜索内容'
        }
        ,
            {
            text: '搜索',
            itemId: 'searchBar',
            region: 'east',
            width: 40,
                handler: function () {
                    var toolbar = this.findParentByType('toolbar');//找到根节点
                    var condition = toolbar.items.items[0].value;
                    if (condition == '' || condition == null) {
                        return;
                }
                var pMenuTree = this.findParentByType('projectMenuTree');

                Ext.apply(pMenuTree.store.proxy.extraParams, {
                projectname: condition
            });
            pMenuTree.getRootNode().removeAll();
                pMenuTree.isClickBar = true;
                pMenuTree.store.load();
                pMenuTree.store.proxy.extraParams = {};
}
        }]
    }],
            listeners: {
                'itemcontextmenu': function (view, rec, node, index, e) {
                    e.stopEvent();
                    var root = this.getRootNode();
                    this.menu.rootNode = root;
                    this.menu.showAt(e.getXY());
            return false;
        }
        ,
        'containercontextmenu': function (treeP, e, eOpts) {
            e.stopEvent();
            var root = this.getRootNode();
            this.menu.rootNode = root;
            this.menu.showAt(e.getXY());
            return false;
            },
        'afteritemexpand': function (node, index, item, eOpts) {
            this.isExpanded = true;
        },
        'load': function (self, node, records, successful, eOpts) {

            var curpcid
            Ext.Ajax.request({
                //params: { userid: $appinfo.userID },
                    url: C_ROOT + 'PMS/PC/ProjectTable/GetDefaultPC',
                async: false, //同步请求
                    success: function (response) {
                        if (response.responseText != "" && response.responseText != "0") {

                        curpcid = response.responseText;
                        var defaultnode = node.findChild('ProjectId', curpcid, true);
                        defaultnode.data.text = defaultnode.data.text + '<img height="13px" width="13px" src="' +C_ROOT + 'NG3Resource/icons/Location.gif"/>';
            }

        }
});
}
},
        //刷新
            refreshData: function () {
            var me = this;
            var mestore = me.getStore();
            me.getRootNode().removeAll();
        mestore.load();
}
});















//i6s合同树
Ext.define('Ext.ng.cntFunTreei6s', {
    extend: 'Ext.ng.TreePanel',
    autoLoad: false,
    alias: 'widget.ngCntFunTreei6s',
    //初始化组件	
    initComponent: function () {
        var me = this;
        me.treeFields = [
            //{ name: 'phid', type: 'string', mapping: 'phid' },
            { name: 'con_systemcode', type: 'string', mapping: 'con_systemcode' },
            { name: 'subtype', type: 'string', mapping: 'subtype' },
			{ name: 'contype', type: 'string', mapping: 'contype' },
			{ name: 'catalogcode', type: 'string', mapping: 'catalogcode' },
            { name: 'node', type: 'string', mapping: 'node' },
            { name: 'pnode', type: 'string', mapping: 'pnode' },
            { name: 'ccontype', type: 'string', mapping: 'ccontype' },
            { name: 'pcontype', type: 'string', mapping: 'pcontype' },
            { name: 'catalog', type: 'string', mapping: 'catalog' },
            { name: 'pcatalog', type: 'string', mapping: 'pcatalog' },
            { name: 'affixto', type: 'string', mapping: 'affixto' },
            { name: 'paffixto', type: 'string', mapping: 'paffixto' },
			{ name: 'employee', type: 'string', mapping: 'employee' },
            { name: 'pemployee', type: 'string', mapping: 'pemployee' },
			{ name: 'text', type: 'string', mapping: 'text' }
        ];
        //覆盖父类同名方法
        me.callParent();
        //右键菜单
        me.menu = Ext.create('Ext.menu.Menu', {
            items: [{
                text: '合同卡片',
                handler: function () {
                    var node = me.getSelectionModel().getSelection()[0];
                    if (node.data.con_systemcode) {
                        if (window.external) {
                            window.external.ShowManagerWithParm("ServiceContractEditManager", 6, "con_systemcode=" + node.data.con_systemcode);
                        }
                    }
                }
            }, {
                text: '合同新增',
                handler: function () {
                    var node = me.getSelectionModel().getSelection()[0];
                    if (node.data.contype && node.data.catalogcode) {
                        if (window.external) {
                            window.external.ShowManagerWithParm("ServiceContractEditManager", 1, "ConType=" + node.data.contype + "@@**catalogcode=" + node.data.catalogcode);
                        }
                    }
                }
            }]
        });
    },
    //根节点	
    root: {
        expanded: true,
        text: "合同",
        typename: 'root',
        proname: 'root',
        node: 'root'
    },
    height: 300,
    width: 300,
    split: true,
    border: false,
    rootVisible: true,
    layout: 'fit',
    url: C_ROOT + 'DMC/BasicData/ConContract/LoadCntFunTree',
    loadData: function (showtype) {
        Ext.apply(this.getStore().proxy.extraParams, { 'showtype': showtype });
        this.store.load();
    },
    dockedItems: [{
        xtype: 'toolbar',
        dock: 'top',
        layout: 'border',
        minWidth: 200,
        //height: 25,
        height: 30,
        items: [{
            // itemId: 'add',
            // region: 'west',
            // //width: 45,
            // text: '自定义',
            // handler: function () {
            // $OpenTab("自定义", C_ROOT + 'DMC/BasicData/ConContract/CntFunTreeList')
            // }
            // }, {
            itemId: 'refresh',
            region: 'east',
            //width: 45,
            text: '刷新',
            handler: function () {
                var ngCntFunTreei6s = this.findParentByType('ngCntFunTreei6s');
                ngCntFunTreei6s.store.proxy.url = C_ROOT + 'DMC/BasicData/ConContract/LoadCntFunTree?type=all';
                ngCntFunTreei6s.getStore().load();
            }
        }]
    }
    , {
        xtype: 'toolbar',
        dock: 'top',
        layout: 'border',
        minWidth: 200,
        height: 30,
        items: [{
            region: 'west',
            maxWidth: 80,
            //grow: true,
            xtype: 'textfield',
            itemId: 'i6sCntFunTree_query',
            name: 'queryname',
            emptyText: '搜索内容',
            enableKeyEvents: true,
            listeners: {
                'keydown': function (el, e, eOpts) {
                    if (e.getKey() == e.ENTER) {
                        var ngCntFunTreei6s = this.findParentByType('ngCntFunTreei6s');
                        ngCntFunTreei6s.store.proxy.url = C_ROOT + 'DMC/BasicData/ConContract/LoadCntFunTree?type=all&&condition=' + ngCntFunTreei6s.queryById('i6sCntFunTree_query').getValue();
                        ngCntFunTreei6s.getStore().load();
                    }
                }
            }
        }, {
            text: '搜索',
            itemId: 'query',
            region: 'east',
            //width: 30,
            handler: function () {
                var ngCntFunTreei6s = this.findParentByType('ngCntFunTreei6s');
                ngCntFunTreei6s.store.proxy.url = C_ROOT + 'DMC/BasicData/ConContract/LoadCntFunTree?type=all&&condition=' + ngCntFunTreei6s.queryById('i6sCntFunTree_query').getValue();
                ngCntFunTreei6s.getStore().load();
            }
        }, {
            itemId: 'setting',
            text: '设置',
            iconCls: "icon-Setup",
            maxWidth: 80,
            region: 'east',
            menu: {
                items: [{
                    itemId: 'type', text: '合同类型'
                            , handler: function () {
                                var cntFunTreei6s = this.findParentByType('ngCntFunTreei6s');
                                cntFunTreei6s.loadData("type");
                            }
                }, {
                    itemId: 'catalog', text: '合同目录'
                            , handler: function () {
                                var cntFunTreei6s = this.findParentByType('ngCntFunTreei6s');
                                cntFunTreei6s.loadData("catalog");
                            }
                }, {
                    itemId: 'affixto', text: '签约组织'
                            , handler: function () {
                                var cntFunTreei6s = this.findParentByType('ngCntFunTreei6s');
                                cntFunTreei6s.loadData("affixto");
                            }
                }, {
                    itemId: 'employee', text: '业务员'
                            , handler: function () {
                                var cntFunTreei6s = this.findParentByType('ngCntFunTreei6s');
                                cntFunTreei6s.loadData("employee");
                            }
                }]
            }
        }]
    }
    ],
    listeners: {
        'afterrender': function () {
            this.getRootNode().expand();
        },
        'itemclick': function (view, record, item, idx, event, eOpts) {
        },
        'itemdblclick': function (view, rcd, item, idx, event, eOpts) {
            if (rcd.raw.con_systemcode) {
                if (window.external) {
                    window.external.ShowManagerWithParm("ServiceContractEditManager", 6, "con_systemcode=" + rcd.raw.con_systemcode);
                }
            }
        },
        'checkchange': function (node, checked) {
            setChildNodeChecked(node, checked); //同时选择下级
        },
        'itemcontextmenu': function (view, rec, node, index, e) {
            e.stopEvent();
            var root = this.getRootNode();
            this.menu.rootNode = root;
            this.menu.showAt(e.getXY());
            return false;
        },
        'containercontextmenu': function (treeP, e, eOpts) {
            e.stopEvent();
            var root = this.getRootNode();
            this.menu.rootNode = root;
            this.menu.showAt(e.getXY());
            return false;
        }
    }
});

//功能导航 "tabPageNavigation"
var tabPageNavigationStore = Ext.create('Ext.data.TreeStore', {
    root: {
        expanded: true,
        children: [
            {
                text: "功能导航", expanded: true, children: [
                  { text: "功能导航自定义", leaf: true }
                ]
            }, {
                text: "导航中心测试", expanded: true, leaf: true
            }
        ]
    }

});

Ext.define('Ext.ng.tabPageNavigation', {
    //extend: 'Ext.ng.TreePanel',
    extend: 'Ext.tree.Panel',
    alias: 'widget.ngTabPageNavigation',
    store: tabPageNavigationStore,
    rootVisible: false,
    initComponent: function () {
        var me = this;
        this.callParent();
        me.menu = Ext.create('Ext.menu.Menu', {
            selectedNode:null,
            items: [
                 {
                     text: '删除',
                     handler: function () {
                         var node = me.menu.selectedNode;
                         var text = node.raw.text;
                         node.remove();
                         Ext.Ajax.request({
                             async: false, //同步请求
                             params: {
                                 'text': text
                             },
                             url: C_ROOT + 'SUP/IndividualNavigation/Delete',                            
                             success: function (response) {
                                 //if (response.responseText == 'True') {
                                 //    node.remove();
                                 //}
                             }
                         });
                     }
                 }
            ]
        });
    },
    listeners: {
        'afterrender': function (treepanel, e) {
            if (treepanel.getRootNode().childNodes[0].childNodes.length > 1) {
                return;
            }
            var text;
            Ext.Ajax.request({
                async: false, //同步请求
                url: C_ROOT + 'SUP/IndividualNavigation/LoadTree',
                success: function (response) {
                    text = Ext.JSON.decode(response.responseText);
                }
            });
            var root = treepanel.getRootNode();
            for (var i = 0; i < text.length; i++) {
                root.childNodes[0].appendChild({                  
                    text: (text[i].text == null || text[i].text == '') ? '默认功能导航图' : text[i].text,
                    leaf: true,
                    expanded: true
                });
            }
        },
        'itemdblclick': function (view, rcd, item, idx, event, eOpts) {
            if (rcd.data.text == "功能导航") {
                return;
            }
            if (rcd.data.text == "导航中心测试") {
                $OpenTab("导航中心", C_ROOT + 'SUP/NavigationCenter/NavigationCenter');
                return;
            }
            if (rcd.data.text == "功能导航自定义") {               
                $OpenTab("功能导航自定义", C_ROOT + 'SUP/MainTree/IndividualNavigation?text=' + rcd.data.text);
            } else {
                $OpenTab(rcd.data.text, C_ROOT + 'SUP/MainTree/Navigation?text=' + rcd.data.text);
            }
        },
        'itemcontextmenu': function (view, rec, node, index, e) {
            e.stopEvent();
            if (rec.raw.text == '功能导航自定义' || rec.raw.text == '功能导航') {
                return;
            }
            this.menu.selectedNode = rec;
            this.menu.showAt(e.getXY());
            return false;
        },
    }
});
//报表树
Ext.define('Ext.ng.RWReportTree', {
    extend: 'Ext.tree.TreePanel',
    alias: 'widget.ngRwReportTree',
    region: 'center',
    autoScroll: true,
    animate: true,
    collapsible: false,
    useArrows: true,
    rootVisible: false,
    align: 'center',
    hideHeaders: true,
    selectMode: 'Single',
    nodeIndex: -1,
    type: 'MY',
    filter: '',
    columns: [
        {
            text: '物理主键',
            flex: 0,
            sortable: false,
            dataIndex: 'phid',
            hideable: false,
            hidden: true
        }, {
            text: '代码',
            flex: 0,
            dataIndex: 'code',
            sortable: false,
            hideable: false,
            hidden: true
        }, {
            text: '名称',
            flex: 1,
            xtype: 'treecolumn',
            dataIndex: 'text',
            hidden: false,
            hideable: false,
            align: 'left'
        }, {
            text: '名称',
            flex: 0,
            dataIndex: 'name',
            hidden: true,
            hideable: false,
            sortable: false,
            align: 'left'
        }, {
            text: '报表',
            flex: 0,
            dataIndex: 'rep_id',
            hidden: true,
            hideable: false,
            sortable: false,
            align: 'left'
        }, {
            text: '报表',
            flex: 0,
            dataIndex: 'isreport',
            hidden: true,
            hideable: false,
            sortable: false,
            align: 'left'
        }
    ],
    initComponent: function () {
        var me = this;
        Ext.define('model', {
            extend: 'Ext.data.Model',
            fields: [
                {
                    name: 'phid',
                    type: 'string',
                    mapping: 'phid'
                }, {
                    name: 'code',
                    type: 'string',
                    mapping: 'code'
                }, {
                    name: 'text',
                    type: 'string',
                    mapping: 'text'
                }, {
                    name: 'name',
                    type: 'string',
                    mapping: 'name'
                }, {
                    name: 'isreport',
                    type: 'string',
                    mapping: 'isreport'
                }, {
                    name: 'rep_id',
                    type: 'string',
                    mapping: 'rep_id'
                }
            ]
        });
        var store = Ext.create('Ext.data.TreeStore', {
            model: 'model',
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: C_ROOT + 'RW/DesignFrame/GetRepGuideTree'
            }
        });
        store.on('beforeload', function (store) {
            if (me.type) {
                Ext.apply(store.proxy.extraParams, { 'type': me.type });
            }
            if (me.filter || me.filter == '') {
                Ext.apply(store.proxy.extraParams, { 'filter': me.filter });
            }
        });
        me.store = store;
        me.dockedItems = [
            {
                xtype: 'toolbar',
                //height: 26,
                height: 30,
                dock: 'top',
                layout: 'border',
                minWidth: 200,
                items: [
                    {
                        region: 'center',
                        xtype: 'textfield',
                        id: 'RWReportTree_query',
                        name: 'queryname',
                        emptyText: '搜索内容'
                    }, {
                        region: 'east',
                        width: 40,
                        text: '搜索',
                        handler: function () {
                            var searchfilter = me.down('#RWReportTree_query');
                            me.filter = searchfilter.value;
                            store.load();
                        }
                    }, {
                        region: 'east',
                        width: 40,
                        text: '刷新',
                        anchor: '100%',
                        handler: function () {
                            var searchfilter = me.down('#RWReportTree_query');
                            searchfilter.setValue('');
                            me.filter = '';
                            store.load();
                        }
                    }
                ]
            }
        ];
        me.callParent();
    },
    listeners: {
        'itemdblclick': function (item, record, it, index, e, eOpts) {
            var id = record.data.rep_id;
            var isreport = record.data.isreport;
            var name = record.data.name;
            if (isreport == "1") {
                $OpenTab(name, C_ROOT + 'RW/DesignFrame/ReportView?otype=view&page=reportwarehousetree&id=' +id);
            }

        }
    }
});
Ext.define('Ext.ng.RWReportGuideTree', {
    extend: 'Ext.form.Panel',
    alias: 'widget.ngRwReportGuideTree',
    animate: true,
    collapsible: false,
    useArrows: true,
    rootVisible: false,
    align: 'center',
    autoScroll: false,
    hideHeaders: true,
    selectMode: 'Single',
    nodeIndex: -1,
    region: 'center', //默认center
    layout: 'border',
    initComponent: function () {
        var me = this;
        var treepanel = Ext.create('Ext.ng.RWReportTree');
        var toptoolbar = Ext.create('Ext.ng.Toolbar', {
            region: 'north',
            showArrowBtn: false,
            ngbuttons: [{
                text: '自定义',
                handler: function () {
                    var searchfilter = treepanel.down('#RWReportTree_query');
                    searchfilter.setValue('');
                    treepanel.filter = '';
                    treepanel.type = 'MY';
                    treepanel.store.load();
                }
            }, {
                text: '全部',
                handler: function () {
                    var searchfilter = treepanel.down('#RWReportTree_query');
                    searchfilter.setValue('');
                    treepanel.filter = '';
                    treepanel.type = 'ALL';
                    treepanel.store.load();
                }
            }, '->', {
                text: '自定义菜单',
                handler: function () {
                    $OpenTab('自定义报表菜单', C_ROOT + 'RW/DesignFrame/RepGuideTree');
                }
            }]
        });
        me.items = [toptoolbar, treepanel];
        me.loadData = function () {
            treepanel.store.load();
        };
        me.refreshData = function () {
            treepanel.store.load();
        };
        me.callParent();
    }
});









