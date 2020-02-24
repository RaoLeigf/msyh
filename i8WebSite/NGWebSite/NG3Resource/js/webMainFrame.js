var MainFrame = {};
var OpenFunctionWeb;
var createFrame;
var alertView;
var PageUrl = window.location.href;
var myTitle = "首页";
var myIndexUrl = "GXM/XM/DefinedDesk/Index";
if (PageUrl.indexOf("GHExpense") > 0) {
    myTitle = "项目支出预算审批表";
    myIndexUrl = "GYS/YS/ExpenseMst/ExpenseMstList";
}
if (PageUrl.indexOf("PerformanceMst") > 0) {
    myTitle = "绩效评价";
    myIndexUrl = "GJX/JX/PerformanceMst/PerformanceMstList";
}

(function (WF) {
    sessionStorage.setItem("FYear", "2020");
    WF.config = (function () {
        var pagePath = C_ROOT + 'Main/Frame/',
            imgPath = C_ROOT + 'NG3Resource/pic/web/';
        return {
            pagePath: pagePath,
            //登录处理页面
            loginUrl: C_ROOT + "Main/Login/LoginPart",
            logo: imgPath + 'logo.png',
            leftWidth: 248,
            rightWidth: 200,
            headHeight: 125,
            minHeadHeight: 80,
        };
    })();

    OpenFunctionWeb = function (url, rightkey, title) {
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
                    'opentype': 'myfunction'
                }
            },
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText)
                if (resp.allowopen == "true") {
                    WF.Center.openTab(title, resp.url);
                } else {
                    Ext.Msg.alert("提示", resp.errmsg);
                }
            }
        });
    }

    createFrame = function (src) {
        var portalFrame = document.createElement("IFRAME");
        portalFrame.scrolling = "auto";
        portalFrame.frameBorder = 0;
        portalFrame.src = src;
        portalFrame.height = "100%";
        portalFrame.width = "100%";
        return portalFrame;
    }

    ChangeTab = function (id) {
        if (id == "mainMyDesktop" && !Ext.getCmp("mainMyDesktop")) {
            OpenFunctionWeb(C_ROOT + '/Portal.mvc/MyDesktop', "", "我的桌面");
        } else {
            WF.Center.setActiveTab(id);
        }
    }

    //小叮当
    alertView = WF.AlertView = function (text) {
        var eBody = Ext.getBody();
        //var frame = document.createElement("IFRAME");
        //frame.id = "frame1";
        //frame.frameBorder = 0;
        //frame.src = C_ROOT + 'SUP/MainTree/AlertView';
        //frame.height = "100%";
        //frame.width = "100%";
        win = Ext.create('Ext.window.Window', {
            title: '小铃铛弹窗',
            autoScreen: true,
            iconCls: 'icon-Key',
            closable: true,
            maximizable: false,
            resizable: false,
            //modal: true,
            border: false,
            x: eBody.getWidth() - 370,
            y: eBody.getHeight() - 235,
            width: 369,
            height: 229,
            layout: 'fit',
            html: "<div>" +
                "<div style='width:369px;height:35px;background-color:beige;padding-top:5px'><p style='padding-top:0px;padding-left:240px'id='alertViewCloseTime'></p></div>" +
                //"<hr />"+
                "<div style='width:369px;height:184px;background-color:white;padding-top:5px'><p style='font-size: 18px !important;padding-top:30px;padding-left:20px;'>您收到了新的<<strong style='font-size: 18px !important;'>'" + text + "'</strong>></p></div>" +
                "</div>"
            //flyIn: function () {
            //    var me = win;
            //    me.show();
            //    me.getEl().shift({ x: eBody.getWidth() - me.getWidth() + 20, y: eBody.getHeight() - me.getHeight() + 20 });
            //},
            //flyOut: function () {
            //    if (!this.isVisible()) { return; }
            //    var me = win;
            //    me.getEl().shift({ x: eBody.getWidth() - me.getWidth(), y: eBody.getHeight() - me.getHeight() })
            //}
        }).show();
        alertViewClose(15);
        function alertViewClose(n) {
            document.getElementById("alertViewCloseTime").innerHTML = n + "秒后自动关闭弹窗";
            if (n > 0) {
                setTimeout(function () {
                    alertViewClose(n - 1);
                }, 1000);
            } else {
                win.close();
            }
        }
    }

    //#region HeadToolbar
    WF.HeadToolbar = (function () {
        var title = '预算项目库管理系统';
        //Ext.Ajax.request({
        //    url: C_ROOT + "SUP/Login/GetCustomTitle",
        //    async: false,
        //    success: function (response) {
        //        title = response.text;
        //    }
        //});
        return Ext.create("Ext.toolbar.Toolbar",
            {
                defaults: {
                    width: 55
                },
                items: [
                    title,
                    '->',
                    //{
                    //    iconCls: 'icon-unfold',
                    //    text: '展开',
                    //    width: 55,
                    //    handler: function () {
                    //        var iconObj = document.getElementById('fiveCenter')//五大中心图标
                    //        var logoObj = document.getElementById('logo')//左侧logo

                    //        if (this.iconCls == 'icon-unfold') {
                    //            WF.Head.setHeight(WF.config.headHeight);
                    //            var logoHTML = '<img style="margin-top:12px;" src="' + WF.config.logo + '"/>'
                    //            //图标显示在右侧
                    //            var rightHtml = '<div style="width:440px;height:60px;margin-right:10px;float:right;margin-top:0px;">' +
                    //           '<div onclick=OpenFunctionWeb("' + C_ROOT + '/EPM/MC/EpmMetopeOptions/EpmMetopeShow","","决策中心") class="icon-right"><image ondragstart="return false;" class="image-right" src="../../NG3Resource/pic/web/largeDecideCenter.png"/><div class="title-right">决策中心</div></div>' +
                    //           '<div onclick=OpenFunctionWeb("I6PProjectBIMManager","","项目中心") class="icon-right"><image ondragstart="return false;" class="image-right" src="../../NG3Resource/pic/web/largeProjectCenter.png"/><div class="title-right">项目中心</div></div>' +
                    //           '<div onclick=OpenFunctionWeb("' + C_ROOT + '/WM/Doc/DocumentSearch/DocSearchList?AppTitle=文档中心","","文档中心") class="icon-right"><image ondragstart="return false;" class="image-right" src="../../NG3Resource/pic/web/largeFileCenter.png"/><div class="title-right">文档中心</div></div>' +
                    //           '<div onclick=OpenFunctionWeb("' + C_ROOT + '/WM/KM/KnowlgSearch/KnowlgSearchList?AppTitle=知识中心","","知识中心") class="icon-right"><image ondragstart="return false;" class="image-right" src="../../NG3Resource/pic/web/largeKnowledgeCenter.png"/><div class="title-right">知识中心</div></div>' +
                    //           '<div onclick=OpenFunctionWeb("' + C_ROOT + 'SUP/NavigationCenter/NavigationCenter","","导航中心") class="icon-right"><image ondragstart="return false;" class="image-right" src="../../NG3Resource/pic/web/largeNavigationCenter.png"/><div class="title-right">导航中心</div></div>' +
                    //           '<div onclick=ChangeTab("mainMyDesktop") class="icon-right"><image ondragstart="return false;" class="image-right" src="../../NG3Resource/pic/web/largeMyDesktop.png"/><div class="title-right">我的桌面</div></div>' +
                    //           '<div onclick=ChangeTab("mainHome") class="icon-right"><image ondragstart="return false;" class="image-right" src="../../NG3Resource/pic/web/largeWorkPortal.png"/><div class="title-right">工作首页</div></div>' +
                    //           '</div>';
                    //            if (C_UserType == 'SYSTEM') {
                    //                rightHtml = rightHtml.replace('<div onclick=ChangeTab("mainMyDesktop") class="icon-right"><image ondragstart="return false;" class="image-right" src="../../NG3Resource/pic/web/largeMyDesktop.png"/><div class="title-right">我的桌面</div></div>', '');
                    //            }
                    //            iconObj.innerHTML = rightHtml;
                    //            logoObj.innerHTML = logoHTML;
                    //            this.setIconCls('icon-fold');
                    //            this.setText("收起");
                    //        }
                    //        else {
                    //            WF.Head.setHeight(WF.config.minHeadHeight);
                    //            //图标显示在左侧
                    //            var leftHtml = '<div style="width:640px;height:40px;margin-left:10px;float:left;margin-top:0px;">' +
                    //           '<div onclick=ChangeTab("mainHome") class="icon-left"><image ondragstart="return false;" class="image-left" src="../../NG3Resource/pic/web/largeWorkPortal.png"/><div class="title-left">工作首页</div></div>' +
                    //           '<div onclick=ChangeTab("mainMyDesktop") class="icon-left"><image ondragstart="return false;" class="image-left" src="../../NG3Resource/pic/web/largeMyDesktop.png"/><div class="title-left">我的桌面</div></div>' +
                    //            '<div onclick=OpenFunctionWeb("' + C_ROOT + '/SUP/NavigationCenter/NavigationCenter","","导航中心") class="icon-left"><image ondragstart="return false;" class="image-left" src="../../NG3Resource/pic/web/largeNavigationCenter.png"/><div class="title-left">导航中心</div></div>' +
                    //           '<div onclick=OpenFunctionWeb("' + C_ROOT + '/WM/KM/KnowlgSearch/KnowlgSearchList?AppTitle=知识中心","","知识中心") class="icon-left"><image ondragstart="return false;" class="image-left" src="../../NG3Resource/pic/web/largeKnowledgeCenter.png"/><div class="title-left">知识中心</div></div>' +
                    //           '<div onclick=OpenFunctionWeb("' + C_ROOT + '/WM/Doc/DocumentSearch/DocSearchList?AppTitle=文档中心","","文档中心") class="icon-left"><image ondragstart="return false;" class="image-left" src="../../NG3Resource/pic/web/largeFileCenter.png"/><div class="title-left">文档中心</div></div>' +
                    //           '<div onclick=OpenFunctionWeb("I6PProjectBIMManager","","项目中心") class="icon-left"><image ondragstart="return false;" class="image-left" src="../../NG3Resource/pic/web/largeProjectCenter.png"/><div class="title-left">项目中心</div></div>' +
                    //            '<div onclick=OpenFunctionWeb("' + C_ROOT + '/EPM/MC/EpmMetopeOptions/EpmMetopeShow","","决策中心") class="icon-left"><image ondragstart="return false;" class="image-left" src="../../NG3Resource/pic/web/largeDecideCenter.png"/><div class="title-left">决策中心</div></div>' +
                    //           '</div>';
                    //            if (C_UserType == 'SYSTEM') {
                    //                leftHtml = leftHtml.replace('<div onclick=ChangeTab("mainMyDesktop") class="icon-left"><image ondragstart="return false;" class="image-left" src="../../NG3Resource/pic/web/largeMyDesktop.png"/><div class="title-left">我的桌面</div></div>', '');
                    //            }
                    //            iconObj.innerHTML = '';
                    //            logoObj.innerHTML = leftHtml;//logo区显示五大中心
                    //            this.setIconCls('icon-unfold');
                    //            this.setText("展开");
                    //        }

                    //        WF.viewport.doLayout();
                    //    }
                    //},
                    {
                        text: '设置',
                        width: 52,
                        //xtype: 'ngComboBox',
                        //store:states,
                        handler: function (t, e, eOpts) {
                            e.stopEvent();
                            Ext.create("Ext.menu.Menu", {
                                items: [
                                    {
                                        iconCls: 'icon-Assign',
                                        text: '方案分配',
                                        handler: function () {
                                            WF.Center.openTab('方案分配', C_ROOT + "SUP/SchemeAllocation/SchemeAllocation");
                                        }
                                    },
                                    {
                                        iconCls: 'icon-maddress',
                                        text: '个性化',
                                        handler: function () {
                                            WF.Center.openTab('个性设置', C_ROOT + "SUP/MainTree/IndividualConfig");
                                        }
                                    },
                                    {
                                        iconCls: 'icon-Key',
                                        text: '修改密码',
                                        handler: function () {
                                            WF.ChgPwd();
                                        }
                                    },
                                    {
                                        iconCls: "icon-Note",
                                        text: '关于',
                                        handler: function () {
                                            WF.ProductAbout();
                                        }
                                    }
                                ]
                            }).showAt(e.getXY())
                            //bottomTabPanel.tabMenu.showAt(e.getXY());
                            return false;
                        }
                    },
                    {
                        iconCls: 'icon-Refresh',
                        text: '刷新',
                        width: 70,
                        handler: function () {
                            if (Ext.getCmp("mainHome") != null) {

                                Ext.getCmp("mainHome").contentEl.contentWindow.savePortal();
                                Ext.getCmp("mainHome").contentEl.contentWindow.refreshMainFrame();

                            } else if (Ext.getCmp("mainDefinedDesk") != null) {

                                Ext.getCmp("mainDefinedDesk").contentEl.contentWindow.refreshMainFrame();
                            }
                        }
                    },
                    {
                        iconCls: 'icon-UserGo',
                        text: '注销',
                        handler: function () {
                            if ($SetSupcan) $SetSupcan(false);
                            Ext.Msg.confirm('提示', '确定要注销吗?', function (btn) {
                                if (btn == 'yes') {
                                    if (Ext.getCmp("mainHome") != null) {
                                        if (Ext.getCmp("mainHome").contentEl.contentWindow != null) {
                                            Ext.getCmp("mainHome").contentEl.contentWindow.savePortal();
                                        }
                                    }
                                    //Ext.Ajax.abortAll();
                                    WF.loadMarsk("正在注销, 请稍等...").show();
                                    /*if (Ext.isChrome) {
                                        //window.opener = null;
                                        //window.location.href = "about:blank";
                                        window.close();
                                    }
                                    else {
                                        //window.opener = null;
                                        //window.open(' ', "_self");
                                        window.close();
                                    }*/
                                    Ext.Ajax.request({
                                        //url: C_ROOT + 'web/NG3WebLogin/LoginOut',
                                        url: C_ROOT + 'SUP/Login/LoginOut',
                                        success: function (res, opts) {
                                            location.href = C_ROOT + 'web';
                                        }
                                    });
                                } else {
                                    if ($SetSupcan) $SetSupcan(true);
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
                                    if (Ext.getCmp("mainHome") != null) {
                                        if (Ext.getCmp("mainHome").contentEl.contentWindow != null) {
                                            Ext.getCmp("mainHome").contentEl.contentWindow.savePortal();
                                        }
                                    }
                                    //Ext.Ajax.abortAll();
                                    if (Ext.isChrome) {
                                        //window.opener = null;
                                        //window.location.href = "about:blank";
                                        window.close();
                                    }
                                    else {
                                        //window.opener = null;
                                        //window.open(' ', "_self");
                                        window.close();
                                    }
                                    //Ext.Ajax.request({
                                    //    //url: C_ROOT + 'web/NG3WebLogin/LoginOut',
                                    //    url: C_ROOT + 'SUP/Login/LoginOut',
                                    //    success: function (res, opts) {
                                    //        if (Ext.isChrome) {
                                    //            window.opener = null;
                                    //            window.location.href = "about:blank";
                                    //            window.close();
                                    //        }
                                    //        else {
                                    //            window.opener = null;
                                    //            window.open(' ', "_self");
                                    //            window.close();
                                    //        }
                                    //    }
                                    //});
                                }
                            });
                        }
                    }
                ]
            });
    })();
    //#endregion HeadToolbar
    //快捷功能
    var items = [];
    WF.Shortcuts = (function () {
        //name:菜单名
        //url：菜单地址
        //rightkey：菜单权限
        Ext.Ajax.request({
            url: C_ROOT + "SUP/ShortcutMenu/GetShortcutMenuForWeb",
            async: false,
            params: {
                userid: $appinfo.userID,
            },
            success: function (response) {
                if (response.responseText != "")
                    items = Ext.JSON.decode(response.responseText)
            }
        });

        var FYear = new Date().getFullYear();
        Ext.Ajax.request({
            params: { 'UserCode': $appinfo.userID },
            url: C_ROOT + 'GQT/QT/CorrespondenceSettings2/FindUserYear',
            async: false,
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.Status === "success") {
                    FYear = resp.Msg;
                }
            }
        });
        sessionStorage.setItem("FYear", FYear);

        return Ext.create("Ext.toolbar.Toolbar",
            {
                autoRender: true,
                items: [
                    {
                        xtype: 'toolbar',
                        border: 0,
                        itemId: 'toolbarKJ'
                    },
                    {
                        xtype: 'button',
                        text: '更多',
                        itemId: 'menuMore',
                        width: 70,
                        menu: Ext.create("Ext.menu.Menu", {
                        })
                    },
                    "->",
                    {
                        //text: "年度",
                        text: FYear,
                        width: 60,
                        id: 'FYear',
                        //xtype: 'ngComboBox',
                        //store:states,
                        handler: function (t, e, eOpts) {
                            e.stopEvent();
                            Ext.create("Ext.menu.Menu", {
                                items: [
                                    {
                                        text: '2019',
                                        handler: function () {
                                            sessionStorage.setItem("FYear", "2019");
                                            Ext.getCmp('FYear').setText('2019');
                                            if (Ext.getCmp("mainHome") != null) {

                                                Ext.getCmp("mainHome").contentEl.contentWindow.savePortal();
                                                Ext.getCmp("mainHome").contentEl.contentWindow.refreshMainFrame();

                                            } else if (Ext.getCmp("mainDefinedDesk") != null) {

                                                Ext.getCmp("mainDefinedDesk").contentEl.contentWindow.refreshMainFrame();
                                            }
                                            SaveUserYear("2019");
                                            $NG3Refresh();
                                        }
                                    },
                                    {
                                        text: "2020",
                                        handler: function () {
                                            var fyear = "2020";
                                            sessionStorage.setItem("FYear", fyear);
                                            Ext.getCmp('FYear').setText(fyear);
                                            if (Ext.getCmp("mainHome") != null) {

                                                Ext.getCmp("mainHome").contentEl.contentWindow.savePortal();
                                                Ext.getCmp("mainHome").contentEl.contentWindow.refreshMainFrame();

                                            } else if (Ext.getCmp("mainDefinedDesk") != null) {

                                                Ext.getCmp("mainDefinedDesk").contentEl.contentWindow.refreshMainFrame();
                                            }
                                            SaveUserYear("2020");
                                            $NG3Refresh();
                                        }
                                    },
                                    {
                                        text: "2021",
                                        handler: function () {
                                            var fyear = "2021";
                                            sessionStorage.setItem("FYear", fyear);
                                            Ext.getCmp('FYear').setText(fyear);
                                            if (Ext.getCmp("mainHome") != null) {

                                                Ext.getCmp("mainHome").contentEl.contentWindow.savePortal();
                                                Ext.getCmp("mainHome").contentEl.contentWindow.refreshMainFrame();

                                            } else if (Ext.getCmp("mainDefinedDesk") != null) {

                                                Ext.getCmp("mainDefinedDesk").contentEl.contentWindow.refreshMainFrame();
                                            }
                                            SaveUserYear("2021");
                                            $NG3Refresh();
                                        }
                                    },
                                    {
                                        //iconCls: 'icon-Assign',
                                        text: "2022",
                                        handler: function () {
                                            var fyear = "2022";
                                            sessionStorage.setItem("FYear", fyear);
                                            Ext.getCmp('FYear').setText(fyear);
                                            if (Ext.getCmp("mainHome") != null) {

                                                Ext.getCmp("mainHome").contentEl.contentWindow.savePortal();
                                                Ext.getCmp("mainHome").contentEl.contentWindow.refreshMainFrame();

                                            } else if (Ext.getCmp("mainDefinedDesk") != null) {

                                                Ext.getCmp("mainDefinedDesk").contentEl.contentWindow.refreshMainFrame();
                                            }
                                            SaveUserYear("2022");
                                            $NG3Refresh();
                                        }
                                    },
                                    {
                                        text: "2023",
                                        handler: function () {
                                            var fyear = "2023";
                                            sessionStorage.setItem("FYear", fyear);
                                            Ext.getCmp('FYear').setText(fyear);
                                            if (Ext.getCmp("mainHome") != null) {

                                                Ext.getCmp("mainHome").contentEl.contentWindow.savePortal();
                                                Ext.getCmp("mainHome").contentEl.contentWindow.refreshMainFrame();

                                            } else if (Ext.getCmp("mainDefinedDesk") != null) {

                                                Ext.getCmp("mainDefinedDesk").contentEl.contentWindow.refreshMainFrame();
                                            }
                                            SaveUserYear("2023");
                                            $NG3Refresh();
                                        }
                                    },
                                    {
                                        text: "2024",
                                        handler: function () {
                                            var fyear = "2024";
                                            sessionStorage.setItem("FYear", fyear);
                                            Ext.getCmp('FYear').setText(fyear);
                                            if (Ext.getCmp("mainHome") != null) {

                                                Ext.getCmp("mainHome").contentEl.contentWindow.savePortal();
                                                Ext.getCmp("mainHome").contentEl.contentWindow.refreshMainFrame();

                                            } else if (Ext.getCmp("mainDefinedDesk") != null) {

                                                Ext.getCmp("mainDefinedDesk").contentEl.contentWindow.refreshMainFrame();
                                            }
                                            SaveUserYear("2024");
                                            $NG3Refresh();
                                        }
                                    },
                                    {
                                        text: "2025",
                                        handler: function () {
                                            var fyear = "2025";
                                            sessionStorage.setItem("FYear", fyear);
                                            Ext.getCmp('FYear').setText(fyear);
                                            if (Ext.getCmp("mainHome") != null) {

                                                Ext.getCmp("mainHome").contentEl.contentWindow.savePortal();
                                                Ext.getCmp("mainHome").contentEl.contentWindow.refreshMainFrame();

                                            } else if (Ext.getCmp("mainDefinedDesk") != null) {

                                                Ext.getCmp("mainDefinedDesk").contentEl.contentWindow.refreshMainFrame();
                                            }
                                            SaveUserYear("2025");
                                            $NG3Refresh();
                                        }
                                    },
                                    {
                                        text: "2026",
                                        handler: function () {
                                            var fyear = "2026";
                                            sessionStorage.setItem("FYear", fyear);
                                            Ext.getCmp('FYear').setText(fyear);
                                            if (Ext.getCmp("mainHome") != null) {

                                                Ext.getCmp("mainHome").contentEl.contentWindow.savePortal();
                                                Ext.getCmp("mainHome").contentEl.contentWindow.refreshMainFrame();

                                            } else if (Ext.getCmp("mainDefinedDesk") != null) {

                                                Ext.getCmp("mainDefinedDesk").contentEl.contentWindow.refreshMainFrame();
                                            }
                                            SaveUserYear("2026");
                                            $NG3Refresh();
                                        }
                                    },
                                    {
                                        text: "2027",
                                        handler: function () {
                                            var fyear = "2027";
                                            sessionStorage.setItem("FYear", fyear);
                                            Ext.getCmp('FYear').setText(fyear);
                                            if (Ext.getCmp("mainHome") != null) {

                                                Ext.getCmp("mainHome").contentEl.contentWindow.savePortal();
                                                Ext.getCmp("mainHome").contentEl.contentWindow.refreshMainFrame();

                                            } else if (Ext.getCmp("mainDefinedDesk") != null) {

                                                Ext.getCmp("mainDefinedDesk").contentEl.contentWindow.refreshMainFrame();
                                            }
                                            SaveUserYear("2027");
                                            $NG3Refresh();
                                        }
                                    },
                                    {
                                        text: "2028",
                                        handler: function () {
                                            var fyear = "2028";
                                            sessionStorage.setItem("FYear", fyear);
                                            Ext.getCmp('FYear').setText(fyear);
                                            if (Ext.getCmp("mainHome") != null) {

                                                Ext.getCmp("mainHome").contentEl.contentWindow.savePortal();
                                                Ext.getCmp("mainHome").contentEl.contentWindow.refreshMainFrame();

                                            } else if (Ext.getCmp("mainDefinedDesk") != null) {

                                                Ext.getCmp("mainDefinedDesk").contentEl.contentWindow.refreshMainFrame();
                                            }
                                            SaveUserYear("2028");
                                            $NG3Refresh();
                                        }
                                    },
                                    {
                                        text: "2029",
                                        handler: function () {
                                            var fyear = "2029";
                                            sessionStorage.setItem("FYear", fyear);
                                            Ext.getCmp('FYear').setText(fyear);
                                            if (Ext.getCmp("mainHome") != null) {

                                                Ext.getCmp("mainHome").contentEl.contentWindow.savePortal();
                                                Ext.getCmp("mainHome").contentEl.contentWindow.refreshMainFrame();

                                            } else if (Ext.getCmp("mainDefinedDesk") != null) {

                                                Ext.getCmp("mainDefinedDesk").contentEl.contentWindow.refreshMainFrame();
                                            }
                                            SaveUserYear("2029");
                                            $NG3Refresh();
                                        }
                                    }
                                ]
                            }).showAt(e.getXY())
                            //bottomTabPanel.tabMenu.showAt(e.getXY());
                            return false;
                        }
                    },
                    {
                        text: '设置',
                        width: 52,
                        //xtype: 'ngComboBox',
                        //store:states,
                        handler: function (t, e, eOpts) {
                            e.stopEvent();
                            Ext.create("Ext.menu.Menu", {
                                items: [
                                    {
                                        iconCls: 'icon-Assign',
                                        text: '方案分配',
                                        handler: function () {
                                            WF.Center.openTab('方案分配', C_ROOT + "SUP/SchemeAllocation/SchemeAllocation");
                                        }
                                    },
                                    {
                                        iconCls: 'icon-maddress',
                                        text: '个性化',
                                        handler: function () {
                                            WF.Center.openTab('个性设置', C_ROOT + "SUP/MainTree/IndividualConfig");
                                        }
                                    },
                                    {
                                        iconCls: 'icon-Key',
                                        text: '修改密码',
                                        handler: function () {
                                            WF.ChgPwd();
                                        }
                                    },
                                    {
                                        iconCls: "icon-Note",
                                        text: '关于',
                                        handler: function () {
                                            WF.ProductAbout();
                                        }
                                    }
                                ]
                            }).showAt(e.getXY())
                            //bottomTabPanel.tabMenu.showAt(e.getXY());
                            return false;
                        }
                    },
                    {
                        iconCls: 'icon-Refresh',
                        text: '刷新',
                        width: 70,
                        handler: function () {
                            if (Ext.getCmp("mainHome") != null) {

                                Ext.getCmp("mainHome").contentEl.contentWindow.savePortal();
                                Ext.getCmp("mainHome").contentEl.contentWindow.refreshMainFrame();

                            } else if (Ext.getCmp("mainDefinedDesk") != null) {

                                Ext.getCmp("mainDefinedDesk").contentEl.contentWindow.refreshMainFrame();
                            }
                        }
                    },
                    {
                        iconCls: 'icon-UserGo',
                        text: '注销',
                        handler: function () {
                            if ($SetSupcan) $SetSupcan(false);
                            Ext.Msg.confirm('提示', '确定要注销吗?', function (btn) {
                                if (btn == 'yes') {
                                    if (Ext.getCmp("mainHome") != null) {
                                        if (Ext.getCmp("mainHome").contentEl.contentWindow != null) {
                                            Ext.getCmp("mainHome").contentEl.contentWindow.savePortal();
                                        }
                                    }
                                    //Ext.Ajax.abortAll();
                                    WF.loadMarsk("正在注销, 请稍等...").show();
                                    /*if (Ext.isChrome) {
                                        //window.opener = null;
                                        //window.location.href = "about:blank";
                                        window.close();
                                    }
                                    else {
                                        //window.opener = null;
                                        //window.open(' ', "_self");
                                        window.close();
                                    }*/
                                    Ext.Ajax.request({
                                        //url: C_ROOT + 'web/NG3WebLogin/LoginOut',
                                        url: C_ROOT + 'SUP/Login/LoginOut',
                                        success: function (res, opts) {
                                            location.href = C_ROOT + 'web';
                                        }
                                    });
                                } else {
                                    if ($SetSupcan) $SetSupcan(true);
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
                                    if (Ext.getCmp("mainHome") != null) {
                                        if (Ext.getCmp("mainHome").contentEl.contentWindow != null) {
                                            Ext.getCmp("mainHome").contentEl.contentWindow.savePortal();
                                        }
                                    }
                                    //Ext.Ajax.abortAll();
                                    if (Ext.isChrome) {
                                        //window.opener = null;
                                        //window.location.href = "about:blank";
                                        window.close();
                                    }
                                    else {
                                        //window.opener = null;
                                        //window.open(' ', "_self");
                                        window.close();
                                    }
                                    //Ext.Ajax.request({
                                    //    //url: C_ROOT + 'web/NG3WebLogin/LoginOut',
                                    //    url: C_ROOT + 'SUP/Login/LoginOut',
                                    //    success: function (res, opts) {
                                    //        if (Ext.isChrome) {
                                    //            window.opener = null;
                                    //            window.location.href = "about:blank";
                                    //            window.close();
                                    //        }
                                    //        else {
                                    //            window.opener = null;
                                    //            window.open(' ', "_self");
                                    //            window.close();
                                    //        }
                                    //    }
                                    //});
                                }
                            });
                        }
                    }
                    //{
                    //    xtype: 'combo',
                    //    width: 90,
                    //    editable: false,
                    //    itemId: 'comboSearchCode',
                    //    store: Ext.create('Ext.data.Store', {
                    //        fields: ['val', 'text'],
                    //        data: [
                    //            { "val": "WM3DOC", "text": "文档" },
                    //            { "val": "WM3KMLG", "text": "维基百科" },
                    //            { "val": "WM3FILE", "text": "档案" },
                    //            { "val": "WM3WiKi", "text": "维基百科" }
                    //        ]
                    //    }),
                    //    queryMode: 'local',
                    //    value: 'WM3DOC',
                    //    displayField: 'text',
                    //    valueField: 'val',
                    //    align: 'center',
                    //    tpl: Ext.create('Ext.XTemplate',
                    //     '<tpl for=".">',
                    //         '<div class="x-boundlist-item" style="text-align:center">{text}</div>',
                    //     '</tpl>'
                    //     ),
                    //    listeners: {
                    //        render: function () {
                    //            var me = this;
                    //            Ext.get(me.id + "-inputEl").dom.style.textAlign = "right";
                    //        }
                    //    }
                    //}, {
                    //    xtype: 'textfield',
                    //    width: 110,
                    //    itemId: 'comboSearchText'
                    //}, {
                    //    xtype: 'button',
                    //    iconCls: 'icon-Query',
                    //    handler: function () {
                    //        var sort = WF.Shortcuts.queryById("comboSearchCode").getValue();
                    //        var txt = WF.Shortcuts.queryById("comboSearchText").getValue();
                    //        WF.Center.openTab("万向搜索", C_ROOT + "NGSearch/Search/searchresult?wd=" + txt + "&sort=" + sort);
                    //    }
                    //}
                ]
            });
    })();


    function SaveUserYear(Year) {
        Ext.Ajax.request({
            params: { 'UserCode': $appinfo.userID, 'FYear': Year},
            url: C_ROOT + 'GQT/QT/CorrespondenceSettings2/SaveUserYear',
           // async: false,
            success: function (response) {
                //var resp = Ext.JSON.decode(response.responseText);
                //if (resp.Status === "success") {
                //    FYear = resp.Msg;
                //}
            }
        });
    }


    WF.Shortcuts.Refrech = function (init) {
        var ShortcutsRefrechItems = function (items) {
            var fontWidth = 14;
            Ext.Array.each(WF.Shortcuts.queryById("toolbarKJ").items.items, function (item) {
                item.hide();
            });
            WF.Shortcuts.queryById("toolbarKJ").items.clear();
            Ext.Array.each(WF.Shortcuts.queryById("menuMore").menu.items.items, function (item) {
                item.hide();
            });
            WF.Shortcuts.queryById("menuMore").menu.items.clear();
            //for (var i = 0; i < WF.Shortcuts.queryById("menuMore").menu.items.length; i++) {
            //    WF.Shortcuts.queryById("menuMore").menu.items[i].text = '';
            //}
            /*WF.Shortcuts.queryById("toolbarKJ").add(Ext.create("Ext.button.Button", {
                text: '功能导航:',//快捷功能
                handler: function () {
                    WF.Center.openTab("功能导航", C_ROOT + "/SUP/ShortcutMenu/ShortcutMenu?isweb=true");
                }
            }));*/
            var PageUrl = window.location.href;
            if (PageUrl.indexOf('GHExpense=true') >= 0) {
                WF.Shortcuts.queryById("toolbarKJ").add(Ext.create("Ext.button.Button", {
                    text: '功能导航:',//快捷功能
                    handler: function () {
                        WF.Center.openTab("功能导航", C_ROOT + "/SUP/ShortcutMenu/ShortcutMenu?isweb=true");
                    }
                }));
                WF.Shortcuts.queryById("toolbarKJ").add(Ext.create("Ext.button.Button", {
                    text: '待我审批:0',
                    id: 'ykjh_SP1254',
                    handler: function () {
                        $OpenTab('我的工作任务', C_ROOT + '/GYS/YS/ExpenseMst/WorkFlowTaskList_toExpense');
                    }
                }));
                Ext.Ajax.request({
                    url: C_ROOT + 'WorkFlow3/FlowManager/GetPendingTaskByUser',
                    type: 'POST',
                    params: {
                        page: 0,
                        start: 0,
                        limit: 1,
                        ng3_logid: $appinfo.userID,
                        random: Math.random(),
                        queryfilter: '{ "act_ru_task.FORM_RESOURCE_KEY_*str*like*1": "GHExpense" }'
                    },
                    async: true,
                    success: function (response) {
                        if (response != null && response != undefined) {
                            var data = Ext.JSON.decode(response.responseText);
                            Ext.getCmp('ykjh_SP1254').setText('待我审批:' + data.totalRows);
                        }
                    }
                });
                WF.Shortcuts.queryById("toolbarKJ").add(Ext.create("Ext.button.Button", {
                    text: '项目支出预算情况查询',
                    handler: function () {
                        WF.Center.openTab("项目支出预算情况查询", C_ROOT + "/GYS/YS/ExpenseMst/ZcYs");
                    }
                }));
            }
            var width = window.innerWidth - 450;
            var usedWidth = fontWidth * 4;
            var itemsWith = 0;
            for (var i = 0; i < items.length; i++) {
                itemsWith += fontWidth * items[i].name.length + 30;
            }
            //控制更多选项的显示与隐藏
            WF.Shortcuts.queryById("menuMore").hidden = true;
            /*if ((usedWidth + itemsWith) > width)
                WF.Shortcuts.queryById("menuMore").hidden = false;
            else
                WF.Shortcuts.queryById("menuMore").hidden = true;
            Ext.Array.each(items, function (item) {
                if ((usedWidth + fontWidth * item.name.length + 30) > width) {
                    //WF.Shortcuts.queryById("menuMore").menu.add(Ext.create("Ext.button.Button", {
                    //    text: item.name,
                    //    hidden:false,
                    //    handler: function () {
                    //        OpenFunctionWeb(C_ROOT + item.url, item.rightkey, item.name);
                    //    }
                    //}));
                    //修改原有创建button为menu菜单的item
                    WF.Shortcuts.queryById("menuMore").menu.items.add(Ext.create("Ext.menu.Item", {
                        text: item.name,
                        draggable: false,//禁止菜单拖动
                        handler: function () {
                            OpenFunctionWeb(C_ROOT + item.url, item.rightkey, item.name);
                        }
                    }));
                }
                else {
                    WF.Shortcuts.queryById("toolbarKJ").add(Ext.create("Ext.button.Button", {
                        text: item.name,
                        handler: function () {
                            OpenFunctionWeb(C_ROOT + item.url, item.rightkey, item.name);
                        }
                    }));
                    WF.Shortcuts.queryById("toolbarKJ").add(Ext.create("Ext.toolbar.Separator", {}));
                    usedWidth += fontWidth * item.name.length + 30;//30为toolbar之间的分隔符加上间距

                }
            });*/
        }
        ShortcutsRefrechItems(items);

    }
    WF.Shortcuts.Refrech(true);


    //#region Head
    WF.Head = (function () {
        var leftHtml = '<div style="width:50%;height:42px;margin-right: 50px;float:right;margin-top:0px;">' +
            //'<div onclick=OpenFunctionWeb("' + C_ROOT + '/WM/Doc/DocumentSearch/DocSearchList?AppTitle=文档中心","","文档中心") class="icon-left"><image ondragstart="return false;" class="image-left" src="../../NG3Resource/pic/web/largeFileCenter.png"/><div class="title-left">文档中心</div></div>' +
            //'<div onclick=ChangeTab("mainMyDesktop") class="icon-left"><image ondragstart="return false;" class="image-left" src="../../NG3Resource/pic/web/largeMyDesktop.png"/><div class="title-left">我的桌面</div></div>' +
            //'<div onclick=ChangeTab("mainDefinedDesk") class="icon-left"><image ondragstart="return false;" class="image-left" src="../../NG3Resource/pic/web/largeWorkPortal.png"/><div class="title-left">工作首页</div></div>' +
            //'<div onclick=OpenFunctionWeb("' + C_ROOT + '/SUP/NavigationCenter/NavigationCenter","","导航中心") class="icon-left"><image ondragstart="return false;" class="image-left" src="../../NG3Resource/pic/web/largeNavigationCenter.png"/><div class="title-left">导航中心</div></div>' +
            //'<div onclick=OpenFunctionWeb("' + C_ROOT + '/WM/KM/KnowlgSearch/KnowlgSearchList?AppTitle=知识中心","","知识中心") class="icon-left"><image ondragstart="return false;" class="image-left" src="../../NG3Resource/pic/web/largeKnowledgeCenter.png"/><div class="title-left">知识中心</div></div>' +
            //'<div onclick=OpenFunctionWeb("I6PProjectBIMManager","","项目中心") class="icon-left"><image ondragstart="return false;" class="image-left" src="../../NG3Resource/pic/web/largeProjectCenter.png"/><div class="title-left">项目中心</div></div>' +
            //'<div onclick=OpenFunctionWeb("' + C_ROOT + '/EPM/MC/EpmMetopeOptions/EpmMetopeShow","","决策中心") class="icon-left"><image ondragstart="return false;" class="image-left" src="../../NG3Resource/pic/web/largeDecideCenter.png"/><div class="title-left">决策中心</div></div>' +
            '</div><div style="width:30%;height:42px;margin-left: 20px;float:left;margin-top:0px;"><img src="../../NG3Resource/Desk/img/login_1.png" style="width:192px;height:40px;"/></div>';
        //if (C_UserType == 'SYSTEM') {
        //    leftHtml = leftHtml.replace('<div onclick=ChangeTab("mainMyDesktop") class="icon-left"><image ondragstart="return false;" class="image-left" src="../../NG3Resource/pic/web/largeMyDesktop.png"/><div class="title-left">我的桌面</div></div>', '');
        //}

        var logo = new Ext.form.Label({
            width: window.screen.width,
            style: {
                right: '10px'
            },
            html: '<div id="logo">' + leftHtml + '</div>'
        });
        var apps = new Ext.Container({
            flex: 1,
            html: ''
        });
        var himg = new Ext.Container({
            width: 480,
            height: 60,
            margin: '2 10 0 10',
            id: "fiveCenter",
            html: '<div id="fiveCenter"></div>'
            //'<img align=right src="' + WF.config.image1 + '"/>'
            //    + '<img align=right src="' + WF.config.image2 + '"/>'

        });
        return new Ext.Panel({
            region: 'north',
            //layout: 'hbox',
            //style: 'background-color:white',
            border: false,
            height: WF.config.minHeadHeight,
            items: [logo],
            //tbar: WF.HeadToolbar,
            fbar: WF.Shortcuts,

        });
    })();
    //#endregion Head

    //#region Left
    WF.Left = (function () {

        ////使用系统功能树
        //var tree = Ext.create("Ext.ng.sysFuncTree", {
        //    title: '系统功能菜单',
        //    iconCls: 'icon-occupy-mode',
        //    hasRightClickMenu: true,
        //    hasDbClickListener: true
        //});
        ////使用企业功能
        //var enterpriseTree = Ext.create("Ext.ng.enFuncTree", {
        //    title: '企业功能菜单',
        //    iconCls: 'icon-occupy-mode',
        //    hasRightClickMenu: true
        //});
        ////组织
        //var org = Ext.create("Ext.ng.OrgGuideTree", {
        //    title: '组织',
        //    iconCls: 'icon-occupy-mode'
        //});
        var accordion = Ext.create('Ext.panel.Panel', {
            collapsible: true,
            collapsed: true,
            title: '导航面板',
            region: 'west',
            margins: '5 0 0 5',
            layout: 'fit',
            //split: true,
            width: WF.config.leftWidth,
            border: 0,
            //items: [enterpriseTree, tree, org]
            items: [{
                id: 'MainFrame',
                contentEl: createFrame(C_ROOT + "/SUP/MainTree/MainFrameView")
            }]
        });

        return accordion;
    })();
    //#endregion Left

    //Ext.ux.TabCloseMenu = function () {
    //    var tabs, menu, ctxItem;
    //    this.init = function (tp) {
    //        tabs = tp;
    //        tabs.on('contextmenu', onContextMenu);
    //    }

    //    function onContextMenu(ts, item, e) {
    //        if (!menu) {
    //            menu = new Ext.menu.Menu([{
    //                id: tabs.id + '-close',
    //                text: '关闭当前标签页',
    //                iconCls: 'btnno',
    //                handler: function () {
    //                    tabs.remove(ctxItem);
    //                }
    //            },
    //            {
    //                id: tabs.id + '-close-others',
    //                text: '关闭其他标签页',
    //                iconCls: 'btnno',
    //                handler: function () {
    //                    tabs.items.each(function (item) {
    //                        if (item.closetab && item != ctxItem) {
    //                            tabs.remove(item);
    //                        }
    //                    })
    //                }
    //            }, {
    //                id: tabs.id + '-close-all',
    //                text: '关闭所有标签页',
    //                iconCls: 'btnno',
    //                handler: function () {
    //                    tabs.items.each(function (item) {
    //                        if (item.closetab) {
    //                            tabs.remove(item);
    //                        }
    //                    })
    //                }
    //            }])
    //        }
    //        ctxItem = item;
    //        var items = menu.items;
    //        items.get(tabs.id + '-close').setDisabled(!item.closable);

    //        var disableOthers = true;
    //        tabs.items.each(function () {
    //            if (this != item && this.closable) {
    //                disableOthers = false;
    //                return false;
    //            }
    //        });
    //        items.get(tabs.id + '-close-others').setDisabled(disableOthers);
    //        menu.showAt(e.getPoint());

    //        var diableAll = true;
    //        tabs.items.each(function () {
    //            if (this.closable) {
    //                disableOthers = false;
    //                return false;
    //            }
    //        });
    //        items.get(tabs.id + '-close-all').setDisabled(diableAll);
    //        menu.showAt(e.getPoint());
    //    }
    //};

    if (C_UserType != 'SYSTEM') {
        WF.Center = new Ext.TabPanel({
            region: 'center',
            activeTab: 2,
            enableTabScroll: true,
            //plugins: new Ext.ux.TabCloseMenu(),
            items: [
                //{
                //    id: 'mainHome',
                //    title: "工作首页",
                //    closable: false,
                //    contentEl: createFrame(C_ROOT + "Portal.mvc/Mainframe?isweb=true")
                //},
                {
                    id: 'mainDefinedDesk',
                    title: myTitle,
                    closable: false,
                    contentEl: createFrame(C_ROOT + myIndexUrl)
                    //contentEl: createFrame(C_ROOT + "GXM/XM/DefinedDesk/Index")
                }
                //{
                //    id: 'mainMyDesktop',
                //    title: "我的桌面",
                //    closable: true,
                //    contentEl: createFrame(C_ROOT + "Portal.mvc/MyDesktop")
                //}
            ],
            listeners: {
                tabchange: function (me, tab) {
                    if (Ext.isIE6) {
                        try {
                            if (tab.body.dom.firstChild)
                                setTimeout(tab.body.dom.firstChild.contentWindow.onresize(), 200);
                        }
                        catch (e) { }
                    }

                    //处理金格置顶
                    var tabId = tab.myTabId;
                    var regObj = WF.KG;
                    for (var key in regObj) {
                        var currentObj = regObj[key];
                        if (tabId === key) {
                            if (currentObj['showFlg']) {
                                if (currentObj['kgObj']) {
                                    currentObj['kgObj'].HidePlugin(1);
                                }
                            }
                        }
                        else {
                            if (currentObj['kgObj']) {
                                currentObj['kgObj'].HidePlugin(0);
                            }
                        }
                    }
                },
                beforeremove: beforeRemoveTab,
            },
            plugins: new Ext.create('Ext.ux.TabCloseMenu', {
                closeTabText: '关闭当前标签页',
                closeOthersTabsText: '关闭其它标签页',
                closeAllTabsText: '关闭所有标签页'
            })
        });
    } else {
        WF.Center = new Ext.TabPanel({
            region: 'center',
            activeTab: 0,
            enableTabScroll: true,
            //plugins: new Ext.ux.TabCloseMenu(),
            items: [
                //    {
                //    id: 'mainHome',
                //    title: "工作首页",
                //    closable: false,
                //    contentEl: createFrame(C_ROOT + "Portal.mvc/Mainframe?isweb=true")
                //}
                {
                    id: 'mainDefinedDesk',
                    title: myTitle,
                    closable: false,
                    contentEl: createFrame(C_ROOT + myIndexUrl)
                    //contentEl: createFrame(C_ROOT + "GXM/XM/DefinedDesk/Index")
                }
            ],
            listeners: {
                tabchange: function (me, tab) {
                    if (Ext.isIE6) {
                        try {
                            if (tab.body.dom.firstChild)
                                setTimeout(tab.body.dom.firstChild.contentWindow.onresize(), 200);
                        }
                        catch (e) { }
                    }

                    //处理金格置顶
                    var tabId = tab.myTabId;
                    var regObj = WF.KG;
                    for (var key in regObj) {
                        var currentObj = regObj[key];
                        if (tabId === key) {
                            if (currentObj['showFlg']) {
                                if (currentObj['kgObj']) {
                                    currentObj['kgObj'].HidePlugin(1);
                                }
                            }
                        }
                        else {
                            if (currentObj['kgObj']) {
                                currentObj['kgObj'].HidePlugin(0);
                            }
                        }
                    }
                },
                beforeremove: beforeRemoveTab,
            },
            plugins: new Ext.create('Ext.ux.TabCloseMenu', {
                closeTabText: '关闭当前标签页',
                closeOthersTabsText: '关闭其它标签页',
                closeAllTabsText: '关闭所有标签页'
            })
        });
    }

    function beforeRemoveTab(tab, panel) {
        //debugger;    

        if (WF.KG[panel.myTabId]) {
            delete WF.KG[panel.myTabId];
        }
        if (WF.SUPCAN[panel.myTabId]) {
            delete WF.SUPCAN[panel.myTabId];
        }

        WF.Checker.fireEvent('checkin', WF.Checker); //触发checkin事件     

        //IE的iframe内存泄漏解决
        panel.contentEl.onload = null;
        panel.contentEl.onreadystatechange = null;
        panel.contentEl.src = 'about:blank';
        delete panel.contentEl;
        if (Ext.isIE) { CollectGarbage(); }

        //Ext.MessageBox.show({
        //    title:"操作确认",
        //    msg:"您确定要关闭<b>[" + panel.title +']</b>么？',
        //    buttons: Ext.MessageBox.YESNO,
        //    icon:Ext.MessageBox.QUESTION,
        //    fn:function(btn,text){
        //        if(btn == "yes")
        //        {                              
        //            tab.un('beforeremove',beforeRemoveTab);
        //            WF.Checker.fireEvent('checkin', WF.Checker);//触发checkin事件
        //            tab.remove(panel);
        //            tab.addListener('beforeremove',beforeRemoveTab,tab);
        //        }
        //    }
        //});

        //return false;      
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

    WF.Center.openTab = function (title, url, params, allowedClose) {
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

                var frame = document.createElement("IFRAME");
                frame.scrolling = "auto";
                frame.frameBorder = 0;
                if (url.indexOf(C_ROOT) >= 0 || url.toLowerCase().indexOf("http://") >= 0)
                    frame.src = url;
                else
                    frame.src = C_ROOT + url;
                //frame.src = C_ROOT + (url.indexOf('/') == 0 ? url.substr(1, url.length) : url);
                frame.height = "100%";
                frame.width = "100%";

                var tabPage = Ext.create('Ext.panel.Panel', {
                    'id': autoId,
                    'title': title,
                    hideMode: 'offsets',//supcan控件置顶，tab页切换会自动隐藏
                    closable: closeable,  //通过html载入目标页
                    contentEl: frame
                    //html: '<iframe scrolling="auto" frameborder="0" width="100%" height="100%" src="' + url + '"></iframe>'
                });

                frame.parentContainer = tabPage; //父容器
                tabPage.myTabId = autoId;
                n = me.add(tabPage);
            }
            me.setActiveTab(autoId)

            //n.body.dom.childNodes[0].contentWindow.lastArguments = params || {};
        }
    };
    //#endregion

    //默认打开功能
    WF.DefaultOpenTab = (function () {
        var items = [];
        //name:菜单名
        //url：菜单地址
        //rightkey：菜单权限
        Ext.Ajax.request({
            url: C_ROOT + "SUP/IndividualSetting/LoadDefaultOpenTabForMainFrame",
            async: false,
            success: function (response) {
                items = Ext.JSON.decode(response.responseText).Record;
            }
        });

        var LoadDefaultOpenTab = function (items) {
            Ext.Array.each(items, function (item) {
                OpenFunctionWeb(item.url, item.rightkey, item.name)
            });
        }
        WF.Center.on('afterrender', function () {
            LoadDefaultOpenTab(items);
        })

    })();

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
        region: 'south',
        bbar: Ext.create("Ext.toolbar.Toolbar",
            {
                defaults: {
                    //width: 55,
                    xtype: 'text',
                },
                items: [
                    {
                        itemId: 'TaskbarContent',
                        xtype: 'text',
                        text: Ext.String.format("组织[{0}|{1}],用户[{2}|{3}],帐套[{4}|{5}],项目[{6}]", C_OCode, C_OrgName, C_LoginID, C_LoginName, C_DataBase, C_DataBaseName, C_Project),
                    },
                    "->", {
                        xtype: 'combo',
                        width: 80,
                        editable: false,
                        emptyText: '视图',
                        store: Ext.create('Ext.data.Store', {
                            fields: ['val', 'text', "img"],
                            data: [
                                { "val": "1", "text": "100", "img": 'PortalViewStyle.One.png' },
                                { "val": "2", "text": "6:4", "img": 'PortalViewStyle.TwoLeft.png' },
                                { "val": "3", "text": "4:6", "img": 'PortalViewStyle.TwoRight.png' },
                                { "val": "4", "text": "3:4:3", "img": 'PortalViewStyle.Three.png' },
                                { "val": "5", "text": "3:3:3", "img": 'PortalViewStyle.Three.png' }
                            ]
                        }),
                        queryMode: 'local',
                        displayField: 'text',
                        valueField: 'val',
                        align: 'center',
                        tpl: Ext.create('Ext.XTemplate',
                            '<tpl for=".">',
                            '<div class="x-boundlist-item"><img src="' + C_ROOT + 'NG3Resource/pic/web/{img}" style="margin:0px 0px -4px"/><span style="margin:0px 5px 0px"  >{text}</span></div>',
                            '</tpl>'
                        ),
                        listeners: {
                            change: function (me, newValue, oldValue) {
                                Ext.getCmp("mainHome").contentEl.src = C_ROOT + "Portal.mvc/Mainframe?isweb=true&portalviewstyle=" + newValue;
                            }
                        }
                    }
                ]
            }),
        enableOverflow: true,
        border: false
    });
    //#endregion Taskbar


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
        buttons: [{ text: '登录', iconCls: 'icon-LockGo' }]
    });

    WF.Login.loginSuccess = function (json) {
        this.hide();
    };

    WF.PopLogin = function (lastRequestOptions) {

        var frame = document.createElement("IFRAME");
        frame.id = "frame1";
        frame.frameBorder = 0;
        //frame.src = C_ROOT + 'web/Home/PopLogin';
        frame.src = C_ROOT + 'SUP/Login/PopLogin';
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
            height: 180,
            layout: 'fit',
            contentEl: frame
            //items: [querypanel, grid]
        }).show();

        frame.parentContainer = win; //弹出窗口控件传给iframe      
        frame.lastRequestOptions = lastRequestOptions; //未完成的请求
    }
    //#endregion Login


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
    WF.InitialComet = function () {
        var me = this;
        Ext.Ajax.request({
            url: C_ROOT + 'Main/Frame/InitializeComet',
            success: function (res, opts) {
                var msg = Ext.decode(res.responseText);
                if (msg.rettype == 'true') {
                    me.Comet.privateToken = msg.msg;
                    me.RegisterComet();
                } else {
                    //出现异常，推送失败!
                }
            },
            failure: function (resp, opts) {
                //出现异常，推送失败!
            }
        });
    };

    WF.RegisterComet = function () {
        var me = this;
        Ext.Ajax.request({
            url: C_ROOT + 'CometAsync/DefaultChannel.ashx',
            params: { privateToken: me.Comet.privateToken, lastMessageId: me.Comet.msgId },
            success: function (res, opts) {
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
                        } catch (e) {
                            me.Comet[msg[0].Content.billtype] = undefined;
                        }

                        me.RegisterComet();
                        break;
                }
            },
            failure: function () {

            }
        });

    };
    //#endregion Comet


    WF.init = function () {
        WF.viewport = new Ext.Viewport({
            layout: 'border',
            items: [WF.Head, WF.Left, WF.Center, WF.Taskbar]
        });
        window.MainTab = WF.Center;

        WF.Checker = Ext.create('Ext.ng.Checker');
    };

    WF.active = function () {
        if ($user && $user.id != "") {
            //说明已登录
            WF.Login.loginSuccess($user);
            //WF.Login.open();
        } else {
            //WF.Login.open();
            //WF.PopLogin();
        }
    };

    var pwdWin;
    //修改密码
    WF.ChgPwd = function () {
        var frame = document.createElement("IFRAME");
        frame.id = "frame1";
        frame.frameBorder = 0;
        frame.src = C_ROOT + 'SUP/ChangePwd?isweb=true';
        frame.height = "100%";
        frame.width = "100%";

        pwdWin = Ext.create('Ext.window.Window', {
            title: '修改密码',
            autoScreen: true,
            iconCls: 'icon-Key',
            closable: true,
            maximizable: false,
            resizable: false,
            modal: true,
            border: false,
            width: 700,
            height: 250,
            layout: 'fit',
            contentEl: frame
        }).show();

        frame.parentContainer = pwdWin; //弹出窗口控件传给iframe      
    }

    closePwdWin = function () {
        pwdWin.close();
    }

    var aboutWin;
    //关于
    WF.ProductAbout = function () {
        var frame = document.createElement("IFRAME");
        frame.id = "frame1";
        frame.frameBorder = 0;
        frame.src = C_ROOT + 'SUP/ProductAbout/ProductAbout';
        frame.height = "100%";
        frame.width = "100%";

        aboutWin = Ext.create('Ext.window.Window', {
            title: '关于',
            autoScreen: true,
            iconCls: 'icon-Key',
            closable: true,
            maximizable: false,
            resizable: false,
            modal: true,
            border: false,
            width: 420,
            height: 510,
            layout: 'fit',
            contentEl: frame
        }).show();

        frame.parentContainer = aboutWin; //弹出窗口控件传给iframe      
    }

    closeAboutWin = function () {
        aboutWin.close();
    }

    WF.Checker = {};

    WF.ListObserver = Ext.create('Ext.util.MixedCollection');

    WF.NotCompleteRequest = [];//未完成请求

    WF.KG = {};//金格对象，key为tab页面id

    WF.SUPCAN = {};//SUPCAN对象，key为tab页面id   

    WF.RefreshSession = function () {
        Ext.Ajax.request({
            url: C_ROOT + "SUP/MainTree/RefreshSession",
            async: false,
            success: function (response) {
                if (response.responseText != '' && response.responseText != null) {
                    var item = Ext.JSON.decode(response.responseText);
                    C_OCode = item.C_OCode;
                    C_OrgName = item.C_OrgName;
                    C_LoginID = item.C_LoginID;
                    C_LoginName = item.C_LoginName;
                    C_DataBase = item.C_DataBase;
                    C_DataBaseName = item.C_DataBaseName;
                }
                var str = Ext.String.format("组织[{0}|{1}],用户[{2}|{3}],帐套[{4}|{5}],项目[{6}]", C_OCode, C_OrgName, C_LoginID, C_LoginName, C_DataBase, C_DataBaseName, C_Project);
                WF.Taskbar.queryById('TaskbarContent').setText(str);
            }
        });
    }

})(MainFrame);