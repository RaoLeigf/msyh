<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="NG3.SUP.Frame.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="float: right;">
        <img src="../../resource/pic/copyright.jpg" /></div>
    <div style="position: absolute; left: 0; top: 377px; height: 60px; width: 100%; text-align: center;">
        <img src="../../resource/pic/bg.jpg" style="height: 100%; width: 100%;" />
    </div>
    <div style="position: absolute; left: 20%; top: 112px; border-style: hidden;">        
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <img src="../../resource/pic/product.jpg" />
                </td>
                <td valign=top style="padding-top:100px">
                    <iframe id="ifr" src="Part_Login.aspx" frameborder="0" width="320" height="120"></iframe>
                    <p style="padding-top: 5px">
                        <div id="btnReset" style="margin-left:10px;float:right"></div>
                        <div id="btnLogin" style="float:right"></div>
                    </p>
                </td>
            </tr>
        </table>
    </div>
    <NG3:SUPConfig ID="SUPConfig1" runat="server">
        <Script>
           
            var btnLogin = new Ext.Button({
                renderTo: 'btnLogin',
                iconCls: 'icon-LockGo',
                hidden: true,
                width: 60,
                height: 26,
                text: '登陆',
                handler: function () {

                    var isReady = $$('ifr').contentWindow.isReady;

                    var func = Ext.Function.bind($$('ifr').contentWindow.loginHandler, this);

                    if (isReady && Ext.isFunction(func)) {
                        func(
                            function () {
                                //begin
                                Ext.MessageBox.show({
                                    msg: '正在登陆, 请稍后...',
                                    width: 300,
                                    wait: true,
                                    waitConfig: { interval: 400 },
                                    icon: 'icon-MonitorGo'
                                });
                                $mask(1);
                                btnLogin.disable();
                                return true
                            },
                            function (succ) {
                                //end
                                if (succ) {
                                    location.href = "Index.aspx";
                                }
                                else {
                                    $mask(0);
                                    Ext.Msg.hide();
                                    btnLogin.enable();
                                }
                            }
                        );
                    }
                }
            });

            var btnReset = new Ext.Button({
                renderTo: 'btnReset',
                iconCls: 'icon-ArrowUndo',
                hidden: true,
                width: 60,
                height: 26,
                text: '重置',
                handler: function () {
                    var func = $$('ifr').contentWindow.resetHandler;
                    if (Ext.isFunction(func)) {
                        func();
                    }
                }
            });


            var task = {
                run: function () {
                    if ($$('ifr').contentWindow.isReady) {
                        btnLogin.show();
                        btnReset.show();
                        //Ext.TaskMgr.stop(this);
                        Ext.TaskManager.stop(this);
                    }
                },
                interval: 100
            }

            //Ext.TaskMgr.start(task);
            Ext.TaskManager.start(task);


        </Script>
    </NG3:SUPConfig>
    </form>
</body>
</html>
