//valueField  统一用code
//displayField 统一用 name
var CallPBCommon = {};
(function (me) {

    //下载控件的提示
    me.$insertActiveX = function (downLoadCodebase, URL_NSServer, URL_Shortcut) {

            var str = '';
            str = '<OBJECT name="AF" width="100%" height="25" CLASSID="clsid:B371A9E1-158C-4318-B6F7-8D0BB9AE041E" '
           + ' Codebase="' + downLoadCodebase + '">'
           + '<Param Name="Lgg" Value="Chinese">'
           + '<Param Name="ClientInner_DLL" Value="ClientInner.dll">'
           + '<Param Name="ClientInner_Version" Value="3.0.0.11">'
           + '<Param Name="URL_NSServer" Value="' + URL_NSServer + '">'
           + '<Param Name="MainProductID" Value="G6">'
           + '<Param Name="URL_Shortcut" Value="">'
           + '<Param Name="inifile" Value="Modules.ini">'
           + '<font color="#000000" face="宋体" size="3">无法下载、运行新中大客户端控件?<br> 请在 "Internet选项"'
           + '中降低安全级别，使得ActiveX控件允许被下载，然后重新刷新本页<br> 或者点<a href="' + URL_Shortcut + '/SetupENV.exe">客户端安装程?</a>，下载,运行该程序即可.'
           + '</font>'
           + ' </OBJECT>';
            return str;
        
    };
    //参数组装
    me.$params = function (orgNo, orgName, year, userName, userNo, URL_Server, NSURL, netcallserver, parameter, connecting, userPwd, module, enterNo, depno, pid) {

            var params = "@xzd@" + orgNo + "@xzd@" + orgName
                        + "@xzd@" + year + "@xzd@" + userName
                        + "@xzd@" + userNo + "@xzd@" + URL_Server
                        + "@xzd@" + NSURL + "@xzd@" + netcallserver
                        + "@xzd@" + parameter + "@xzd@" + connecting
                        + "@xzd@" + userPwd + "@xzd@" + module
                        + "@xzd@" + enterNo + "@xzd@" + depno + "@xzd@" + pid
                        + "@xzd@";

            return params;
    }

})(CallPBCommon)