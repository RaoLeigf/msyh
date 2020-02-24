w = window;

var loginpop = false;


var Cookie = Ext.create('Ext.state.CookieProvider',{
    path: "/",
    expires: new Date(new Date().getTime() + (1000 * 60 * 60 * 24 * 30)) //30 days
});

//操作类型
var $Otype = { ADD: 'add', EDIT: 'edit', VIEW: 'view' };


function $path(path) {
    //将 path 中 @ 替换成 当前程序路径
    if (path.indexOf("~/") == 0) {
        path = C_ROOT + path.substr(2);
    };

    return path;
}

//获取主界面对象
function $GetWFrame() {
    try {
        var cw = window;
        while (cw != top) {
            if (cw.MainFrame)
                break;
            else
                cw = cw.parent;
        }
    } catch (e) { }
    return cw.MainFrame;
};

function $OpenTab(title, url, params) {
    var f = $GetWFrame();
    if (f && f.Center) {
        f.Center.openTab(title, url, params);
    }
    else {
        var nw = window.open($path(url), title, 'toolbar=no, menubar=no, scrollbars=no,resizable=no,location=no, status=no');

        nw.lastArguments = params || {};
    }
}

//关闭tab页
function $CloseTab(n) {
    var f = $GetWFrame();
    if (f && f.Center) {

        //debugger;
        //f.Checker.fireEvent('checkin', f.Checker);//触发签入操作
      
        var tab = f.Center.getComponent(n) || f.Center.getActiveTab();

        f.Center.remove(tab);
    }
    else {
        w.close();
    }
};

//#region 打印方法

//通过Grid打印列表(直接打印当前页)
function $Print(grid) {
    if (!grid) {
        Ext.Msg.alert("Error", "Cannot Get Grid Object!");
        return false;
    }
    else if (grid.store.getRange().length < 1) {
        Ext.Msg.alert("Info", "No Data!");
        return false;
    }
    else {
        var f = $GetWFrame();
        var title = f.Center.activeTab.title;
        var printObj = new Object();
        printObj.ctype = f.Center.activeTab.id;
        printObj.grid = grid;
        printObj.title = title;
        f.Center.printObj = printObj;
        f.Center.openTab(title + '打印', C_ROOT + 'Print/Index', "Print");
    }
}

//通过Grid打印列表(连续打印所有页)
function $PrintEx(grid) {
    if (!grid) {
        Ext.Msg.alert("Error", "Cannot Get Grid Object!");
        return false;
    }
    else if (grid.store.getRange().length < 1) {
        Ext.Msg.alert("Info", "No Data!");
        return false;
    }
    else {
        if (!grid.initialConfig.bbar) { //没有分页条时
            $Print(grid);
            return false;
        }
        var f = $GetWFrame();
        var curtab = f.Center.activeTab; //获取tab页容器
        var title = curtab.title;
        var printObj = new Object();

        printObj.ctype = f.Center.activeTab.id;
        printObj.curTab = curtab;
        printObj.grid = grid;
        printObj.title = title;
        printObj.Closed = false;
        f.Center.printObj = printObj;
        f.Center.openTab(title + '打印', C_ROOT + 'Print/PrintEx', "PrintEx");

        var actfunc = function () {  //激活tab页后需要刷新grid视图
            grid.getView().refresh(true);
            if (printObj.Closed) {
                curtab.un('activate', actfunc);
            }
        };
        curtab.un('activate', actfunc);
        curtab.on('activate', actfunc);
    }
}

//打印明细页面Form单据(或者grid，可以多个grid)
function $PrintForm(form, grids, titles) {
    $PrintDetail(form, grids, titles, "", "", "");
}

//此方法不建议直接调用
function $PrintDetail(form, grids, titles, printid, typefile, moudletype) {
    if (!form) {
        Ext.Msg.alert("Error", "Cannot Get Form Object!");
        return false;
    }
    if (!form.length) {
        form = [form];
    }
    if (!form[0].buskey) {
        Ext.Msg.alert("Error", "Cannot Get buskey!");
        return false;
    }
    else {
        if (!titles) { titles = []; }
        var f = $GetWFrame();
        var title = titles[0] ? titles[0] : f.Center.activeTab.title;
        var printObj = new Object();
        printObj.grids = grids;
        printObj.titles = titles;
        printObj.title = title;
        printObj.form = form;
        printObj.printid = printid;       //模板编号
        printObj.typefile = typefile;     //单据类型
        printObj.moudletype = moudletype; //模板类型
        printObj.page = f.Center.activeTab.id.split("?")[0];
        f.Center.printObj = printObj;
        f.Center.openTab(title + '打印', C_ROOT + 'Print/PrintDetail', "PrintDetail");
    }
}

