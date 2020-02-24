using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using NG3.Data.Service;
using SUP.Common.Base;
using SUP.Frame.DataAccess;
using SUP.Common.DataAccess;
using GE.BusinessRules.Common;
using SUP.Frame.DataEntity;
using Newtonsoft.Json;
using GE.DataAccess.Common;

namespace SUP.Frame.Rule
{
    public class LoginRule
    {
        private const string UPAppInfoNameInSession = "NGWebAppInfo";
        private UserOnlineInfor instance = UserOnlineInfor.Instance;
        private LoginDac dac = null;
        private Common.DataAccess.LangInfo langDac = null;
        public LoginRule()
        {
            dac = new LoginDac();
            langDac = new Common.DataAccess.LangInfo();
        }

        public void Check(ref string msg, ref bool loginflag, string svrName, string account, string logid, string pwd)
        {
            string result;
            DBConnectionStringBuilder dbbuilder = new DBConnectionStringBuilder();
            string pubConn = string.Empty;
            string userConn = string.Empty;


            if (string.IsNullOrWhiteSpace(svrName))
            {
                pubConn = dbbuilder.GetMainConnStringElement(0, out result, false);//取第一个（默认）服务器
            }
            else
            {
                pubConn = dbbuilder.GetMainConnStringElement(svrName, out result);
            }

            if (string.IsNullOrWhiteSpace(account))
            {
                userConn = dbbuilder.GetDefaultConnString();//取默认连接串
            }
            else
            {
                userConn = dbbuilder.GetAccConnstringElement(svrName, account, pubConn, out result);
            }

            I6WebAppInfo appInfo = new I6WebAppInfo();
            appInfo.UserType = UserType.OrgUser;


            #region 用户状态

            string sql = "select status from secuser where logid='" + logid + "'";
            string ret = DbHelper.GetString(userConn, sql);

            if (ret == "1")
            {
                msg = "用户[" + logid + "]已锁定，请联系系统管理员!";
                loginflag = false;
                return;
            }

            int sysErrortimes = this.dac.GetSysErrTimes(userConn);//系统定义出错次数
            int currentErrorTimes = this.dac.GetErrTimes(logid, userConn);

            if (currentErrorTimes == sysErrortimes)
            {
                this.dac.SetUserStateOff(logid, userConn);//锁定用户
                msg = "用户[" + logid + "]已锁定，请联系系统管理员!";
                loginflag = false;
                return;
            }

            #endregion

            string username = string.Empty;
            //校验用户
            object obj = DbHelper.ExecuteScalar(userConn, string.Format("select count(logid) from secuser where logid='{0}'", logid));
            if (obj == null || obj == DBNull.Value || obj.ToString() == "0")
            {
                //检测系统管理员
                obj = DbHelper.ExecuteScalar(pubConn, string.Format("select count(cname) from ngrights where cname='{0}'", logid));
                if (obj == null || obj == DBNull.Value || obj.ToString() == "0")
                {
                    //this.SetErrMsg(ps, "不存在该用户!");
                    //return false;

                    msg = "用户名或密码错误！";//"不存在该用户!";
                    loginflag = false;
                    this.dac.AddErrTimes(logid, userConn);
                    return;
                }
                else
                {
                    appInfo.UserType = SUP.Common.Base.UserType.System;
                }
                username = logid;
            }
            else
            {
                string usernameSql = string.Format("select u_name from secuser where logid='{0}'", logid);
                username = DbHelper.GetString(userConn, usernameSql);
            }


            #region 校验密码


            if (UserType.OrgUser == appInfo.UserType)
            {
                obj = DbHelper.ExecuteScalar(userConn, string.Format("select pwd from secuser where logid='{0}'", logid));
            }
            else
            {
                obj = DbHelper.ExecuteScalar(pubConn, string.Format("select cpwd from ngrights where cname='{0}'", logid));
            }

            if (string.IsNullOrEmpty(pwd))
            {
                if (obj != null && obj != DBNull.Value)
                {
                    if (obj.ToString().Length > 0)
                    {
                        msg = "用户名或密码错误！";//"密码不正确";
                        loginflag = false;
                        this.dac.AddErrTimes(logid, userConn);
                        return;
                    }
                }
            }
            else
            {
                if (obj == null || obj == DBNull.Value)
                {
                    msg = "用户名或密码错误！"; //"密码不正确";
                    loginflag = false;
                    this.dac.AddErrTimes(logid, userConn);
                    return;
                }
                else
                {
                    string dbpwd = NG3.NGEncode.DecodePassword(obj.ToString(), 128);
                    if (dbpwd.Equals(pwd) == false)
                    {
                        msg = "用户名或密码错误！";//"密码不正确";
                        loginflag = false;
                        this.dac.AddErrTimes(logid, userConn);
                        return;
                    }
                }
            }

            #endregion

            //错误次数清零
            this.dac.ClearErr(logid, userConn);

            //普通用户，获取组织
            string ocode = string.Empty;
            if (UserType.System != appInfo.UserType)
            {
                ocode = DbHelper.ExecuteScalar(userConn, string.Format("select lastloginorg from secuser where logid='{0}'", logid)).ToString();

                if (string.IsNullOrWhiteSpace(ocode))
                {
                    sql = "select ocode from fg_orglist";
                    DataTable dt = DbHelper.GetDataTable(userConn, sql);

                    if (dt.Rows.Count > 0)
                    {
                        ocode = dt.Rows[0]["ocode"].ToString();//取第一个组织作为默认组织
                    }
                }
            }

            #region 在线用户

            //string message = this.CheckUserOnline(logid, account, string.Empty);
            string message = this.CheckTheSameSessionUser(logid);
            if (!string.IsNullOrEmpty(message))
            {
                msg = message;
                loginflag = false;
                return;
            }

            #endregion

            appInfo.PubConnectString = pubConn;
            appInfo.UserConnectString = userConn;
            appInfo.LoginID = logid;
            appInfo.UserName = username;
            appInfo.OCode = ocode;
            appInfo.UCode = account;
            appInfo.UserID = Convert.ToInt64(DbHelper.GetString(userConn, string.Format("select phid from fg3_user where userno='{0}'", logid)));
            appInfo.OrgID = Convert.ToInt64(DbHelper.GetString(userConn, string.Format("select phid from fg_orglist where ocode='{0}'", ocode)));

            System.Web.HttpContext.Current.Session[UPAppInfoNameInSession] = appInfo;
            NG3.Data.Service.ConnectionInfoService.SetSessionConnectString(appInfo.UserConnectString);//初始化2.0的dbhelper
        }

