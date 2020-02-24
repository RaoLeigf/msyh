using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Web.Controller;
using System.Web.Mvc;
using SUP.Frame.Facade;
using NG3.Aop.Transaction;

namespace SUP.Frame.Controller
{
    public class UserController : System.Web.Mvc.Controller
    {
        private IUserFacade proxy;

        public UserController()
        {
            proxy = AopObjectProxy.GetObject<IUserFacade>(new UserFacade());

            ViewBag.pwdLimitInfo = proxy.GetPwdLimit();
        }

        public ActionResult SetUserPwd()
        {
            return View("ChangePwd");
        }

        public string ChangePwd(string oldpwd, string newpwd)
        {
            string msg = string.Empty;
            return proxy.ChangePwd(NG3.AppInfoBase.LoginID, oldpwd, newpwd,ref msg);
        }

       
    }
}
