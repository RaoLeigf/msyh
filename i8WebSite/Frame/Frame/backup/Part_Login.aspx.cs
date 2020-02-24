using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NG2;
using NG2.Data;
using Newtonsoft.Json.Linq;
using NG2.Web.UI;
using System.Data;
using System.Net;
using System.Text;
using System.Management;

#region 原i6中动态库
//using NG.UP.Kernel.Client;
//using i6.Help;
//using i6.Help.Hardware;

#endregion

#region 添加
using System.Runtime.InteropServices;
using NG2.WCached.Client;//Dillimport
#endregion

namespace Frame
{
    public partial class Part_Login : NG2Page
    {
        /*
        #region 叶俊强源码 （先注释）
        
        private DataSet GetDataBaseConfig()
        {
            ESBData d = ESB.UP.WinLoginService.Invoke("GetDataBaseConfig");

            return Ajax.DealAndGetValue(d) as DataSet;
        }		 
        #endregion
        */

        private static readonly i6.UIProcess.MainFrame.loginUIP loginUIP = new i6.UIProcess.MainFrame.loginUIP(); 
        private static WCacheValue<string> Product;//服务器端获取产品名信息
        

        #region 重写 OnInit
        protected override void OnPreInit(EventArgs e)
        {
            //服务器端产品信息
            Product = WCache.CreateInProcCache<string>(() =>
            {
                ESBData d = ESB.UP.WinLoginService.Invoke("GetProduct");
                return Ajax.DealAndGetValue(d) as string;
            }, DateTime.Now.AddDays(1));

            base.OnPreInit(e);
        }
        #endregion

        #region 初始化加载 PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            //var aa = ESB.UP.CallFacade("i6.BusinessFacade.MainFrame.LoginFacade.isLoginUSB", "01");
            //var a = 1;
        }
        #endregion

        #region ajax服务方法

        #region 测试调用 businessProxy
        //private DataTable test()
        //{
        //    //var alluer = ESB.UP.CallFacade("GE.BusinessFacade.Common.UserFacade.GetAllUsers");
        //    //return Ajax.DealAndGetValue(alluer) as DataTable;
        //}
        #endregion