        #region web登录代码归并
        /// <summary>
        /// 校验用户
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="loginflag"></param>
        /// <param name="svrName"></param>
        /// <param name="database"></param>
        /// <param name="logid"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public string WebCheck(out string msg, out bool loginflag, string svrName, string database, string logid, string pwd, I6WebAppInfo appInfo, string isOnlineCheck)
        {
            msg = string.Empty;
            string pubConn = string.Empty;
            string userConn = string.Empty;
            var dbbuilder = LoginDac.GetAcountDBConnectString(svrName, database, out pubConn, out userConn);
            appInfo.UserType = UserType.OrgUser;

            #region 用户状态

            string sql = "select status from fg3_user where userno = '" + logid + "'";
            string ret = DbHelper.GetString(userConn, sql);

            if (ret == "3")
            {
                msg = "用户[" + logid + "]已锁定，请联系系统管理员!";
                loginflag = false;
                return string.Empty;
            }

            int sysErrortimes = dac.GetSysErrTimes(userConn);//系统定义出错次数
            int currentErrorTimes = dac.GetErrTimes(logid, userConn);

            if (currentErrorTimes == sysErrortimes)
            {
                dac.SetUserStateOff(logid, userConn);//锁定用户
                msg = "用户[" + logid + "]已锁定，请联系系统管理员!";
                loginflag = false;
                return string.Empty;
            }

            #endregion

            #region 校验用户

            string username = string.Empty;            
            object obj = DbHelper.ExecuteScalar(userConn, string.Format("select count(userno) from fg3_user where userno = '{0}'", logid));
            if (obj == null || obj == DBNull.Value || obj.ToString() == "0")
            {
                //检测系统管理员
                obj = DbHelper.ExecuteScalar(pubConn, string.Format("select count(cname) from ngrights where cname = '{0}'", logid));
                if (obj == null || obj == DBNull.Value || obj.ToString() == "0")
                {
                    msg = "不存在该用户!";
                    loginflag = false;
                    return string.Empty;
                }
                else
                {
                    appInfo.UserType = UserType.System;
                }
                username = logid;
            }
            else
            {
                string usernameSql = string.Format("select username from fg3_user where userno = '{0}'", logid);
                username = DbHelper.GetString(userConn, usernameSql);
            }

            #endregion

            #region 校验密码

            if (UserType.OrgUser == appInfo.UserType)
            {
                obj = DbHelper.ExecuteScalar(userConn, string.Format("select pwd from fg3_user where userno = '{0}'", logid));
            }
            else
            {
                obj = DbHelper.ExecuteScalar(pubConn, string.Format("select cpwd from ngrights where cname = '{0}'", logid));
            }

            if (string.IsNullOrEmpty(pwd))
            {
                if (obj != null && obj != DBNull.Value)
                {
                    if (obj.ToString().Length > 0)
                    {
                        msg = "密码不正确！";
                        loginflag = false;
                        return string.Empty;
                    }
                }
            }
            else
            {
                if (obj == null || obj == DBNull.Value)
                {
                    msg = "密码不正确！";
                    loginflag = false;
                    return string.Empty;
                }
                else
                {
                    string dbpwd = NG3.NGEncode.DecodePassword(obj.ToString(), 128);
                    if (dbpwd.Equals(pwd) == false)
                    {
                        msg = "密码不正确！";
                        loginflag = false;
                        return string.Empty;
                    }
                }
            }

            //错误次数清零
            dac.ClearErr(logid, userConn);

            #endregion

            #region 系统维护通知

            if (appInfo.UserType != UserType.System)
            {
                DataTable dt = PubCommonDac.Instance.GetSysMaintainCall(pubConn);
                if (dt.Rows.Count > 0)
                {
                    string userID = PubCommonDac.Instance.GetUserId(userConn, logid);
                    string allowlogin = dt.Rows[0]["allowlogin"].ToString();
                    string[] allowlogins = allowlogin.Split(';');
                    bool flag = true;
                    string ucode = !string.IsNullOrWhiteSpace(database) ? database : new DBConnectionStringBuilder().DefaultDB.Replace("NG", "");
                    for (int i = 0; i < allowlogins.Length - 1; i++)
                    {
                        if (ucode == allowlogins[i].Split('|')[0] && userID == allowlogins[i].Split('|')[1])
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        msg = "系统维护中，预计维护结束时间为" + dt.Rows[0]["preenddate"] + "。" + dt.Rows[0]["runinfo"];
                        loginflag = false;
                        return string.Empty;
                    }
                }
            }

            #endregion

            #region 普通用户，获取组织
            string ocode = string.Empty;
            string oname = string.Empty;
            var loginorg = string.Empty;
            if (UserType.System != appInfo.UserType)
            {
                var strLoginOrg = DbHelper.ExecuteScalar(userConn, string.Format("select lastloginorg from fg3_user where userno = '{0}'", logid));
                loginorg = strLoginOrg == null ? string.Empty : strLoginOrg.ToString();
            }
            if (!string.IsNullOrWhiteSpace(loginorg) && loginorg != "0")
            {
                sql = $"select ocode,oname from fg_orglist where phid = {loginorg}";
                DataTable dt = DbHelper.GetDataTable(userConn, sql);

                if (dt.Rows.Count > 0)
                {
                    ocode = dt.Rows[0]["ocode"].ToString();//取第一个组织作为默认组织
                    oname = dt.Rows[0]["oname"].ToString();
                }
            }
            else
            {
                msg = appInfo.UserType == UserType.System ? "" : "UserNoHaveOrg";//找不到用户最后一次的登录
            }
            #endregion

            #region 在线用户

            string message = CheckTheSameSessionUser(logid);
            if (!string.IsNullOrEmpty(message))
            {
                msg = message;
                loginflag = false;
                return string.Empty;
            }

            string uCode = !string.IsNullOrWhiteSpace(database) ? database : dbbuilder.DefaultDB;
            if (isOnlineCheck != "1")
            {               
                message = CheckUserOnline(logid, uCode.Replace("NG", ""), string.Empty);
                if (!string.IsNullOrEmpty(message))
                {
                    msg = message;
                    loginflag = false;
                    return string.Empty;
                }
            }

            #endregion

            appInfo.PubConnectString = pubConn;
            appInfo.UserConnectString = userConn;
            appInfo.LoginID = logid;
            appInfo.UserName = username;
            appInfo.OCode = ocode;
            appInfo.OrgName = oname;
            appInfo.UCode = uCode;
            appInfo.UserID = appInfo.UserType == UserType.System ? 0 : Convert.ToInt64(DbHelper.GetString(userConn, string.Format("select phid from fg3_user where userno = '{0}'", logid)));
            long orgid = 0;
            long.TryParse(loginorg, out orgid);
            appInfo.OrgID = orgid;
            loginflag = true;
            return userConn;
        }

