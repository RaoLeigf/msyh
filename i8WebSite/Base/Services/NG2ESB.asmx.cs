using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using NG3;
using NG3.ESB;
using NG3.Common;
using NG3.Base;

namespace NG3.SUP.Base
{
    /// <summary>
    /// Summary description for NG2ESB
    /// </summary>
    [WebService(Namespace = "http://ng2.newgrand.cn/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class NG2ESB : System.Web.Services.WebService
    {
        //[WebMethod(EnableSession = true)]
        //public object InvokeService(string serviceFullName, params object[] ps)
        //{
        //    object v = null;
        //    var s = this.GetService(serviceFullName);

        //    var d = s.Invoke(ps);
        //    if (d != null) v = d.Value;

        //    return v;
        //}

        #region Login
        [WebMethod(EnableSession = true)]
        public bool Login(string servername, string account, string logid, string pwd)
        {
            if (AppInfo.IsUserLogined && AppInfo.UCode.EqualsIgnoreCase(account) && AppInfo.LoginID.EqualsIgnoreCase(logid))
            {
                return true;
            }
            else
            {
                return NG3.Common.ESB.Base.Login(servername, account, logid, pwd);
            }
        } 
        #endregion

        [WebMethod(EnableSession = true)]
        public object InvokeMember(string serviceFullName,string methodName, params object[] ps)
        {
            var s = this.GetService(serviceFullName);
            return s.InvokeMethod(methodName, ps); 
        }

        [WebMethod(EnableSession = true)]
        public object InvokeMemberRef(string serviceFullName, string methodName,  ref object[] ps)
        {
            var s = this.GetService(serviceFullName);
            return s.InvokeMethod(methodName, ps);
        }

        private ServiceBase GetService(string serviceFullName)
        {
            var s = NG3.Common.ESB.GetService(serviceFullName);

            if (s.InvokeLevel == ESBInvokeLevel.Public)
            {
                return s;
            }

            throw new Exception(Resources.Base.NG2ESBInvokeLevelError.FormatWith(serviceFullName, s.InvokeLevel.ToString()));

        }
    }
}
