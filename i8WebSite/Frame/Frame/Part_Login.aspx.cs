using System;
using NG3;
using NG3.Data.Builder;
using NG3.Web.UI;
using System.Data;
//using NG2.WCached.Client;
using System.Web;
using NG3.Common.Web.UI;
//using i6.UIProcess.MainFrame;
using NG3.Base;

namespace NG3.SUP.Frame
{
    public partial class Part_Login : NG2Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        #region GetServerList 服务器列表
        [UrlMethod, UrlSecurity(UrlSecurity.Everyone)]
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
        #endregion

        #region GetAccList 获取帐套列表
        [UrlMethod, UrlSecurity(UrlSecurity.Everyone)]
        public void GetAccList(string svrName)
        {
            string pubConnStr = DataBaseConfigcs.GetPubConnString(svrName);
            string dbType = DataBaseConfigcs.GetDbType(svrName);

            string  sql = "select ucode,ucode + '-' + uname  uname from ngusers";
            if("OracleClient" == dbType)
            {
                sql = "select ucode,ucode||'-'||uname  uname from ngusers";
            }
            

             DataTable dt = NG3.Data.Service.DbHelper.GetDataTable(pubConnStr,sql);
             Ajax.WriteRaw(dt.ToJSON("ucode", "uname"));             

        }

       
        #endregion

        #region Login 登录服务
        [UrlMethod, UrlSecurity(UrlSecurity.Everyone)]
        public void Login()
        {
            var fm = this.RequestMeta.AsFormMeta();
            object[] ps = new object[] { fm.Get("DDLServer"), fm.Get("DDLAcc"), fm.Get("EditUser"), fm.Get("EditPass"), string.Empty };
            bool loginSucc = NG3.Common.ESB.GetService<ServiceLogin>().Invoke(ps).Value.TryParseToBool();
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
        #endregion



        
    }
}