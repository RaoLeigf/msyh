using System.Web.Mvc;
using System.Web.SessionState;
using NG3.Web.Controller;
using NG3.Aop.Transaction;
using SUP.Frame.Facade;

namespace SUP.Frame.Controller
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ChangePwdController : AFController
    {
        private IChangePwdFacade proxy;

        public ChangePwdController()
        {
            proxy = AopObjectProxy.GetObject<IChangePwdFacade>(new ChangePwdFacade());
        }

        public ActionResult Index()
        {
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];
            return View("ChangePwd");
        }

        public string GetPwdLimit()
        {
            return proxy.GetPwdLimit();
        }

        public string SavePwd(string oldpwd, string newpwd)
        {
            return proxy.SavePwd(oldpwd, newpwd);
        }

    }
}