//打印多个grid
function $PrintGrids(buskey, grids, titles) {   //buskey = { "id": "", "value": "" };
    if (!grids || grids.length == 0) {
        Ext.Msg.alert("Error", "Cannot Get Form Grids!");
        return false;
    }
    if (!buskey) {
        Ext.Msg.alert("Error", "Cannot Get buskey!");
        return false;
    }
    else {
        if (!titles) { titles = []; }
        var f = $GetWFrame();
        var title = titles[0] ? titles[0] : f.Center.activeTab.title;
        var printObj = new Object();
        printObj.grids = grids;
        printObj.titles = titles;
        printObj.title = title;
        printObj.form = null;
        printObj.buskey = buskey;
        printObj.page = f.Center.activeTab.id.split("?")[0];
        f.Center.printObj = printObj;
        f.Center.openTab(title + '打印', C_ROOT + 'Print/PrintDetail', "PrintDetail");
    }
}

//套打方法调用
function $PrintHelp(typefile, form, grids, titles) {
    var f = $GetWFrame();
    var tab = f.Center.activeTab;
    if (!tab.printWindowHelp) {
        var toolbar = Ext.create('Ext.Toolbar', {
            region: 'north',
            border: false,
            height: 26,
            minSize: 26,
            maxSize: 26,
            items: [{
                id: "help_print",
                text: "打印",
                iconCls: 'icon-Print',
                handler: function () {
                    var data = grid.getSelectionModel().getSelection();
                    if (data.length > 0) {
                        var printId = data[0].get('printid');    //模板编号
                        var moudleType = data[0].get('def_int2');  //模板类型
                        tab.printWindowHelp.hide();
                        $PrintDetail(form, grids, titles, printId, typefile, moudleType);
                    }
                    else {
                        Ext.MessageBox.alert("提示", "请先选择打印模板.");
                    }
                }
            }, {
                id: "help_printex",
                text: "直接打印",
                iconCls: 'icon-Print',
                handler: function () {
                    tab.printWindowHelp.hide();
                    $PrintDetail(form, grids, titles, "", typefile, "");
                }
            }, {
                id: "help_delete",
                text: "删除模板",
                iconCls: 'icon-delete',
                handler: function () {
                    var data = grid.getSelectionModel().getSelection();
                    if (data.length > 0) {
                        var moudleType = data[0].get('def_int2');  //模板类型
                        if (moudleType == "2") {
                            var printId = data[0].get('printid');    //模板编号
                            Ext.Msg.confirm("提示信息", "你确认删除 " + grid.getSelectionModel().getSelection()[0].get("billname") + " 模板",
                            function (btn) {
                                if (btn == "yes") {
                                    Ext.Ajax.request({
                                        params: { printid: printId },
                                        url: C_ROOT + "Print/DeleteModule",
                                        success: function (response) {
                                            var resp = Ext.JSON.decode(response.responseText);
                                            if (resp.status === "ok") {
                                                //                                                Ext.MessageBox.alert("提示", resp.msg);
                                                if (resp.count > 0) {
                                                    tab.tmpStore.cachePageData = false;
                                                    tab.tmpStore.load();
                                                    tab.tmpStore.cachePageData = true;
                                                }
                                            } else {
                                                Ext.MessageBox.alert("提示", resp.msg);
                                            }
                                        }
                                    });
                                }
                            });
                        }
                        else {
                            Ext.MessageBox.alert("提示", "只能删除用户自定义的模板数据.");
                        }
                    }
                    else {
                        Ext.MessageBox.alert("提示", "请先选择要删除的模板.");
                    }
                }
            }, "->", {
                id: "help_close",
                text: "关闭",
                iconCls: 'cross',
                handler: function () {
                    tab.printWindowHelp.hide();
                }
            }]
        });
        Ext.define('TemplateModel', {  //定义模型
            extend: 'Ext.data.Model',
            fields: [{
                name: 'printid',
                type: 'string',
                mapping: 'printid'
            }, {
                name: 'typefile',
                type: 'string',
                mapping: 'typefile'
            }, {
                name: 'billname',
                type: 'string',
                mapping: 'billname'
            }, {
                name: 'moduleno',
                type: 'string',
                mapping: 'moduleno'
            }, {
                name: 'filename',
                mapping: 'filename',
                type: 'string'
            }, {
                name: 'dateflg',
                mapping: 'dateflg',
                type: 'string'
            }, {
                name: 'def_int2',
                mapping: 'def_int2',
                type: 'string'
            }, {
                name: 'remarks',
                mapping: 'remarks',
                type: 'string'
            }]
        });
        var store = Ext.create('Ext.ng.JsonStore', {
            model: 'TemplateModel',
            autoLoad: true,
            url: C_ROOT + 'Print/GetFmtTemplateFromDb?typefile=' + typefile
        });
        tab.tmpStore = store;
        var grid = Ext.create('Ext.ng.GridPanel', {
            region: 'center',
            store: store,
            columnLines: true,
            autoScroll: true,
            forceFit: true,
            scroll: "vertical",
            layout: {
                //                align: 'stretch',
                //                type: 'hbox'
            },
            viewConfig: {
                scrollOffset: 0
            },
            columns: [{
                header: '编号',
                flex: 1,
                sortable: true,
                dataIndex: 'printid'
            }, {
                header: '单据类型',
                flex: 1.5,
                sortable: true,
                dataIndex: 'typefile',
                renderer: function (val, meta) {
                    if (meta) {
                        meta.tdAttr = "title='" + val + "'";
                    }
                    return val;
                }
            }, {
                header: '模板名称',
                flex: 1.5,
                sortable: true,
                dataIndex: 'billname',
                renderer: function (val, meta) {
                    if (meta) {
                        meta.tdAttr = "title='" + val + "'";
                    }
                    return val;
                }
            }, {
                header: '文件名称',
                flex: 2,
                titleAlign: 'center',
                sortable: true,
                dataIndex: 'filename',
                renderer: function (val, meta) {
                    if (meta) {
                        meta.tdAttr = "title='" + val + "'";
                    }
                    return val;
                }
            }, {
                header: '类型',
                flex: 1,
                sortable: true,
                dataIndex: 'def_int2',
                renderer: function (val) {
                    return (val === '1') ? '系统' : (val === '2' ? '自定义' : '用户');
                }
            }, {
                header: '备注',
                flex: 1.5,
                titleAlign: 'center',
                sortable: true,
                dataIndex: 'remarks',
                hidden: true
            }]
        });
        grid.on('itemdblclick', function () {
            var data = grid.getSelectionModel().getSelection();
            if (data.length > 0) {
                var printId = data[0].get('printid');    //模板编号
                var moudleType = data[0].get('def_int2');  //模板类型
                tab.printWindowHelp.hide();
                $PrintDetail(form, grids, titles, printId, typefile, moudleType);
            }
        });
        var y = (this.frameElement.offsetHeight - 300) / 3;
        tab.printWindowHelp = Ext.create('Ext.window.Window', {
            title: "选择套打模板",
            border: false,
            height: 300,
            closable: false,
            width: 500,
            y: y,
            layout: 'border',
            modal: true,
            resizable: false,
            draggable: false,
            items: [toolbar, grid]
        });
    }
    else {
        tab.tmpStore.cachePageData = false;
        tab.tmpStore.load();
        tab.tmpStore.cachePageData = true;
    }
    tab.printWindowHelp.show();
}