        #region Login
        [UrlMethod, UrlSecurity(UrlSecurity.Everyone)]
        public void Login()
        {

            var fm = this.RequestMeta.AsFormMeta();
            string msg = "";
            bool islogin = LoginProcess(fm.Get("EditUser"), fm.Get("EditPass"), fm.Get("DDLServer"), fm.Get("DDLAcc"), ref msg);
            if (islogin)
            {
                //DataTable dt = test();
                if (msg == string.Empty)
                {
                    Ajax.ShowMessage(AjaxType.Succ);
                }
                else
                {
                    Ajax.ShowMessage(AjaxType.Succ, msg);
                }

            }
            else
            {
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

        #region 服务器列表
        [UrlMethod, UrlSecurity(UrlSecurity.Everyone)]
        public void GetServerList()
        {
            DataSet ds = ESB.UP.WinLoginService.Invoke("GetDataBaseConfig").Value as DataSet;
            Ajax.WriteRaw(ds.Tables["Connect"].ToJSON("byname", "servername"));
        }
        #endregion

        #region 获取帐套列表
        //Ajax.WriteRaw("{Record:[{id:\"MY97NB001\",Name:\"MY97NB001\"},{id:\"MY97NB002\",Name:\"MY97NB002\"}]}");
        [UrlMethod, UrlSecurity(UrlSecurity.Everyone)]
        public void GetAccList(string servername)
        {
            var d = this.GetAccInfo(servername);
            Ajax.WriteRaw(d.ToJSON("ucode", "uname"));
        }
        #endregion
        #endregion

        #region 私有方法

        #region 服务器所在数据表的索引值
        /// <summary>
        /// 服务器所在数据表的索引值
        /// 为了兼容web的方式获取pubconnetstr和userconnectstr
        /// </summary>
        /// <param name="servername"></param>
        /// <returns></returns>
        private int GetServerIndex(string servername)
        {
            DataSet ds = ESB.UP.WinLoginService.Invoke("GetDataBaseConfig").Value as DataSet; ;
            int i;
            string srvName = servername.ToLower();
            DataTable dt = ds.Tables["Connect"];
            int count = dt.Rows.Count;
            for (i = 0; i < count; i++)
            {
                if (dt.Rows[i]["ByName"].ToString() == srvName) break;
            }
            return i;
        }
        #endregion

        #region 登录验证操作
        /// <summary>
        /// 登录验证操作
        /// </summary>
        /// <param name="uname">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="servername">数据库服务名</param>
        /// <param name="account">选择帐套名</param>
        /// <param name="error">返回的错误信息记录</param>
        /// <returns>true:表示登录成功；false 表示登录失败</returns>
        private bool LoginProcess(string uname, string pwd, string servername, string account, ref string error)
        {

            #region 用到的变量
            string _userConnectStr;
            string _publicConnectStr;//公共链接串
            string AuthenticationInfoID;//kernclient.AuthenticationInfoID
            string _orgCode;//组织控制
            #endregion



            string result = "";

            #region 取_userConnectStr 链接串
            int serverindex = 0;
            if (!string.IsNullOrEmpty(servername))
            {

                serverindex = this.GetServerIndex(servername);//数据库服务器名在database.xml文件中的索引
            }
            _publicConnectStr = ESB.UP.WinLoginService.InvokeWithMessage("GetMainConnStringElement", 1, serverindex, result).Value as string;
            //用户连接串，帐套连接串
            _userConnectStr = ESB.UP.WinLoginService.Invoke("GetAccConnstringElement",
                                     serverindex,
                                     account,
                                     _publicConnectStr,
                                     uname)
                               .Value as string;
            #endregion

            //验证ip合法
            var checkIP = ESB.UP.WinLoginService.InvokeWithMessage("CheckLegalIP", 1, _userConnectStr, result);
            if (!checkIP.Value.TryParseToBool())
            {
                error = "IP 地址不合法，请与管理员联系！";//显示出错信息
                return false;
            }
            //检测用户是否存在
            var isrealuser = ESB.UP.WinLoginService.Invoke("IsRealUser", uname, _publicConnectStr, _userConnectStr);
            int realuser = isrealuser.Value.TryParseToInt();
            if (realuser == 0)
            {
                error = "系统不存在此用户！";
                return false;
            }
            #region 处理组织信息（这里暂时不考虑）--参照i6登录

            #endregion

            #region 普通用户
            if (realuser == 1)
            {

                //保证服务端的kernelclientproxy 的AuthentionInfoID被初始化
                //就是服务端的kernelclientproxy一次被调用的时候AuthenticationInfoID
                int syserrtimes = ESB.UP.WinLoginService.Invoke("GetSysErrTimes", _userConnectStr).Value.TryParseToInt();

                //注意：这是一个关键操作，取出服务器端的kernelclientproxy 的AuthentionInfoID
                //保持客户端，服务端kernelclientproxy 的AuthentionInfoID一致
                //调用这个方法之前一定要保证先调用一下服务器的代理成（上面的GetSysErrTimes完成调用）就是保证服务器端的kernelclientproxy调用invoke方法
                //在这个方法里面会初始化服务器端的kernelclientproxy的AuthenticationInfoID；
                //初始化好客户端kernelclientproxy的AuthenticationInfoID之后，代理成调用服务端才能通过省份验证
                AuthenticationInfoID = ESB.UP.WinLoginService.Invoke("GetAuthenticationInfoID").Value as string;
                int getErrorTimes = 0;

                getErrorTimes = ESB.UP.WinLoginService.Invoke("GetErrTimes", _userConnectStr, uname, _publicConnectStr).Value.TryParseToInt();//已经出错的次数

                if (syserrtimes > getErrorTimes)
                {

                    //string tempPwd=string.IsNullOrEmpty(pwd)?"":FormsAuthentication.HashPasswordForStoringInConfigFile(pwd,"MD5");
                    //string checkRseulst = ESB.InvokeWebService("i6Service/WinLoginService.asmx", "CheckOtherPwd").Value.ToString();//其他验证

                    //用户名密码是否正确
                    bool islog = false;
                    try
                    {   //在这里若用户名密码出错时，会抛出异常（因此暂时用捕获异常的形式获取）
                        islog = ESB.UP.WinLoginService.Invoke("Login", _userConnectStr, account, uname, pwd).Value.TryParseToBool();
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.IndexOf("错误口令") >= 0)//硬编码不需要翻译
                        {
                            //登录失败
                            ESB.UP.WinLoginService.Invoke("AddErrTimes", _userConnectStr, uname, _publicConnectStr);//增加出错次数
                            int times = (syserrtimes - getErrorTimes - 1);
                            error = string.Format("登录失败，密码不正确，您还有{0}次登录机会", times.ToString());
                        }
                        else if(ex.Message.IndexOf("当前操作员的帐号已被封锁")>=0)//判断不需要翻译
                        {
                            error = "当前操作员的帐号已被封锁";
                        }
                        else if (ex.Message.IndexOf("当前操作员没有登录系统的权限") >= 0)//判断不需要翻译
                        {
                            error = "当前操作员没有登录系统的权限";
                        }
                        else if (ex.Message.IndexOf("此操作员帐户不在有效期中") >= 0)//判断不需要翻译
                        {
                            error = "此操作员帐户不在有效期中";
                        }
                        else
                        {
                            error=ex.Message;
                        }
                        return false;
                    }
                    if (islog)
                    {
                        //登录成功
                        #region USB加密验证（暂时不考虑）

                        #endregion

                        #region 普通用户，需要组织控制("GE"和“A3”另外操作-)
                        _orgCode = ESB.UP.WinLoginService.Invoke("GetLoginOcode", _userConnectStr, uname).Value as string;
                        if (!string.IsNullOrEmpty(_orgCode))
                        {
                            ESB.UP.WinLoginService.Invoke("SimulateWebLogin", account, _orgCode, uname, serverindex, "OrgUser");//用webservice登录  
                            this.InitialKernalProxyClient(AuthenticationInfoID);
                        }
                        else
                        {
                            error = "用户没有组织号！";
                            return false;
                        }
                        #endregion

                        #region 帐套，用户数控制

                        #endregion

                        #region onlineuser
                        string temmsg = string.Empty;
                        string sessionIDKilled = string.Empty;
                        string mac = "00-19-21-C1-71-B2";//这里只是静态获取我自己本地信息
                        object[] args = new object[] 
                        {
                         _userConnectStr,
                        Product.Get() ,
                         uname,
                         account,
                          mac,
                         sessionIDKilled,
                         temmsg
                        };
                        bool pass = true;
                        try
                        {
                           pass=  ESB.UP.WinLoginService.Invoke("OnlineUserCheck", temmsg).Value.TryParseToBool();
                        }
                        catch
                        {
                        }
                        if (!pass)
                        {
                            //自动注销对面的用户
                            ESB.UP.WinLoginService.Invoke("KillLoginUser", sessionIDKilled);
                            //nsserver的在线用户注销
                            ESB.UP.WinLoginService.Invoke("SetLoginUsers", _userConnectStr, Product, string.Empty, mac);
                        }
                        #endregion
                        //记录登录信息
                        try
                        {
                            string LogInHisCode = ESB.UP.WinLoginService.Invoke("AddLogInHistory", _userConnectStr).Value as string;
                        }
                        catch 
                        {
                        }
                        //登录出错次数清零
                        ESB.UP.WinLoginService.Invoke("ClearErr", _userConnectStr, uname, _publicConnectStr);
                        //记录信息
                        AppInfo.SetUserInfo(uname, uname, uname, string.Empty, string.Empty, string.Empty, string.Empty);
                        return true;
                    }
                }
                else
                {
                    //锁定用户
                    ESB.UP.WinLoginService.Invoke("SetUserStateOff", _userConnectStr, uname);
                    error = "您的帐号已经被锁定，请与管理员联系";
                    return false;
                }
            }
            #endregion

            #region 系统管理员
            if (realuser == 2)
            {
                //保证服务端的kernelclientproxy 的AuthentionInfoID被初始化
                //就是服务端的kernelclientproxy一次被调用的时候AuthenticationInfoID
                //int syserrtimes = ESB.InvokeWebService("i6Service/WinLoginService.asmx", "GetSysErrTimes", _userConnectStr).Value.TryParseToInt();

                //注意：这是一个关键操作，取出服务器端的kernelclientproxy 的AuthentionInfoID
                //保持客户端，服务端kernelclientproxy 的AuthentionInfoID一致
                //调用这个方法之前一定要保证先调用一下服务器的代理成（上面的GetSysErrTimes完成调用）就是保证服务器端的kernelclientproxy调用invoke方法
                //在这个方法里面会初始化服务器端的kernelclientproxy的AuthenticationInfoID；
                //初始化好客户端kernelclientproxy的AuthenticationInfoID之后，代理成调用服务端才能通过省份验证
                AuthenticationInfoID = ESB.UP.WinLoginService.Invoke("GetAuthenticationInfoID").Value as string;

                //用户名密码是否正确
                bool islog = false;
                try
                {   //防止密码错误时抛出异常
                    islog = ESB.UP.WinLoginService.Invoke("sysLogin", pwd, uname, _publicConnectStr).Value.TryParseToBool();
                }
                catch
                {
                }

                if (islog)
                {
                    //进行服务器与客户端进行同步
                    _orgCode = ESB.UP.WinLoginService.Invoke("GetLoginOcode", _userConnectStr, uname).Value as string;
                    ESB.UP.WinLoginService.Invoke("SimulateWebLogin", account, _orgCode, uname, serverindex, "SYSTEM");//用webservice登录  
                    this.InitialKernalProxyClient(AuthenticationInfoID);

                    #region Online
                    string IPAddress = GetClientIP();
                    DataTable dt = ESB.UP.WinLoginService.Invoke("GetOnlineUserDt").Value as DataTable;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow[] drs = dt.Select(string.Format("UserID='{0} and UName='{1}' and IPAddress<>'{2}'", uname, account, IPAddress));
                        if (drs.Length > 0)
                        {
                            if (IPAddress == "127.0.0.1")
                            {
                                if (dt.Select(string.Format("UserID='{0} and UName='{1}' and IPAddress<>'Localhost'", uname, account)).Length > 0)
                                {
                                    error = "用户已经登录";
                                    return true;
                                }
                            }
                            else
                            {
                                error = "用户已经登录";
                                return true;
                            }
                        }
                    }
                    #endregion
                    return true;
                }
                else
                {
                    error = "登录失败，用户名或者密码不正确！";
                    return false;
                }

            }
            #endregion

            return false;
        }
        #endregion



