using SUP.Common.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SUP.Frame.Facade
{
    public interface ILoginFacade
    {

        void Check(ref string msg, ref bool loginflag, string svrName, string account, string logid, string pwd);

        string CheckUserOnline(string logid, string curAccName, string macAddress);

        bool SetLoginUsers(string macAddress, ref string guid, ref string msg);

        void LoginOut(string code);

        void KillLoginUser();

        Dictionary<string, string> GetLabelLang(string busiType, string svrName, string account);

        //Dictionary<string, string> GetPortalListBarLang(string svrName, string account);

        Dictionary<string, string> GetPortalListBarLang(string language,string conn);

        string GetLangInfo(string svrName, string account);

        string GetLoginIDByMobileOrEmail(string svrName, string account, string userName);       

        string GetAttachid(string src);

        string WebCheck(out string msg, out bool loginflag, string svrName, string account, string logid, string pwd, I6WebAppInfo appInfo, string isOnlineCheck);

        string SetLoginUsers(I6WebAppInfo appInfo);

        void UpdateLastLoginOrg(string loginorg);

         //DBConnectionStringBuilder GetAcountDBConnectString(string svrName, string database, out string pubConn, out string userConn);

        DataTable GetLoginOrgList(out string msg, string userid, string database);

        string GetEncryPassWord(string svrName, string database, string logid);


        string GetCustomTitle();

        string GetConnectType(string svrName, string account);
    }
}
