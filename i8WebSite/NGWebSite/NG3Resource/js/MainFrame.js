///<reference path="../../extjs/ext-all-debug.js">
var MainFrame = {};
(function (WF) {

   WF.config = (function () {
        var pagePath = C_ROOT + 'Main/Frame/',
        imgPath = C_ROOT + 'Resource/pic/';
        return {
            pagePath: pagePath,
            //登录处理页面
            loginUrl: C_ROOT + "Main/Login/LoginPart",
            logo: imgPath + 'logo.gif',
            headerImg: imgPath + 'header.gif?Ver=2',
            leftWidth: 200,
            rightWidth: 200,
            headHeight: 86
        };
    })();

    //#region HeadToolbar
    WF.HeadToolbar = Ext.extend(Ext.Toolbar, {
        defaults: {
            width: 55
        },
        region:'north',
        id: "MainBar",
        initComponent: function () {
            this.items = ['->', {
                iconCls: 'icon-fold',
                text: '隐藏',
                handler: function () {
                    if (this.iconCls == 'icon-unfold') {
                        WF.Head.setHeight(WF.config.headHeight);
                        this.setIconCls('icon-fold');
                        this.setText("隐藏");
                    }
                    else {
                        WF.Head.setHeight(26);
                        this.setIconCls('icon-unfold');
                        this.setText("显示");
                    }

                    WF.viewport.doLayout();
                }
            },
//            {
//                iconCls: 'icon-Key',
//                text: '修改密码',
//                width: 80,
//                handler: function () {
//                    WF.ShowDialog({ url: C_ROOT + "Main/Frame/ChangePwd", closable: false, title: "修改密码", width: 320, height: 200, modal: true });
//                }
//            },
//            {
//                iconCls: 'icon-Lock',
//                text: '锁定',
//                handler: function () {
//                    WF.Login.open({ lock: true });
//                }
//            },
//             {
//                iconCls: 'icon-maddress',
//                text: '关于',
//                handler: function () {
//                     WF.ShowDialog({ url: C_ROOT + "Main/Frame/ProductAbout", closable: true, title: "新中大工程项目管理沙盘软件", width: 436, height: 500, modal: true });
//                }
//            },
//            {
//                iconCls: 'icon-Refresh',
//                text: '刷新',
//                handler: function () {
//                    window.location.href = WF.config.pagePath + "Index";
//                }
//            }, 
                {
                iconCls: 'icon-UserGo',
                text: '注销',
                handler: function () {
                    Ext.Msg.confirm('提示', '确定要注销吗?', function (btn) {
                        if (btn == 'yes') {
                            location.href = C_ROOT; //+ 'Home/Login/';
                             Ext.Ajax.abortAll();
                            WF.loadMarsk("正在注销, 请稍等...").show();
                            location.href = C_ROOT; //+ 'Home/Login/';
                        }
                    });
                }
            },
            {
                iconCls: 'icon-Exit',
                text: '退出',
                handler: function () {

                    Ext.Msg.confirm('提示', '确定要退出系统吗?', function (btn) {
                        if (btn == 'yes') {
                             Ext.Ajax.abortAll();                            
                            var re = window.open("", "_self");
                            //re.close();
                            window.close();
                        }
                    });

                }
            }
            ];
            WF.HeadToolbar.superclass.initComponent.call(this);
        }
    });
    //#endregion

    //#region Head
    WF.Head = (function () {
        var logo = new Ext.form.Label({
            width: 320,
            height: 60,
            html: '<img src="' + WF.config.logo + '"/>'
        });
        var apps = new Ext.Container({
            flex: 1,
            html: ''
        });
        var himg = new Ext.Container({
            width: 480,
            height: 60,
            html: '<img align=right src="' + WF.config.headerImg + '"/>'
        });
        return new Ext.Panel({
            region: 'north',
            layout: 'hbox',
            align: 'stretch',
            style: 'background-color:white',
            border: false,
            height: WF.config.headHeight,
            items: [logo, apps, himg],
            bbar: new WF.HeadToolbar()
        });
    })();
    //#endregion

    //#region Left

    WF.Left = (function () {

        //    return new Ext.Panel({
        //        region: 'west',
        //        id:"leftpanle",
        //        title:'功能菜单',
        //        iconCls: 'icon-use-mode',
        //        //border:false,
        //        layout: 'fit',
        //        //plugins:VMODE,
        //        split: true,
        //        collapsible: true,
        //        //animCollapse: true,
        //        //collapseMode: 'mini',
        //        maxWidth: 400,
        //        width: WF.config.leftWidth
        //    });

//        var treeStore1 = Ext.create('Ext.data.TreeStore', {
//               autoLoad: false,
//               fields: [{ name: 'text', type: 'string' },
//               { name: 'my', type: 'string'}//我的自定义属性                
//               ],
//               proxy: {
//                   type: 'ajax',
//                   url: C_ROOT + 'Tree/LoadTree'
//               },
//               folderSort: true,
//               sorters: [{
//                   property: 'text',
//                   direction: 'ASC'
//               }]
//           });

           //menu
//           var tree1 = Ext.create('Ext.tree.TreePanel',{
//                    id: "tree1",
//                    store: treeStore1,
//                    region: "west",
//                    title: "员工信息",
//                    autoScroll: true,
//                    enableTabScroll: true,
//                    collapsible: true,
//                    collapsed: false,
//                    split: true,
//                    rootVisible: false,
//                    lines: true,
//                    useArrows: true,
//                    width: 220,
//                    minSize: 220,
//                    maxSize: 220,                
//                    listeners: {
//                        'afterrender': function () {
//                            //Ext.getCmp("maintree").expandAll();
//                        },
//                        'itemdblclick': function (view, rcd, item, idx, event, eOpts) {
//                            
//                            if (rcd.raw.leaf) {
//                                WF.Center.openTab(rcd.raw.text, rcd.raw.url);
//                            }
//                        }
//                  }                          
//                });

           var menuTreeStore = Ext.create('Ext.data.TreeStore', {
                    autoLoad: false,
                    fields: [{ name: 'text', type: 'string' },
                   { name: 'my', type: 'string'}//我的自定义属性                
               ],
                proxy: {
                    type: 'ajax',
                    url: C_ROOT + 'HR/EmpInfoList/LoadMenu'
                }    
              });

           var menuTree = Ext.create('Ext.ng.TreePanel', {
               title:'供应商门户',     
               treeFields: [{ name: 'text', type: 'string' },
                   { name: 'my', type: 'string'}//我的自定义属性                
               ],
               url: C_ROOT + 'SUP/MainTree/LoadMenu', //'HR/EmpInfoList/LoadMenu'
               listeners: {
                  'afterrender': function () {
                            this.expandAll();
                        },                   
                    'itemdblclick': function (view, rcd, item, idx, event, eOpts) {
                            
                        if (rcd.raw.leaf) {
                            WF.Center.openTab(rcd.raw.text, rcd.raw.url);
                        }
                    }
               }                          
           });

            var accordion = Ext.create('Ext.Panel', {
               title: '功能菜单',
               iconCls: 'icon-occupy-mode',
               collapsible: true,
               region: 'west',
               margins: '5 0 5 5',
               split: true,
               width: 210,
               layout: 'accordion',
               items: [menuTree] 
           });

           return accordion;
    })();
    //#endregion

    //#region Center
    WF.Center = new Ext.TabPanel({
        region: 'center',
        activeTab: 0,
        enableTabScroll: true,
        items: [{
            id: 'home',
            title: "首页",
            iconCls: 'icon-Home',
            closeable: true,
            autoLoad: {
                url: WF.config.pagePath + 'Main',
                scope: this,
                scripts: true
            }
        }],
        listeners: {
            tabchange: function (me, tab) {


                //                 if(Ext.isIE == false)
                //                 {
                //                    try{
                //                     if(tab.body.dom.firstChild)
                //                     {
                //                        //alert(tab.body.dom.firstChild.contentWindow.location.reload);
                //                        var wind = tab.body.dom.firstChild.contentWindow
                //                        wind.location.href =  wind.location.href;
                //                        wind.location.reload();
                //                     }
                //                    }
                //                    catch(e){}
                //                 }

                if (Ext.isIE6) {
                    try {
                        if (tab.body.dom.firstChild)
                            setTimeout(tab.body.dom.firstChild.contentWindow.onresize(), 200);
                    }
                    catch (e) { }
                }
            },
            beforeremove: beforeRemoveTab
        }
    });

    WF.MainPanel = new Ext.Panel({
            region: 'center',
            layout: 'border',
            items:[WF.Center,new WF.HeadToolbar() ]
    });

    function beforeRemoveTab(tab, panel) {
        //debugger;    

        WF.Checker.fireEvent('checkin', WF.Checker); //触发checkin事件

        //       Ext.MessageBox.show({
        //                          title:"操作确认",
        //                          msg:"您确定要关闭<b>[" + panel.title +']</b>么？',
        //                          buttons: Ext.MessageBox.YESNO,
        //                          icon:Ext.MessageBox.QUESTION,
        //                          fn:function(btn,text){
        //                              if(btn == "yes")
        //                              {                              
        //                                   tab.un('beforeremove',beforeRemoveTab);

        //                                   WF.Checker.fireEvent('checkin', WF.Checker);//触发checkin事件

        //                                   tab.remove(panel);
        //                                   tab.addListener('beforeremove',beforeRemoveTab,tab);

        //                              }
        //                          }
        //                         });

        //  return false;      
    };

    function beforeClosePanel(panel) {

        Ext.MessageBox.show({
            title: "操作确认",
            msg: "您确定要关闭<b>" + panel.title + '</b>么？',
            buttons: Ext.MessageBox.YESNO,
            icon: Ext.MessageBox.QUESTION,
            fn: function (btn, text) {
                if (btn == "yes") {
                    //debugger;                                 
                    var p = WF.Center.getActiveTab();
                    WF.Center.un('beforeremove', beforeRemoveTab);

                    if (panel.body.dom.firstChild.contentWindow.say) {
                        panel.body.dom.firstChild.contentWindow.say();
                    }

                    WF.Center.remove(p);
                    WF.Center.addListener('beforeremove', beforeRemoveTab, WF.Center);
                }
            }
        });
        return false;
    };

    WF.Center.openTab = function(title, url, params,allowedClose) {
        var me = WF.Center;
        if (url) {
            var closeable = allowedClose === undefined ? true : allowedClose;
            var autoId = '';
            if (params) {
                for (var p in params) {
                    var pv = params[p];
                    if (pv != null) {
                        autoId += p + (Ext.isObject(pv) ? Ext.urlEncode(pv) : pv + '').replace(/\W/g, '').substr(0, 30);
                        ;
                    }
                }
            }
            autoId = url.replace(/[\/\.]/g, '') + autoId;

            var n = me.getComponent(autoId);
            if (!n) { //判断是否已经打开该面板
            var myMask = new Ext.LoadMask(me, {msg:"Please wait..."});
            myMask.show();
            var frame = document.createElement("IFRAME");
            frame.scrolling = "auto";
            frame.frameBorder = 0;
            frame.src = url;
            frame.height = "100%";
            frame.width = "100%";
            frame.onload=frame.onreadystatechange=function(){
                if(this.readyState &&this.readyState!="complete"){
                    return;
                }
                else{
                    myMask.hide();
                }
            }
            var tabPage = Ext.create('Ext.panel.Panel',{
                    'id': autoId,
                    'title': title,                  
                    closable: closeable,  //通过html载入目标页
                    contentEl: frame                   
                    //html: '<iframe scrolling="auto" frameborder="0" width="100%" height="100%" src="' + url + '"></iframe>'
                });
                               
                frame.parentContainer = tabPage;//父容器
                n = me.add(tabPage);
            }
            
            me.setActiveTab(autoId);

            //n.body.dom.childNodes[0].contentWindow.lastArguments = params || {};
        }
    };
    //#endregion

    //#region Right
    //    WF.Right = new Ext.Panel({
    //        region: 'east',
    //        split: true,
    //        collapsible: true,
    //        width: WF.config.rightWidth
    //    });
    //#endregion

    //#region Taskbar
    WF.Taskbar = new Ext.panel.Panel({
        id: 'Taskbar',
        title: '新中大软件',
        region: 'south',
        enableOverflow: true,
        border: false,
        changtitle: function (titlename) {
            this.setTitle(titlename);
        }
    })
    //#endregion

    //#region Login

    WF.Login = new Ext.Window({
        title: '登 录',
        autoScreen: true,
        iconCls: 'icon-Lock',
        //plugins:VMODE,
        closeAction: 'hide',
        initHidden: true,
        closable: false,
        maximizable: false,
        resizable: false,
        modal: true,
        width: 320,
        height: 200,
        layout: 'fit',
        url: WF.config.loginUrl,
        open: function (lastRequestOptions) {
            if (Ext.isEmpty(this.url)) return;
            //显示窗口
            this.show();

            this.waitIframe(this.url,
	        function () {
	            var dw = this.dw;
	            dw.win = this;

	            this.getDockedItems()[1].items.get(0).setHandler(dw.loginHandler); //ext4
	            dw.loginSuccess = Ext.Function.createSequence(dw.loginSuccess, WF.Login.loginSuccess, this);

	            this.on('hide', this.hideAction);
	        },
	        function () {
	            if (this.dw) {
	                this.dw.lastRequestOptions = lastRequestOptions;
	                if (lastRequestOptions && lastRequestOptions.lock) this.dw.lockHandler();
	                this.dealAction();
	            }
	        }
        );
        },
        hideAction: function () {

        },
        dealAction: function () {
            //this.enable();
        },
        buttons: [{ text: '登录', iconCls: 'icon-LockGo'}]
    });

    WF.Login.loginSuccess = function(json) {
        this.hide();
    };
    
    WF.PopLogin = function(lastRequestOptions) {
    
        var frame = document.createElement("IFRAME");
        frame.id = "frame1";
        frame.frameBorder = 0;
        frame.src = C_ROOT + 'Home/PopLogin';
        frame.height = "100%";
        frame.width = "100%";

        //显示登录窗口
        win = Ext.create('Ext.window.Window', {
            title: '登 录',
            autoScreen: true,
            iconCls: 'icon-Lock',
            closable: false,
            maximizable: false,
            resizable: false,
            modal: true,
            width: 300,
            height: 200,
            layout: 'fit',
            contentEl: frame
            //items: [querypanel, grid]
        }).show();                      

        frame.parentContainer = win; //弹出窗口控件传给iframe      
        frame.lastRequestOptions = lastRequestOptions;//未完成的请求
    }

    //#endregion

    //#region Dialog
    WF.ShowDialog = function (opts) {
        return $ShowDialog(opts);
    };

    WF.loadMarsk = function (msg) {
        return new Ext.LoadMask(document.body, {
            msg: msg
        });
    };

    WF.ShowInfo = function (msg) {
        Ext.MessageBox.show({
            title: '提示',
            msg: msg,
            buttons: Ext.MessageBox.OK,
            icon: Ext.MessageBox.INFO,
            closable: false
        });
    };
    //#endregion Dialog
    
    //#region Comet
    WF.Comet = {};
    //初始消息ID
    WF.Comet.msgId = 0;
    //私钥 一般为操作员ID
    WF.Comet.privateToken = '';
    //初始
    WF.InitialComet = function() {
        var me = this;
        Ext.Ajax.request({
            url: C_ROOT + 'Main/Frame/InitializeComet',
            success: function(res, opts) {
                var msg = Ext.decode(res.responseText);
                if (msg.rettype == 'true') {
                    me.Comet.privateToken = msg.msg;
                    me.RegisterComet();
                } else {
                    //出现异常，推送失败!
                }
            },
            failure: function(resp,opts) {
                debugger;
                //出现异常，推送失败!
            }
        });
    };

    WF.RegisterComet = function() {
        var me = this;
        Ext.Ajax.request({
            url: C_ROOT + 'CometAsync/DefaultChannel.ashx',
            params: { privateToken: me.Comet.privateToken, lastMessageId: me.Comet.msgId },
            success: function(res, opts) {
                var msg = Ext.decode(res.responseText);
                switch (msg[0].Type) {
                case "CometError":
                    break;
                case "CometTimeOut":
                    //超时重新刷新
                    me.RegisterComet();
                    break;
                default:
                    me.Comet.msgId = msg[0].ID;
                    //调用业务注册代码
                    try {
                       if (me.Comet[msg[0].Content.billtype]) {
                        me.Comet[msg[0].Content.billtype]();
                    }
                    } catch(e) {
                        me.Comet[msg[0].Content.billtype] = undefined;
                    } 

                    me.RegisterComet();
                    break;
                }
            },
            failure: function() {
                
            }
        });

    };
    //#endregion

    WF.init = function() {
        WF.viewport = new Ext.Viewport({
            layout: 'border',
            items: [WF.Head, WF.Left, WF.Center]
        });
        window.MainTab = WF.Center;

        WF.Checker = Ext.create('Ext.ng.Checker');
    };

    WF.active = function() {
        if ($user && $user.id != "") {
            //说明已登录
            WF.Login.loginSuccess($user);
            //WF.Login.open();
        } else {
            //WF.Login.open();
            //WF.PopLogin();
        }
    };

    WF.Checker = {};

    WF.ListObserver = Ext.create('Ext.util.MixedCollection');
        
    WF.NotCompleteRequest = [];

})(MainFrame);