        #endregion

        public DataTable GetOnlineUserDt()
        {
            return instance.GetDt();
        }

        public string CheckUserOnline(string logid, string ucode, string macAddress)
        {
            DataTable onlineUsers = GetOnlineUserDt();
            DataTable loginedUser = onlineUsers.Clone();
            //DataRow[] drs = onlineUsers.Select("UserID = '" + logid + "' and UName = '" + curAccName + "' and MacAddress <> '" + macAddress + "'");
            DataRow[] drs = onlineUsers.Select("UserID = '" + logid + "' and UCode = '" + ucode + "'");
            if (drs.Length > 0)
            {
                loginedUser.Rows.Add(drs[0].ItemArray);
            }

            string msg = string.Empty;
            if (loginedUser != null && loginedUser.Rows.Count > 0)
            {
                string ipAddress = loginedUser.Rows[0]["IPAddress"].ToString();
                OnlineUsersValidateEntity entity = new OnlineUsersValidateEntity();
                entity.Message = String.Format("您使用的用户名已在[{0}]登录，是否强行登录并清除[{1}]上的登录信息？", ipAddress, ipAddress);
                entity.IpAddress = ipAddress;
                entity.Devicetype = loginedUser.Rows[0]["DeviceType"].ToString();
                entity.UserId = loginedUser.Rows[0]["UserId"].ToString();
                entity.SessionID = loginedUser.Rows[0]["SessionID"].ToString();
                msg = JsonConvert.SerializeObject(entity);
                HttpContext.Current.Session["LoginedUserDt"] = loginedUser;
            }
            return msg;
        }

