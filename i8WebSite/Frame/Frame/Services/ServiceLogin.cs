using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using NG3;
//using i6.UIProcess.MainFrame;
using System.Data;
using NG3.Data.Service;
using NG3.ESB;
using NG3.Base;

namespace NG3.SUP.Frame
{
    [ESBConfig(InvokeLevel = ESBInvokeLevel.Public)]
    public class ServiceLogin : ServiceBase<ServiceLogin>
    {
        private const string UPAppInfoNameInSession = "NGWebAppInfo";

        protected override object Run(object[] ps)
        {
            #region 提交数据初始
            string Product = ConfigurationManager.AppSettings["Product"];
            //数据库服务器
            string svrName = ps.GetOrDefault<string>(0);
            //帐套
            string account = "NG" + ps.GetOrDefault<string>(1);
            //用户名
            string logid = ps.GetOrDefault<string>(2);
            //密码
            string pwd = string.IsNullOrEmpty(ps.GetOrDefault<string>(3)) ? string.Empty : ps.GetOrDefault<string>(3);
            string pubConn = DataBaseConfigcs.GetPubConnString(svrName);
            string userConn = DataBaseConfigcs.GetUserConnString(svrName, account);
            string RealPwd = string.Empty;
            string UseGroup = string.Empty;
            string UserOcode = string.Empty;
            string UserName = string.Empty;
            #endregion

            string PostPwdWithEn = NG3.NGEncode.EncodeMD5(pwd);
            //校验用户
            DataTable UserDt = DbHelper.GetDataTable(userConn, string.Format("select logid,u_name,pwd,ocode,usergroup from secuser where logid='{0}' ", logid));

            if (UserDt.Rows.Count == 0)
            {
                //检测用户是否存在
                this.SetErrMsg(ps, "不存在该用户!");
                return false;
            }
            else
            {
                RealPwd = UserDt.Rows[0]["pwd"] == DBNull.Value ? string.Empty : UserDt.Rows[0]["pwd"].ToString();
                UseGroup = UserDt.Rows[0]["usergroup"] == DBNull.Value ? string.Empty : UserDt.Rows[0]["usergroup"].ToString().ToUpper();
                UserOcode = UserDt.Rows[0]["ocode"] == DBNull.Value ? string.Empty : UserDt.Rows[0]["ocode"].ToString();
                UserName = UserDt.Rows[0]["u_name"] == DBNull.Value ? string.Empty : UserDt.Rows[0]["u_name"].ToString();
                if (!RealPwd.Equals(PostPwdWithEn))
                {
                    this.SetErrMsg(ps, "用户密码错误!");
                    return false;
                }

                if (string.IsNullOrEmpty(UseGroup) && !UseGroup.Equals("M") && !UseGroup.Equals("S") && !UseGroup.Equals("T"))
                {
                    this.SetErrMsg(ps, "该用户不隶属于任何角色，请联系管理员!");
                    return false;
                }
            }

            I6WebAppInfo appInfo = new I6WebAppInfo();
            appInfo.PubConnectString = pubConn;
            appInfo.UserConnectString = userConn;
            appInfo.LoginID = logid;
            appInfo.UserName = UserName;
            appInfo.OCode = UserOcode;
            appInfo.UserGroup = UseGroup;

            HttpContext.Current.Session[UPAppInfoNameInSession] = appInfo;
            NG3.Data.Service.ConnectionInfoService.SetSessionConnectString(appInfo.UserConnectString);//初始化2.0的dbhelper

             return true;
        }

        /// <summary>
        /// 登录验证操作
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="servername">数据库服务名</param>
        /// <param name="account">选择帐套名</param>
        /// <param name="error">返回的错误信息记录</param>
        /// <returns>true:表示登录成功；false 表示登录失败</returns>
        protected  object OldRun(object[] ps)
        {
            //by weizj

            //#region 参数1 serverIndex  可以传入null, serverName或index
            //int serverIndex;
            //if (ps[0] == null)
            //    serverIndex = GetDefaultDBSelectIndex();
            //else if (ps[0] is int)
            //    serverIndex = (int)ps[0];
            //else
            //    serverIndex = GetServerIndex(ps[0] as string);
            //#endregion
            //string account = ps.GetOrDefault<string>(1, GetDefaultAccName());
            //string username = ps.GetOrDefault<string>(2);
            //string errMsg = string.Empty;
            //string pwd = ps.GetOrDefault<string>(3, string.Empty);
            //if (pwd != null && pwd.Length == 128)
            //{
            //    pwd = NGEncode.DecodePassword(pwd, 128);
            //}

            //#region 初始化工作
            ////PubConnectStr
            //var pubConnStr = UIP.Login.GetMainConnStringElement(serverIndex, out errMsg);
            ////var encodePubConnStr = NGEncode.EncodePassword(pubConnStr);
            ////UserConnectStr
            //var dbName = this.GetDbName(pubConnStr, account);
            //var userConnStr = UIP.Login.GetAccConnstringElement(serverIndex, dbName, pubConnStr, out errMsg);
            ////var encodeUserConnStr = NGEncode.EncodePassword(userConnStr);

            //UIP.Login.SetUpSession(userConnStr);

            //NG2.Data.Service.ConnectionInfoService.SetSessionConnectString(userConnStr);//2.0的

            //#region SetAppInfo

            //I6WebAppInfo myAppInfo = new I6WebAppInfo();

            //myAppInfo.PubConnectString = pubConnStr;
            //myAppInfo.UserConnectString = userConnStr;
            //myAppInfo.LoginID = username;
            //myAppInfo.UserName = username;
            //myAppInfo.OCode = "001";

