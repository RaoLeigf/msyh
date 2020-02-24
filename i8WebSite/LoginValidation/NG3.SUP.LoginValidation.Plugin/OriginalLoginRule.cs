using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.SUP.LoginValidation.Interface;
//using SUP.Common.Base;
using SUP.Frame.Facade;
using NG3.Aop.Transaction;

namespace NG3.SUP.LoginValidation.Plugin
{
    /// <summary>
    /// 产品原有的登录逻辑
    /// </summary>
    public sealed class OriginalLoginRule : LoginValidationFilterBase<FilterResult, LoginValidationParam>
    {
        public OriginalLoginRule()
        {
            //string logserviceurl = System.Configuration.ConfigurationManager.AppSettings["WinLoginWebServiceUrl"].ToString();
            //this._winLoginService.Url = logserviceurl;
        }

        public override FilterResult Filter(LoginValidationParam param)
        {
            FilterResult filterResult = new FilterResult();

            string svrName = param["svrName"].ToString();
            string account = param["account"].ToString();
            string logid = param["logid"].ToString();
            string pwd = param["pwd"].ToString();
            ILoginFacade proxy  = AopObjectProxy.GetObject<ILoginFacade>(new LoginFacade());
            bool loginflag=true;
            string msg=string.Empty;
            proxy.Check(ref msg, ref loginflag, svrName, account, logid, pwd);
            filterResult.Success = loginflag;
            filterResult.ErrorMsg = msg;
            return filterResult;
        }
    }
}
