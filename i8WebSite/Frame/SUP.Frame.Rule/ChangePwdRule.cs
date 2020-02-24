using Enterprise.Common.DataAccess;
using i6.UIProcess.MainFrame;
using NG.InstantMessage.Interface;
using NG.InstantMessage.Rules;
using NG.SDK10;
using NG.SDK10.Request;
using NG.SDK10.Response;
using NG.UP.UC.UCP.PublicService;
using SUP.Frame.DataAccess;
using System;
using System.Web;

namespace SUP.Frame.Rule
{
    public class ChangePwdRule
    {

        private ChangePwdDac dac = null;

        public ChangePwdRule()
        {
            dac = new ChangePwdDac();
        }

        public string SavePwd(string oldpwd, string newpwd)
        {
            string getoldpwd = dac.GetPassWord(AppSessionConfig.GetPubDBConnStr(), NG3.AppInfoBase.LoginID);
            string decodeoldpwd = NG.NGEncode.DecodePassword(getoldpwd, 128);
            if (oldpwd != decodeoldpwd)
            {
                return "旧密码输入不正确，请重新输入!";
            }

            string encodenewpwd = NG.NGEncode.EncodePassword(newpwd, 128);
            int iret = SetPassWord(NG3.AppInfoBase.LoginID, encodenewpwd);
            if (iret > 0)
            {
                //i6AppInfoEntity.PWD = newpwd;
                return string.Empty;
            }
            else
            {
                return "密码修改失败!";
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="logid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int SetPassWord(string logid, string password)
        {
            int iret = 0;
            try
            {
                //统一身份认证启用的情况下，同步修改统一身份认证服务器的用户密码
                if (IsUseUia())
                {
                    //旧密码
                    string oldpassword = NG.NGEncode.DecodePassword(HttpContext.Current.Session["uiapwd"].ToString(), 128);

                    string tokenIdentity = string.Empty;
                    //统一身份认证
                    string ssoPath = HttpContext.Current.Request.PhysicalApplicationPath + "\\sso.config";
                    string serverUrl = string.Format("http://{0}/v1.0/Router.ashx", SSOService.GetSTSLoginWebSiteName());
                    var client = new DefaultNGClient(serverUrl, MainCommonUIP.GetConfigSetion(ssoPath, "WebSiteIdentity"), MainCommonUIP.GetConfigSetion(ssoPath, "MD5key"), NG.SDK10.Enums.EnumDataFormat.Xml);
                    string newpassword = NG.NGEncode.DecodePassword(password, 128);

                    var reqCPW = new UserChangePWRequest()
                    {
                        //tokenkey
                        TokenIdentity = HttpContext.Current.Session["ssotoken"].ToString(),
                        //userkey
                        ChangeUserKey = HttpContext.Current.Session["ssouserkey"].ToString(),
                        NewPassword = newpassword,
                        OldPassword = oldpassword
                    };

                    UserResponse user = client.Execute(reqCPW);

                    if (!string.IsNullOrEmpty(user.UserKey))
                    {
                        iret = 1;
                    }

                    iret = dac.SetPassWord(AppSessionConfig.GetPubDBConnStr(),logid, password);

                    string loginACCount = AppSessionConfig.GetLoginACCount();//登录帐套
                    bool netcallIsAvailable = PublicUtils.GetNetcallIsAvailableEx(loginACCount);
                    if (netcallIsAvailable)
                    {
                        IFactory facatory = new NetCallFactory();
                        IInstantMessageRules rules = facatory.Create();
                        string msg = string.Empty;
                        rules.SetUserPassWord(logid, password, ref msg);
                    }

                    System.Diagnostics.Trace.WriteLine("修改用户Password成功！ 用户标识：" + user.UserKey);
                }
                else
                {                   
                    //string file = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + @"\DMC\TimerService\PortalMaptoNetcallConfig.dat";
                    string loginACCount = AppSessionConfig.GetLoginACCount();//登录帐套
                    bool netcallIsAvailable = PublicUtils.GetNetcallIsAvailableEx(loginACCount);
                    if (netcallIsAvailable)
                    {
                        IFactory facatory = new NetCallFactory();
                        IInstantMessageRules rules = facatory.Create();
                        string msg = string.Empty;
                        rules.SetUserPassWord(logid, password, ref msg);
                    }

                    iret = dac.SetPassWord(AppSessionConfig.GetPubDBConnStr(), logid, password);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message + e.StackTrace);
                iret = 0;
                //throw e;  netcall密码有时候修改失败，异常不抛出
            }
            return iret;
        }

        /// <summary>
        /// 是否使用统一身份认证
        /// </summary>
        /// <returns></returns>
        private bool IsUseUia()
        {
            bool useUia = false;
            string ssoPath = HttpContext.Current.Request.PhysicalApplicationPath + "\\..\\sso.config";

            if ("1" == MainCommonUIP.GetConfigSetion(ssoPath, "UseNGSSO"))
            {
                useUia = true;
            }
            return useUia;
        }

    }
}