        //检测同一会话中是否存在其他用户,同一个浏览器登录多个用户存在这种情况
        public string CheckTheSameSessionUser(string logid)
        {
            DataTable onlineUsers = GetOnlineUserDt();
            DataRow[] drs = onlineUsers.Select("UserID <> '" + logid + "' and SessionID = '" + HttpContext.Current.Session.SessionID + "'");
            foreach (DataRow dr in drs)
            {
                string user = dr["UserID"].ToString();
                //this.KillLoginUser(user);//剔除同一会话中的其他用户
                return "用户[" + user + "]已经登录当前浏览器的会话中";
            }
            return string.Empty;
        }

        /// <summary>
        /// 设置登录历史，注册在线用户
        /// </summary>
        /// <returns></returns>
        public bool SetLoginUsers(string macAddress, ref string guid, ref string msg)
        {            
            string product = new ProductInfo().ProductCode;           
            if (instance.CheckValidUser(product, "")) //检测
            {
                if (instance.SetLoginUsers(product, "", macAddress)) //注册
                {
                    guid = instance.AddLogInHistory();
                    return true;
                }
                else
                {
                    msg = "超过最大用户数";
                    return false;
                }
            }
            return false;
        }

        public void LoginOut(string code)
        {
            dac.SetLoginUserOut(code);
            instance.KillLoginUser(HttpContext.Current.Session.SessionID);
        }

