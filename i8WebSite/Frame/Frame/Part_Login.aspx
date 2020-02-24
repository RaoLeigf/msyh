<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Part_Login.aspx.cs" Inherits="NG3.SUP.Frame.Part_Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <NG3:SUPObject ID="SUPObject1" runat="server" TemplateID="t1" Bottom="0">
    </NG3:SUPObject>
</body>
</html>
<NG3:SUPConfig ID="SUPConfig1" runat="server">
    <Templates>
        <NG3:SUPTemplateItem ID="t1" Path="part_login.xml"></NG3:SUPTemplateItem>
    </Templates>
    <Script>
        
        var AF = $AF.create(SUPObject1);

        AF.OnReady = function () {
            var ddlServer = AF.get('DDLServer'), ddlAcc = AF.get('DDLAcc'), editUser = AF.get('EditUser');

            AF.OpenLoadMask();
            Ext.Ajax.request({
                url: '../../Part_Login/GetServerList',
                success: function (res, opts) {
                    if (res.valid) {
                        AF.SetDroplistProp("DDLServer", "dataURL", res.responseText);
                        if (Cookie.get('DDLServerValue')) {
                            ddlServer.SetValue(Cookie.get('DDLServerValue'));
                        }
                        if (Cookie.get('DDLAccValue')) {
                            AF.SetDroplistProp("DDLAcc", "dataURL", '{Record:[{ucode:"' + Cookie.get('DDLAccValue') + '",uname:"' + Cookie.get('DDLAccText') + '"}');
                            ddlAcc.setValue(Cookie.get('DDLAccValue'));
                        }
                        if (Cookie.get('EditUserName')) {
                            editUser.setValue(Cookie.get('EditUserName'))
                        }
                    }
                },
                callback: function () {
                    AF.CloseLoadMask();
                }
            });

            ddlServer.OnEditChanged = function () {
                Ext.Ajax.request({
                    url: '../../Part_Login/GetAccList',
                    params: { svrName: this.getValue() },
                    success: function (res, opts) {
                        AF.SetDroplistProp("DDLAcc", "dataURL", res.responseText);
                    }
                });
            }

            w.isReady = true;
        }

        w.lockHandler = function () {
            w.lock = true;
            AF.get('DDLServer').enable(0);
            AF.get('DDLAcc').enable(0);
            AF.get('EditUser').enable(0);
            AF.get('EditPass').setValue('');
        }

        w.resetHandler = function () {
            AF.get('EditUser').value('');
            AF.get('EditPass').value('');
        }

        w.loginHandler = function (bFunc, eFunc) {
            var me = this;
            me.disable();
            //返回mac地址
            AF.SetObjectProp('EditMac', 'value', AF.GetMac());
            var v = true;
            if (Ext.isFunction(bFunc)) {
                v = bFunc();
            }
            if (v) {
                Ext.Ajax.request({
                    url: '../../Part_Login/login',
                    params: AF.GetChangedXML(),
                    success: function (res, opts) {
                        v = successHandler(res, opts);
                        if (Ext.isFunction(eFunc)) {
                            eFunc(v);
                        }
                        me.enable();
                    }
                });
            }
        }

        w.loginSuccess = function (json) {
            Cookie.set('DDLServerValue', AF.get('DDLServer').getValue());
            Cookie.set('DDLServerText', AF.get('DDLServer').getText());
            Cookie.set('DDLAccValue', AF.get('DDLAcc').getValue());
            Cookie.set('DDLAccText', AF.get('DDLAcc').getText());
            Cookie.set('EditUserName', AF.get('EditUser').getValue());
        }

        function successHandler(res, opts) {
            var json = $DecodeResponse(res), succ = false;
            if (json) {
                succ = true;             
                w.loginSuccess(json);
                if (w.lastRequestOptions && w.lastRequestOptions.url) {
                    Ext.Ajax.request(w.lastRequestOptions);
                    w.lastRequestOptions = null;
                }
            }
                        
            return succ;
        }
    </Script>
</NG3:SUPConfig>
