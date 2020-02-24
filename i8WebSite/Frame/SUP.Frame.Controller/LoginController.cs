using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using SUP.Common.Base;
using System.Data;
using System.Web.Mvc;
//using NG3.SUP.Frame;
using NG3.Data.Service;
using NG3.Web.Mvc;
using NG3.Web.Controller;
using SUP.Frame.Facade;
using NG3.Aop.Transaction;
using System.Security.Cryptography;
using i6.UIProcess.MainFrame;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Configuration;
using I6.Base.Message;
using NG.KeepConn;
using i6.Web.i6Service;

namespace SUP.Frame.Controller
{
    public class LoginController : AFController//System.Web.Mvc.Controller
    {
        private const string UPAppInfoNameInSession = "NGWebAppInfo";

        private const string WebNGWebAppInfo = "webNGWebAppInfo";
        private const string LoginMark = "webLoginMark";

        private ILoginFacade proxy;

        public LoginController()
        {
            proxy = AopObjectProxy.GetObject<ILoginFacade>(new LoginFacade());
        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult Index()
        {
            string targetulr = HttpContext.Request.Params["targetulr"];
            if (!string.IsNullOrEmpty(targetulr))
            {
                ViewBag.targetUrl = HttpUtility.UrlDecode(targetulr);
            }

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            //将登陆密码的私钥存Session中
            Session["private_key"] = rsa.ToXmlString(true);
            RSAParameters parameter = rsa.ExportParameters(true);
            //公钥
            ViewBag.strPublicKeyExponent = this.BytesToHexString(parameter.Exponent);
            ViewBag.strPublicKeyModulus = this.BytesToHexString(parameter.Modulus);

            return View("Login");
        }

        private string BytesToHexString(byte[] input)
        {
            StringBuilder hexString = new StringBuilder(64);
            for (int i = 0; i < input.Length; i++)
            {
                hexString.Append(String.Format("{0:X2}", input[i]));
            }
            return hexString.ToString();
        }

        public Dictionary<string, string> GetPortalListBarLang(string language, string conn)
        {

            return proxy.GetPortalListBarLang(language, conn);
        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult PopLogin()
        {
            return View("PopLogin");
        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public string GetServerList()
        {
            DataSet ds = new DBConnectionStringBuilder().DBConfig;
            try
            {
                //Ajax.WriteRaw(ds.Tables["Connect"].ToJSON("byname", "servername"));

                string str = DataConverterHelper.ToJson(ds.Tables["Connect"], ds.Tables["Connect"].Rows.Count);
                return str;

                //return this.Json(str, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public string GetAccList(string svrName)
        {
            DBConnectionStringBuilder dbbuilder = new DBConnectionStringBuilder();
            string result;
            string pubConnStr = dbbuilder.GetMainConnStringElement(svrName, out result);

            //string sql = "select ucode,uname from ngusers order by ucode ";  

            string sql = "select dbname,uname from ngusers order by ucode ";

            DataTable dt = NG3.Data.Service.DbHelper.GetDataTable(pubConnStr, sql);

            foreach (DataRow dr in dt.Rows)
            {
                //dr["uname"] = dr["ucode"].ToString() + "-" + dr["uname"].ToString();
                dr["uname"] = dr["dbname"].ToString() + "-" + dr["uname"].ToString();
            }

            //Ajax.WriteRaw(dt.ToJSON("ucode", "uname"));

            string str = DataConverterHelper.ToJson(dt, dt.Rows.Count);
            return str;

            //return this.Json(str, JsonRequestBehavior.AllowGet);

        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public string Login()
        {
            string msg = string.Empty;
            bool loginflag = true;

            //验证码
            string checkCode = System.Web.HttpContext.Current.Request.Params["CheckCode"];

            if (!string.IsNullOrWhiteSpace(checkCode))
            {
                if (!checkCode.Equals(Session["checkcode"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    msg = "验证码不正确！";
                    return "{ success: false,msg:'" + msg + "'}";
                }
            }

            //数据库服务器
            string svrName = System.Web.HttpContext.Current.Request.Params["ServerName"];
            //帐套
            string account = System.Web.HttpContext.Current.Request.Params["EnterPriseName"];
            //用户名
            string logid = System.Web.HttpContext.Current.Request.Params["UserID"];
            //密码
            string pwd = System.Web.HttpContext.Current.Request.Params["UserPwd"];


            if (!string.IsNullOrWhiteSpace(pwd))
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString((string)Session["private_key"]);
                byte[] result = rsa.Decrypt(HexStringToBytes(pwd), false);
                UTF8Encoding utf8encoder = new UTF8Encoding();
                pwd = utf8encoder.GetString(result);
            }

            //Check(ref msg, ref loginflag, svrName, account, logid, pwd);
            proxy.Check(ref msg, ref loginflag, svrName, account, logid, pwd);

            if (loginflag)
            {
                return "{ success: true}";
            }
            else
            {
                return "{ success: false,msg:'" + msg + "'}";
            }
        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public void SessionAbandon()
        {
            proxy.KillLoginUser();
            System.Web.HttpContext.Current.Session.Abandon();
        }

        private byte[] HexStringToBytes(string hex)
        {
            if (hex.Length == 0)
            {
                return new byte[] { 0 };
            }

            if (hex.Length % 2 == 1)
            {
                hex = "0" + hex;
            }

            byte[] result = new byte[hex.Length / 2];

            for (int i = 0; i < hex.Length / 2; i++)
            {
                result[i] = byte.Parse(hex.Substring(2 * i, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            }

            return result;
        }

        #region web登录归并
        //[ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        //public ActionResult Index()
        //{
        //    return View("Login");
        //}

        //[ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        //public ActionResult PopLogin()
        //{
        //    return View("PopLogin");
        //}

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public string WebLogin()
        {
            //SUP.Common.Base.DBConnectionStringBuilder db = new SUP.Common.Base.DBConnectionStringBuilder();

            //string conn = db.GetDefaultConnString();
            //ConnectionInfoService.SetSessionConnectString(conn);

            string msg = string.Empty;
            bool loginflag = true;

            //用户名
            string logid = System.Web.HttpContext.Current.Request.Params["UserID"];
            //密码
            string pwd = System.Web.HttpContext.Current.Request.Params["UserPwd"];
            //帐套
            string database = System.Web.HttpContext.Current.Request.Params["DataBase"];
            //是否检测在线用户
            string isOnlineCheck = System.Web.HttpContext.Current.Request.Params["IsOnlineCheck"];

            try
            {
                WebCheck(out msg, out loginflag, string.Empty, database, logid, pwd, isOnlineCheck);
            }
            catch (Exception ex)
            {
                return "{ success: false,msg:'" + "登录系统异常:" + ex.Message + "'}";
            }
            if (loginflag)
            {
                return "{ success: true,msg:'" + msg + "'}";
            }
            else
            {
                return "{ success: false,msg:'" + msg + "'}";
            }
        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public void KillOnlineUser(string IpAddress, string Devicetype, string UserId, string SessionID)
        {
            if (Devicetype == "1")
            {
                //发送桌面消息推送,用于Web版强制下线登录用户
                DesktopPortalRefreshNotice notice = new DesktopPortalRefreshNotice();
                string logoutMessage = "当前登录被强制注销，点击确定后将取消当前登录！";
                DataTable receiverDt = i6.Biz.MessageTimeManager.GetTimeTriggerObjectDt();
                receiverDt.TableName = "receiver";
                receiverDt.Rows.Add();
                receiverDt.Rows[0]["uid"] = UserId;
                receiverDt.Rows[0]["utype"] = i6.Biz.ReceiverType.Type_Ope;
                notice.NoticeDesktopRefreshMsg("SYSTEM", "KillLoginUser", receiverDt, logoutMessage);
            }
            else
            {
                new WinLoginService().KillLoginUser(SessionID);
            }
        }

        /// <summary>
        /// 弹出消息
        /// </summary>
        /// <param name="msg"></param>
        private void AlertMsg(string msg)
        {
            this.Response.Write(string.Format("<script language='javascript'>alert('{0}')</script>", msg));
        }
        /// <summary>
        /// 特变专用单点登录
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        [HttpGet]
        public void TbSignLogin()
        {
            try
            {
                //E+ ticket 获取logid
                string ticket = System.Web.HttpContext.Current.Request.QueryString["ticket"];//必须
                if (string.IsNullOrWhiteSpace(ticket))
                {
                    AlertMsg("E+Ticket必须传!");
                    return;
                }
                #region 可选参数
                //帐套
                string database = System.Web.HttpContext.Current.Request.QueryString["database"];
                //组织
                string orgid = System.Web.HttpContext.Current.Request.QueryString["orgid"];
                //直接打开的url
                string openUrl = System.Web.HttpContext.Current.Request.QueryString["openUrl"];
                //直接打开的url
                string urlTitle = System.Web.HttpContext.Current.Request.QueryString["urlTitle"];
                var openAloneStr = System.Web.HttpContext.Current.Request.QueryString["openAlone"];
                bool openAlone = false;
                bool.TryParse(openAloneStr, out openAlone);
                #endregion
                //var tools = new ControllerTools();
                var logid = string.Empty;
                try
                {
                    if (!this.EJiaValidateUser(ticket, out logid))
                    {
                        AlertMsg("E+获取Logid失败!");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    AlertMsg(string.Format("E+获取Logid异常：{0}!", ex.Message));
                    return;
                }
                #region 系统模拟登录
                //var pwd = tools.GetEncryPassWord(string.Empty, database, logid);
                var pwd = proxy.GetEncryPassWord(string.Empty, database, logid);
                if (!string.IsNullOrWhiteSpace(pwd))
                    pwd = NG3.NGEncode.DecodePassword(pwd, pwd.Length);
                string msg = string.Empty;
                bool loginflag = true;
                var userConn = string.Empty;
                try
                {
                    //userConn = Check(out msg, out loginflag, string.Empty, database, logid, pwd);
                    userConn = this.WebCheck(out msg, out loginflag, string.Empty, database, logid, pwd);
                }
                catch (Exception ex)
                {
                    AlertMsg(string.Format("系统登录异常：{0}!", ex.Message));
                    return;
                }
                if (!loginflag)
                {
                    AlertMsg(string.Format("系统登录失败：{0}!", msg));
                    return;
                }
                try
                {
                    if (!string.IsNullOrWhiteSpace(orgid) && string.IsNullOrWhiteSpace(msg))
                    {
                        var orgDt = DbHelper.GetDataTable(userConn, string.Format("SELECT phid,ocode,oname FROM fg_orglist phid='{0}'", orgid));
                        if (orgDt != null && orgDt.Rows.Count > 0)
                        {
                            ChangeOrg(orgDt.Rows[0]["ocode"].ToString(), orgDt.Rows[0]["phid"].ToString(), orgDt.Rows[0]["oname"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    AlertMsg(string.Format("切换组织失败：{0}!", ex.Message + ex.StackTrace));
                    return;
                }
                try
                {
                    #endregion
                    if (openAlone)
                    {
                        Response.Redirect(string.Format("{0}/{1}", Request.ApplicationPath, Base64ToStr(openUrl)));
                    }
                    else
                        Response.Redirect(string.Format("../Home/Index?openUrl={0}&urlTitle={1}", openUrl, urlTitle));
                }
                catch (Exception ex)
                {
                    AlertMsg(string.Format("URL跳转失败：{0}!", ex.Message));
                    return;
                }
            }
            catch (Exception ex)
            {
                AlertMsg(string.Format("未知错误：{0}!", ex.Message));
                return;
            }
        }

        /// <summary>
        /// 特变专用单点登录
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        [HttpGet]
        public void SignLogin()
        {
            try
            {
                #region 可选参数
                //登录人
                string logid = System.Web.HttpContext.Current.Request.QueryString["logid"];

                //登录人
                string pwd = System.Web.HttpContext.Current.Request.QueryString["pwd"];

                //帐套
                string database = System.Web.HttpContext.Current.Request.QueryString["database"];
                //组织
                string orgid = System.Web.HttpContext.Current.Request.QueryString["orgid"];
                //直接打开的url
                string openUrl = System.Web.HttpContext.Current.Request.QueryString["openUrl"];
                //直接打开的url
                string urlTitle = System.Web.HttpContext.Current.Request.QueryString["urlTitle"];
                var openAloneStr = System.Web.HttpContext.Current.Request.QueryString["openAlone"];
                bool openAlone = false;
                bool.TryParse(openAloneStr, out openAlone);
                #endregion
                //var tools = new ControllerTools();
                #region 系统模拟登录
                //var pwd = tools.GetEncryPassWord(string.Empty, database, logid);
                if (!string.IsNullOrWhiteSpace(pwd))
                    pwd = NG3.NGEncode.DecodePassword(pwd, pwd.Length);
                string msg = string.Empty;
                bool loginflag = true;
                var userConn = string.Empty;
                try
                {
                    //userConn = Check(out msg, out loginflag, string.Empty, database, logid, pwd);
                    userConn = this.WebCheck(out msg, out loginflag, string.Empty, database, logid, pwd);
                }
                catch (Exception ex)
                {
                    AlertMsg(string.Format("系统登录异常：{0}!", ex.Message));
                    return;
                }
                if (!loginflag)
                {
                    AlertMsg(string.Format("系统登录失败：{0}!", msg));
                    return;
                }
                try
                {
                    if (!string.IsNullOrWhiteSpace(orgid) && string.IsNullOrWhiteSpace(msg))
                    {
                        var orgDt = DbHelper.GetDataTable(userConn, string.Format("SELECT phid,ocode,oname FROM fg_orglist phid='{0}'", orgid));
                        if (orgDt != null && orgDt.Rows.Count > 0)
                        {
                            ChangeOrg(orgDt.Rows[0]["ocode"].ToString(), orgDt.Rows[0]["phid"].ToString(), orgDt.Rows[0]["oname"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    AlertMsg(string.Format("切换组织失败：{0}!", ex.Message + ex.StackTrace));
                    return;
                }
                try
                {
                    #endregion
                    if (openAlone)
                    {
                        Response.Redirect(string.Format("{0}/{1}", Request.ApplicationPath, Base64ToStr(openUrl)));
                    }
                    else
                        Response.Redirect(string.Format("../Home/Index?openUrl={0}&urlTitle={1}", openUrl, urlTitle));
                }
                catch (Exception ex)
                {
                    AlertMsg(string.Format("URL跳转失败：{0}!", ex.Message));
                    return;
                }
            }
            catch (Exception ex)
            {
                AlertMsg(string.Format("未知错误：{0}!", ex.Message));
                return;
            }
        }
        private string Base64ToStr(string base64str)
        {
            if (string.IsNullOrEmpty(base64str))
                return base64str;
            try
            {
                var outputb = Convert.FromBase64String(base64str.Replace("%2B", "+"));
                return Encoding.Default.GetString(outputb);
            }
            catch
            {
                return base64str;
            }
        }
        private string WebCheck(out string msg, out bool loginflag, string svrName, string database, string logid, string pwd, string isOnlineCheck = "")
        {
            var appInfo = new I6WebAppInfo();
            var userConn = proxy.WebCheck(out msg, out loginflag, svrName, database, logid, pwd, appInfo, isOnlineCheck);
            if (!string.IsNullOrEmpty(userConn))
            {
                if (msg == "UserNoHaveOrg")
                {
                    System.Web.HttpContext.Current.Session[WebNGWebAppInfo] = appInfo;
                    SimulateWebLogin(appInfo.UCode.Replace("NG", ""), appInfo.OCode, appInfo.LoginID, appInfo);

                    string guid = string.Empty;
                    if (appInfo.UserType != UserType.System)
                    {
                        if (!proxy.SetLoginUsers("", ref guid, ref msg))//注册在线用户
                        {
                            loginflag = false;
                        }
                    }
                    else
                    {
                        guid = Guid.NewGuid().ToString();
                    }
                    System.Web.HttpContext.Current.Session[LoginMark] = guid;
                }
                else if (string.IsNullOrEmpty(msg))
                {
                    System.Web.HttpContext.Current.Session[UPAppInfoNameInSession] = appInfo;
                    System.Web.HttpContext.Current.Session[WebNGWebAppInfo] = appInfo;
                    ConnectionInfoService.SetSessionConnectString(userConn);//初始化2.0的dbhelper
                    SimulateWebLogin(appInfo.UCode.Replace("NG", ""), appInfo.OCode, appInfo.LoginID, appInfo);

                    string guid = string.Empty;
                    if (appInfo.UserType != UserType.System)
                    {
                        if (!proxy.SetLoginUsers("", ref guid, ref msg))//注册在线用户
                        {
                            loginflag = false;
                        }
                    }
                    else
                    {
                        guid = Guid.NewGuid().ToString();
                    }
                    System.Web.HttpContext.Current.Session[LoginMark] = guid;
                }
            }
            return userConn;
        }
        private bool ChangeOrg(string ocode, string loginorg, string ocodeName)
        {
            var appInfo = System.Web.HttpContext.Current.Session[WebNGWebAppInfo] as SUP.Common.Base.I6WebAppInfo;
            long orgid = 0L;
            if (string.IsNullOrWhiteSpace(ocode) || !long.TryParse(loginorg, out orgid))
            {
                return false;
            }
            appInfo.OCode = ocode;
            appInfo.OrgID = orgid;
            appInfo.OrgName = ocodeName;
            System.Web.HttpContext.Current.Session[WebNGWebAppInfo] = appInfo;
            System.Web.HttpContext.Current.Session[UPAppInfoNameInSession] = appInfo;
            
            SimulateWebLogin(appInfo.UCode.Replace("NG", ""), appInfo.OCode, appInfo.LoginID, appInfo);
            return true;
        }
        /// <summary>
        /// 模拟web登录 aspx 页面要用
        /// </summary>
        /// <param name="curAcc">当前帐套</param>
        /// <param name="asOCode">组织号</param>
        /// <param name="loginID">登陆id</param>
        /// <param name="usertype">用户类型（普通用户：OrgUser；系统管理员：SYSTEM）</param>
        private void SimulateWebLogin(string curAcc, string asOCode, string loginID, SUP.Common.Base.I6WebAppInfo appinfo)
        {
            string UserConnectString = string.Empty;
            //模拟登陆
            WebServiceUIP webUIP = new WebServiceUIP(curAcc, asOCode, loginID, ref UserConnectString);
            webUIP.SetKernelSession();
            //webUIP.SetAppInfo();//会将usertype固定位orguser
            webUIP.SetAppInfo(appinfo.UserType, 0, "1", "");

            this.Session["LoginID"] = appinfo.LoginID;
            this.Session["OCode"] = appinfo.OCode;
            this.Session["UCode"] = appinfo.UCode;
            this.Session["userName"] = appinfo.UserName;
            this.Session["OrgName"] = appinfo.OrgName;
            this.Session["UName"] = appinfo.UName;
            this.Session["PubConnectStr"] = appinfo.PubConnectString;
            this.Session["UserConnectStr"] = UserConnectString; //appinfo.UserConnectString;
            this.Session["CurProduct"] = appinfo.Product;

            this.Session["HostIP"] = Request.UserHostAddress;// HttpContext.Current.Request.UserHostAddress;
            this.Session["HostName"] = Request.UserHostName;// HttpContext.Current.Request.UserHostName;
            this.Session["HostCode"] = "";
            this.Session["IsOAUser"] = appinfo.IsOAUser;
        }
        /// <summary>
        /// 切换组织
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.Normal)]
        public bool ChangeOrg()
        {
            var ocode = System.Web.HttpContext.Current.Request.Params["ocode"];
            var loginorg = System.Web.HttpContext.Current.Request.Params["orgid"];
            var ocodeName = System.Web.HttpContext.Current.Request.Params["ocodeName"];
            var re = ChangeOrg(ocode, loginorg, ocodeName);
            if (re)
            {
                //new LoginDAC().UpdateLastLoginOrg(loginorg);
                proxy.UpdateLastLoginOrg(loginorg);
            }
            return re;
        }
        /// <summary>
        /// 获取默认帐套
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public string GetDefaultDB()
        {
            string pubConn = string.Empty;
            string userConn = string.Empty;
            var dbbuilder = GetAcountDBConnectString(string.Empty, string.Empty, out pubConn, out userConn);
            return dbbuilder.DefaultDB;
        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public void LoginOut()
        {
            if (System.Web.HttpContext.Current.Session[LoginMark] != null)
            {
                var guid = System.Web.HttpContext.Current.Session[LoginMark].ToString();
                proxy.LoginOut(guid);
                System.Web.HttpContext.Current.Session.Abandon();
            }
        }

        /// <summary>
        /// 获取当前登录人可登录组织
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public string GetLoginOrgList()
        {
            string userid = System.Web.HttpContext.Current.Request.Params["userid"];
            string database = System.Web.HttpContext.Current.Request.Params["dataBase"];
            string msg = string.Empty;
            string json = string.Empty;
            //var dac = new LoginDAC();
            //DataTable dt = dac.GetLoginOrgList(out msg, userid, database);
            DataTable dt = proxy.GetLoginOrgList(out msg, userid, database);
            int totalRecord = dt.Rows.Count;
            json = DataConverterHelper.ToJson(dt, totalRecord);
            return json;
        }

        /// <summary>
        /// 获取用户自定义标题
        /// </summary>
        /// <returns></returns>
        public string GetCustomTitle()
        {
            string title = proxy.GetCustomTitle();

            return title;
        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public string GetConnectType()
        {
            string svrName = System.Web.HttpContext.Current.Request.Params["svrName"];
            string account = System.Web.HttpContext.Current.Request.Params["account"];
            string connectType = "0";
            try
            {
                connectType = proxy.GetConnectType(svrName, account);
                return connectType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region E+
        /// <summary>
        /// E+令牌获取
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        private bool EJiaTokenGet(out string access_token, string validateuserhost, string appid, string appsecret)
        {
            //轻应用appid：101153
            //appsecret10135
            access_token = string.Empty;
            string validateuserurl = "http://" + validateuserhost + "/openauth2/api/token?grant_type=client_credential&appid=" + appid + "&secret=" + appsecret;
            var request = (HttpWebRequest)WebRequest.Create(validateuserurl);
            System.Net.ServicePointManager.DefaultConnectionLimit = 50;
            request.Method = "GET";
            request.Accept = "*/*";
            request.ContentType = "application/json";
            request.KeepAlive = false;
            request.UserAgent = "Mozilla/4.0(compatible;MSIE 8.0;Windows NT 6.1;.NET CLR 1.0.3705;)";
            var response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                System.IO.Stream resStream = response.GetResponseStream();
                System.IO.StreamReader streamReader = new System.IO.StreamReader(resStream, System.Text.Encoding.UTF8);
                string content = streamReader.ReadToEnd();
                streamReader.Close();
                resStream.Close();
                //{ "access_token":"ACCESS_TOKEN", "expires_in":3600*24*7}
                JObject jo = JObject.Parse(content);
                if (jo["access_token"] != null)
                {
                    access_token = jo["access_token"].ToString();
                    return true;
                }
            }
            return false;
        }
        private void GetEJiaBaseMsg(out string validateuserhost, out string appid, out string appsecret)
        {
            string Base = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo dr = new DirectoryInfo(Base);
            string ppath = dr.FullName;
            string fileName = Path.Combine(ppath, "DMC\\TimerService\\NG.UP.UC.Service.config");
            validateuserhost = GetConfigAppsetting(fileName, "validateuserhost");
            appid = GetConfigAppsetting(fileName, "appid");
            appid = string.IsNullOrWhiteSpace(appid) ? "101153" : appid;
            appsecret = GetConfigAppsetting(fileName, "appsecret");
            appsecret = string.IsNullOrWhiteSpace(appid) ? "10135" : appsecret;
        }
        public bool EJiaValidateUser(string ticket, out string userid)
        {
            userid = string.Empty;
            var validateuserhost = string.Empty;
            var appid = string.Empty;
            var appsecret = string.Empty;
            GetEJiaBaseMsg(out validateuserhost, out appid, out appsecret);
            string token = string.Empty;
            if (!EJiaTokenGet(out token, validateuserhost, appid, appsecret))
            {
                return false;
            }
            string validateuserurl = "http://" + validateuserhost + "/openauth2/api/getcontext?ticket=" + ticket + "&access_token=" + token;
            var request = (HttpWebRequest)WebRequest.Create(validateuserurl);
            System.Net.ServicePointManager.DefaultConnectionLimit = 50;
            request.Method = "GET";
            request.Accept = "*/*";
            request.ContentType = "application/json";
            request.KeepAlive = false;
            request.UserAgent = "Mozilla/4.0(compatible;MSIE 8.0;Windows NT 6.1;.NET CLR 1.0.3705;)";
            var response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                System.IO.Stream resStream = response.GetResponseStream();
                System.IO.StreamReader streamReader = new System.IO.StreamReader(resStream, System.Text.Encoding.UTF8);
                string content = streamReader.ReadToEnd();
                streamReader.Close();
                resStream.Close();
                JObject jo = JObject.Parse(content);
                if (jo["mobile"] != null)
                {
                    userid = jo["mobile"].ToString();
                    return true;
                }
            }
            return false;
        }

        private string GetConfigAppsetting(string path, string keyName)
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = path;
            System.Configuration.Configuration cfg = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            string configValue = string.Empty;
            if (cfg.AppSettings.Settings[keyName] != null)
                configValue = cfg.AppSettings.Settings[keyName].Value;
            return configValue;
        }
        #endregion

        public static DBConnectionStringBuilder GetAcountDBConnectString(string svrName, string database, out string pubConn, out string userConn)
        {
            var dbbuilder = new DBConnectionStringBuilder();
            string result;

            if (string.IsNullOrWhiteSpace(svrName))
            {
                pubConn = dbbuilder.GetMainConnStringElement(0, out result, false);//取第一个（默认）服务器
            }
            else
            {
                pubConn = dbbuilder.GetMainConnStringElement(svrName, out result);
            }
            if (string.IsNullOrWhiteSpace(database))
            {
                userConn = dbbuilder.GetDefaultConnString();//取默认连接串
            }
            else
                userConn = dbbuilder.GetAccConnstringElement(svrName, "NG" + database.Replace("NG", ""), pubConn, out result);
            return dbbuilder;
        }

    }
}
