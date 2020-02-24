<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index_Left.aspx.cs" Inherits="NG3.SUP.Frame.Index_Left" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<script type="text/javascript">
        
</script>
<body>
    <NG3:SUPObject ID="SUPObject1" runat="server" ObjectType="Tree" Bottom="0">
    </NG3:SUPObject>
</body>
</html>
<NG3:SUPConfig ID="SUPConfig1" runat="server">
    <Templates>
        <NG3:SUPTemplateItem ID="tdata" Path="FuncTree.xml"></NG3:SUPTemplateItem>
        <NG3:SUPTemplateItem ID="tdataT" Path="FuncTreeTeacher.xml"></NG3:SUPTemplateItem>
        <NG3:SUPTemplateItem ID="tdataS" Path="FuncTreeStudent.xml"></NG3:SUPTemplateItem>
    </Templates>
    <Script>
        var T = $AF.create(SUPObject1);
        T.OnReady = function () {
            if ($user) {
                if ($user.usergroup == 'M') {
                    T.func('ReadXML', tdata);
                }
                if ($user.usergroup == 'T') {
                    T.func('ReadXML', tdataT);
                }
                if ($user.usergroup == 'S') {
                    T.func('ReadXML', tdataS);
                }
            }
        };

        T.OnDblClicked = function (p1, p2, p3) {
            parent.WFrame.Center.openTab(p3, p2);
        }
    </Script>
</NG3:SUPConfig>
