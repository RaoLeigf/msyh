using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SUP.Frame.Rule;
using NG3;
using SUP.Common.Base;
using SUP.Frame.DataAccess;
using System.Data;

namespace SUP.Frame.Facade
{
    public class LoginFacade : ILoginFacade
    {
        private LoginDac dac = new LoginDac();
        private LoginRule rule; 

        public LoginFacade()
        {
            rule = new LoginRule();
        }

        //[DBControl]
        public void Check(ref string msg, ref bool loginflag, string svrName, string account, string logid, string pwd)
        {
             rule.Check(ref msg, ref loginflag, svrName, account, logid, pwd);
        }


        public string CheckUserOnline(string logid, string curAccName, string macAddress)
        {
            return rule.CheckUserOnline(logid, curAccName, macAddress);
        }


        public Dictionary<string, string> GetLabelLang(string busiType, string svrName, string account)
        {
            return rule.GetLabelLang(busiType,svrName,account);
        }

        //public Dictionary<string, string> GetPortalListBarLang(string svrName, string account)
        //{
        //    return rule.GetPortalListBarLang(svrName,account);
        //}

        public string GetLangInfo(string svrName, string account)
        {
            return rule.GetLangInfo(svrName, account);
        }

        public Dictionary<string, string> GetPortalListBarLang(string language,string conn)
        {
            return rule.GetPortalListBarLang(language,conn);
        }

        public bool SetLoginUsers(string macAddress, ref string guid, ref string msg)
        {
            return rule.SetLoginUsers(macAddress, ref guid, ref msg);
        }

        public void LoginOut(string code)
        {
            rule.LoginOut(code);
        }

        public void KillLoginUser()
        {
            rule.KillLoginUser();
        }

        public string GetLoginIDByMobileOrEmail(string svrName, string account, string userName)
        {
             return rule.GetLoginIDByMobileOrEmail(svrName,account,userName);
        }

        public string GetAttachid(string src)
        {
            return rule.GetAttachid(src);
        }

        public string WebCheck(out string msg, out bool loginflag, string svrName, string account, string logid, string pwd, I6WebAppInfo appInfo, string isOnlineCheck)
        {
            return rule.WebCheck(out msg, out loginflag, svrName, account, logid, pwd,appInfo, isOnlineCheck);
        }

        public string SetLoginUsers(I6WebAppInfo appInfo)
        {
            return dac.SetLoginUsers(appInfo);
        }

        public void UpdateLastLoginOrg(string loginorg)
        {
            dac.UpdateLastLoginOrg(loginorg);
        }

        //public static DBConnectionStringBuilder GetAcountDBConnectString(string svrName, string database, out string pubConn, out string userConn)
        //{
        //    return LoginDac.GetAcountDBConnectString(svrName, database, out pubConn, out userConn);
        //}

        public DataTable GetLoginOrgList(out string msg, string userid, string database)
        {
            return dac.GetLoginOrgList(out msg, userid, database);
        }

        public string GetEncryPassWord(string svrName, string database, string logid)
        {
            return dac.GetEncryPassWord(svrName, database, logid);
        }


        public string GetCustomTitle() {
            return dac.GetCustomTitle();
        }

        public string GetConnectType(string svrName, string account)
        {
            return dac.GetConnectType(svrName,account);
        }
    }
}