        /// <summary>
        /// 剔除用户
        /// </summary>
        public void KillLoginUser()
        {
            instance.KillLoginUser(HttpContext.Current.Session.SessionID);
        }

        /// <summary>
        /// 根据业务类型获取多语言
        /// </summary>
        /// <param name="busiType"></param>
        /// <param name="svrName"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetLabelLang(string busiType, string svrName, string account)
        {
            this.SetSessionConnectString(svrName, account);
            return Common.DataAccess.LangInfo.GetLabelLang(busiType);
        }

        public string GetLangInfo(string svrName, string account)
        {
            this.SetSessionConnectString(svrName, account);
            return dac.GetLangInfo();
        }

        public string GetLoginIDByMobileOrEmail(string svrName, string account, string userName)
        {
            this.SetSessionConnectString(svrName, account);
            return dac.GetLoginIDByMobileOrEmail(userName);
        }

        public void SetSessionConnectString(string svrName, string account)
        {
            string result;
            DBConnectionStringBuilder dbbuilder = new DBConnectionStringBuilder();
            string pubConn = string.Empty;
            string userConn = string.Empty;

            if (string.IsNullOrWhiteSpace(svrName))
            {
                pubConn = dbbuilder.GetMainConnStringElement(0, out result, false);//取第一个（默认）服务器
            }
            else
            {
                pubConn = dbbuilder.GetMainConnStringElement(svrName, out result);
            }

            //数据库名不完整，缺少NG 
            if (account.Length > 0 && account.ToLower().IndexOf("ng") != 0)
            {
                account = "NG" + account;
            }
            userConn = dbbuilder.GetAccConnstringElement(svrName, account, pubConn, out result);
            NG3.Data.Service.ConnectionInfoService.SetSessionConnectString(userConn);//初始化2.0的dbhelper
        }


        public string GetConnectString(string svrName, string account)
        {
            string result;
            DBConnectionStringBuilder dbbuilder = new DBConnectionStringBuilder();
            string pubConn = string.Empty;
            string userConn = string.Empty;

            if (string.IsNullOrWhiteSpace(svrName))
            {
                pubConn = dbbuilder.GetMainConnStringElement(0, out result, false);//取第一个（默认）服务器
            }
            else
            {
                pubConn = dbbuilder.GetMainConnStringElement(svrName, out result);
            }

            //数据库名不完整，缺少NG 
            if (account.Length > 0 && account.ToLower().IndexOf("ng") != 0)
            {
                account = "NG" + account;
            }
            userConn = dbbuilder.GetAccConnstringElement(svrName, account, pubConn, out result);
            NG3.Data.Service.ConnectionInfoService.SetSessionConnectString(userConn);//初始化2.0的dbhelper
            return userConn;
        }

        public string GetAttachid(string src)
        {
            return dac.GetAttachid(src);
        }

        /// <summary>
        /// 获取登录主框架多语言信息
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetPortalListBarLang(string language, string conn)
        {
            try
            {
                //System.Web.HttpContext.Current.Session["ClientLanguage"] = string.IsNullOrEmpty(language) ? "zh-CN" : language;
                //this.SetSessionConnectString(svrName, account);
                //NG3.Data.Service.ConnectionInfoService.SetSessionConnectString(conn);
                return Common.DataAccess.LangInfo.GetLabelLangWithConn(language, conn, "PortalListBar");
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
