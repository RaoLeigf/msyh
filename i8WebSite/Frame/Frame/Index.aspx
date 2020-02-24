<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="NG3.SUP.Frame.Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <%--<script src="<%= NG2.Pub.JsPath %>WFrame.js"></script>--%>
    <script type="text/javascript" src="../../NG2/Js/WFrame.js"></script>
</body>
</html>
<NG3:SUPConfig ID="SUPConfig1" runat="server">
    <Script>

        WFrame.init();


        WFrame.Login.loginSuccess = function (json) {
            if (!this.dw || !this.dw.lock) {

                WFrame.Left.updateIframe('Index_Left.aspx');
                //WFrame.Center.getItem('home').load('Index_Main.aspx');
                //WFrame.Center.getComponent('home').load('Index_Main.aspx');
                ShowStatus();
            }
            this.hide();
        }

        function ShowStatus() {
            if ($user) {
                WFrame.Taskbar.changtitle("当前用户:[" + $user.id+"]" + $user.loginname);
            }
            else {
                Ext.Ajax.request({
                    url: '../../FrameBase/GetStatusBarContent',
                    success: function (res, opts) {
                        WFrame.Taskbar.changtitle(res.responseText);
                    }
                });
            }
        }

        WFrame.active();

        Ext.onReady(function () {

            window.onbeforeunload = function () {
                //                var f = $GetWFrame();
                //                if (f && f.Center) {
                //                    var tab = f.Center;

                //                    for (var i = 0; i < tab.items.length; i++) {                       
                //                        //tab.remove(tab.items.items[i]);
                //                        tab.fireEvent('beforeremove', tab, tab.items.items[i]);
                //                    }
                //                }

                //debugger;
                WFrame.Checker.fireEvent('checkin', WFrame.Checker);
            }

        });


    </Script>
</NG3:SUPConfig>
