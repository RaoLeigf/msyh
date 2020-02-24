using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Mvc;

using NG3;
//using NG3.SUP.Frame;
using System.Data;
using NG3.Data.Builder;
using NG3.Web.Mvc;
using NG3.Web.Controller;

using NG3.Base;
using NG3.Common;

namespace SUP.Frame.Controller
{
    public class Part_LoginController : AFController
    {

        public Part_LoginController()
        { }

        /// <summary>
        /// GetServerList 服务器列表
        /// </summary>
        [UrlMethod, UrlSecurity(UrlSecurity.Everyone), ActionAuthorize(Level.NonCheck)]
        public void GetServerList()
        {
            DataSet ds = DataBaseConfigcs.DataBaseInfo;
            try
            {
                Ajax.WriteRaw(ds.Tables["Connect"].ToJSON("byname", "servername"));
            }
            catch (Exception)
            {
                throw;
            }

        }
        /// <summary>
        /// GetAccList 获取帐套列表
        /// </summary>
        /// <param name="svrName"></param>
        [UrlMethod, UrlSecurity(UrlSecurity.Everyone), ActionAuthorize(Level.NonCheck)]
        public void GetAccList(string svrName)
        {
            string pubConnStr = DataBaseConfigcs.GetPubConnString(svrName);
            string dbType = DataBaseConfigcs.GetDbType(svrName);

            string sql = "select ucode,ucode + '-' + uname  uname from ngusers";
            if ("OracleClient" == dbType)
            {
                sql = "select ucode,uname from ngusers";
            }


            DataTable dt = NG3.Data.Service.DbHelper.GetDataTable(pubConnStr, sql);

            if ("OracleClient" == dbType)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dr["uname"] = dr["ucode"].ToString() + "-" + dr["uname"].ToString();
                }
            }

            Ajax.WriteRaw(dt.ToJSON("ucode", "uname"));

        }
        /// <summary>
        /// 登录服务
        /// </summary>
        [UrlMethod, UrlSecurity(UrlSecurity.Everyone), ActionAuthorize(Level.NonCheck)]
        public void Login()
        {
            var fm = this.RequestMeta.AsFormMeta();
            object[] ps = new object[] { fm.Get("DDLServer"), fm.Get("DDLAcc"), fm.Get("EditUser"), fm.Get("EditPass"), string.Empty };
            bool loginSucc = ESB.GetService<ServiceLogin>().Invoke(ps).Value.TryParseToBool();
            string msg = ps[4] as string;
            if (loginSucc)
            {
                Ajax.ShowMessage(AjaxType.Succ);
            }
            else
            {
                Session.Abandon();
                if (!string.IsNullOrEmpty(msg))
                {
                    Ajax.ShowMessage(AjaxType.Fail, msg);
                }
                else
                {
                    Ajax.ShowMessage(AjaxType.Fail, "登录失败,错误的用户名或密码!");
                }
            }

        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="pw"></param>
        /// <param name="newpw"></param>
        [UrlMethod, UrlSecurity(UrlSecurity.LoginUser)]
        public void ChangePW(string pw, string newpw)
        {
            Ajax.WriteRaw(ESB.GetService<UIP>().ChangePW(pw, newpw).ToString());
        }

    }
}
