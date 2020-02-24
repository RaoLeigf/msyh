<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Part_ChangePW.aspx.cs" Inherits="NG3.SUP.Frame.Part_ChangePW" %>

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
        <NG3:SUPTemplateItem ID="t1" Path="Part_ChangePW.xml"></NG3:SUPTemplateItem>
    </Templates>
    <Script>
        var AF = $AF.create(SUPObject1);


        w.init = function () {

            winCmp.setTitle("修改密码", 'icon-Key');

            winCmp.getDockedItems()[1].add({ xtype: 'button', text: '确定', handler: function () {

                var pw = AF.get('EditOldPW').value();
                var newpw = AF.get('EditNewPW').value();
                var newpw2 = AF.get('EditNewPW2').value();

                if (newpw != newpw2) {
                    AF.MessageBoxFloat("新密码输入不一致!");
                }
                else if (pw == newpw) {
                    AF.MessageBoxFloat("新密码不能与原密码相同!");
                }
                else {
                    Ext.Ajax.request({
                        url: '../../Part_Login/ChangePW',
                        params: { pw: pw, newpw: newpw },
                        success: function (res) {

                            if (res.valid) {
                                if (res.text == "1") {
                                    AF.MessageBoxFloat("密码修改成功!");
                                    winCmp.hide();
                                }
                                else if (res.text == "0") {
                                    AF.MessageBoxFloat("原密码错误!");
                                }
                                else if (res.text == "-1") {
                                    AF.MessageBoxFloat("不存在该用户信息!");
                                }
                            }

                        }
                    });
                }
            }
            });
            winCmp.getDockedItems()[1].add({ xtype: 'button', text: '关闭', handler: function () {
                winCmp.destroy();
            }
            });
            winCmp.doLayout();
        };

        w.dealAction = function () {

        };

        w.hideAction = function () {
            
        };

        AF.OnReady = function () {
            AF.get('EditUser').setValue($user.loginname);
            AF.get('EditOldPW').value('');
            AF.get('EditNewPW').value('');
            AF.get('EditNewPW2').value('');

            w.isReady = true;
        };
    </Script>

</NG3:SUPConfig>