//#endregion

//#region Ext.Ajax处理

//将 Ajax.request 的 url 中 @ 替换成 当前页的名称
Ext.Ajax.on('beforerequest', function (conn, opts) {
    //opts.url = $url(opts.url);
    //opts.params 的一级子对象转换为字符串
    if (Ext.isObject(opts.params)) {
        for (var p in opts.params) {
            if (Ext.isArray(opts.params[p]) || Ext.isObject(opts.params[p])) {
                opts.params[p] = Ext.encode(opts.params[p]);
            }
        }
    }
});
//对Ajax返回串进行有效性验证
Ext.Ajax.on('requestcomplete', function (conn, res, opts) {
    res.options = opts;
    res.text = res.responseText
    try {
        $ValidateResponse(res);
    }
    catch (e) {
        Ext.Msg.alert(e.message, res.responseText);
    }
});
//当发生错误时,统一处理
Ext.Ajax.on('requestexception', function (conn, res, opts) {
    if (res.status == 0)
        Ext.Msg.alert('requestexception', 'url:' + opts.url + '<br/>' + res.statusText);
});
//#endregion

function $ValidateResponse(xhr) {
    //debugger;       
    if (xhr.valid === undefined) {
        var info = $ajaxInfo = $GetAjaxInfo(xhr);
        if (!$ajaxInfo) return true;
        $ajaxData = $ajaxInfo.data;
        if ($ajaxData && $ajaxData.NeedLogin) {

            //显示登陆窗口
            var f = parent.MainFrame || w.MainFrame;//parent.WFrame || w.WFrame;
            if (f) {
                var opts = xhr.options;
                if (opts.url && opts.url.charAt(0) != '/') {
                    opts.url = $path(C_PATH + opts.url);
                }
                //f.Login.open(opts);                

                if (!loginpop) {
                    f.PopLogin(opts);
                    loginpop = true;
                }
                //var wf = $GetWFrame();
                 f.NotCompleteRequest.push(opts);
                return true;
            }
        }
        if (info.msgtype && info.msg) {
            var msg = unescape(info.msg);
            var title = unescape(info.msgtitle);
            //var icon = Ext.MessageBox[info.msgicon.toUpperCase()];
            switch (info.msgtype) {
                case 'Alert':
                    alert(msg);
                    break;
                default:
                    alert(info.msgtype + ' 未实现,在common.js');
                    break;
            }
        }
        xhr.valid = info.type == 'Succ';
    }
    return xhr.valid;
}

function $GetAjaxInfo(response) {

     
    //必须加上trim,IE中的BUG,会在后面加一个空格
    if (response.getResponseHeader("AjaxInfo")) {

        return Ext.decode(response.getResponseHeader("AjaxInfo"));
    }
    else
    {
        return '';
    }
}