        #region 初始化kernalProxyCLient
        private void InitialKernalProxyClient(string AuthenticationInfoID)
        {
            string _sessionid = ESB.UP.WinLoginService.Invoke("GetSessionID").Value as string;
            //KernelProxyClient.SetWebService(WebService.DealUrl("KernelService.asmx"));
            //KernelProxyClient.SetAuthenticationInfoID(AuthenticationInfoID);//这个id是关键取得服务器端第一次调用kernelProxyClient的AuthenticationInfo,不然客户端通过kernelProxyClient服务器端会不能通过验证
            //KernelProxyClient.SessionID = _sessionid;//x这个一定要设置，否则客户端kernalProxyCLient不能与服务器同步session
        }
        #endregion

        #region  根据服务器名称获取帐套信息
        /// <summary>
        /// 根据服务器名称获取帐套信息
        /// </summary>
        /// <param name="servername">数据库服务名称</param>
        /// <returns></returns>
        private DataTable GetAccInfo(string servername)
        {
            string result = null;
            int serverindex = 0;
            if (!string.IsNullOrEmpty(servername))
            {
                serverindex = this.GetServerIndex(servername);//数据库服务器名在database.xml文件中的索引
            }
            //获取公共链接串
            string _publicConnectStr = ESB.UP.WinLoginService.InvokeWithMessage("GetMainConnStringElement", 1, serverindex, result).Value as string;
            //获取帐套信息
            var d = ESB.UP.WinLoginService.Invoke("GetAccInfo", _publicConnectStr);
            return Ajax.DealAndGetValue(d) as DataTable;
        }
        #endregion

        #region 获取数据链接信息
        //private DataSet GetDataBaseConfig()
        //{
        //    ESBData d = ESB.UP.WinLoginService.Invoke("GetDataBaseConfig");
        //    return Ajax.DealAndGetValue(d) as DataSet;
        //}
        #endregion

        #region 获取客户端IP地址
        private string GetClientIP()
        {
            string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            return result;
        }
        #endregion
        #endregion
    }
}