            //HttpContext.Current.Session[UPAppInfoNameInSession] = myAppInfo;

            //return true;

            //#endregion

            //#endregion

            //var userKind = UIP.Login.IsRealUser(username, pubConnStr);//判断是否存在该用户
            //if (userKind == 0)
            //{
            //    this.SetErrMsg(ps, "不存在该用户!");
            //    return false;
            //}

            //if (userKind == 1)
            //{
            //    #region 普通用户
            //    var syserrtimes = UIP.Login.GetSysErrTimes();
            //    var getErrorTimes = UIP.Login.GetErrTimes(username, pubConnStr);
            //    if (syserrtimes > getErrorTimes)
            //    {
            //        //用户名密码是否正确
            //        bool loginSuccess = false;
            //        try
            //        {
            //            loginSuccess = UIP.Login.Login(account, username, pwd);
            //        }
            //        catch (Exception ex)
            //        {
            //            if (ex.Message.IndexOf("错误口令") >= 0)//硬编码不需要翻译
            //            {
            //                //登录失败
            //                UIP.Login.AddErrTimes(username, pubConnStr);
            //                int times = syserrtimes - getErrorTimes - 1;
            //                this.SetErrMsg(ps, string.Format("登录失败，密码不正确，您还有{0}次登录机会", times.ToString()));
            //            }
            //            else
            //            {
            //                this.SetErrMsg(ps, ex.Message);
            //            }
            //        }

            //        if (loginSuccess)
            //        {
            //            #region 普通用户，需要组织控制("GE"和“A3”另外操作-)

            //            WinformLoginService winlogserv = new WinformLoginService();
            //            var orgCode = winlogserv.GetLoginOcode(userConnStr, username);
            //            if (!string.IsNullOrEmpty(orgCode))
            //            {
            //                //webservice对应的服务端登录
            //                ESB.UP.WinLoginService.Invoke("SimulateWebLogin", account, orgCode, username, serverIndex, "OrgUser", string.Empty);//用webservice登录                              
            //                WebServiceUIP webUIP = new WebServiceUIP(account, orgCode, username, serverIndex);
            //                webUIP.SetKernelSession();
            //                webUIP.SetAppInfo();
            //            }
            //            else
            //            {
            //                this.SetErrMsg(ps, "用户没有组织号！");
            //                return false;
            //            }
            //            #endregion

            //            #region onlineuser

            //            #endregion

            //            UIP.Login.ClearErr(username, userConnStr);

            //            return true;
            //        }
            //        else if (syserrtimes - getErrorTimes - 1 < 0)
            //        {
            //            //锁定用户
            //            UIP.Login.SetUserStateOff(username);
            //            this.SetErrMsg(ps, "您的帐号已经被锁定，请与管理员联系");
            //        }

            //        return false;
            //    }
            //    #endregion
            //}
            //else if (userKind == 2)
            //{
            //    #region 系统管理员
            //    errMsg = "系统管理员登录未实现！";
            //    this.SetErrMsg(ps, "系统管理员登录未实现！");
            //    #endregion
            //}

            return false;
        }

        private void SetErrMsg(object[] ps, string msg)
        {
            ps.SetValue(msg, 4);
        }

        internal static string GetDefaultAccName()
        {
            //by weizj

            string defAcc = null;
            //var dt = DataBaseXML.DataBaseConfig.Tables["AccConfig"];
            //if (dt.Rows.Count > 0 && dt.Columns.Count > 0)
            //{
            //    defAcc = Convert.ToString(dt.Rows[0][0]);
            //}
            return defAcc;
        }

        internal static int GetDefaultDBSelectIndex()
        {
            //by weizj

            var index = -1;
            //var dt = DataBaseXML.DataBaseConfig.Tables["ConnectDB"];
            //if (dt.Rows.Count > 0 && dt.Columns.Count > 0)
            //{
            //    index = dt.Rows[0][0].TryParseToInt();
            //}

            return index;
        }

        #region GetServerIndex 服务器所在数据表的索引值
        /// <summary>
        /// 服务器所在数据表的索引值
        /// 为了兼容web的方式获取pubconnetstr和userconnectstr
        /// </summary>
        /// <param name="servername"></param>
        /// <returns></returns>
        internal static int GetServerIndex(string servername)
        {
            //by weizj

            int i = 0;
            //if (!string.IsNullOrEmpty(servername))
            //{
            //    DataSet ds = DataBaseXML.DataBaseConfig;
            //    string srvName = servername.ToLower();
            //    DataTable dt = ds.Tables["Connect"];
            //    int count = dt.Rows.Count;
            //    for (i = 0; i < count; i++)
            //    {
            //        if (dt.Rows[i]["ByName"].ToString().EqualsIgnoreCase(srvName)) break;
            //    }
            //}
            return i;
        }
        #endregion

        #region GetDbName 根据账套号得到账套数据库
        /// <summary>
        /// 根据账套号得到账套数据库
        /// </summary>
        /// <param name="ucode">账套号</param>
        /// <returns>账套数据库</returns>
        private string GetDbName(string pubConnStr, string ucode)
        {
            //by weizj

            //var dt = UIP.Login.GetAccInfo(pubConnStr);

            //foreach (DataRow dr in dt.Rows)
            //{
            //    if (Convert.ToString(dr["ucode"]) == ucode)
            //    {
            //        return Convert.ToString(dr["dbname"]);
            //    }
            //}
            return null;
        }
        #endregion


    }

   
}