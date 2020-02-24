using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Web.Controller;
using SUP.Common.Facade;
using NG3.Aop.Transaction;
using System.Web.Mvc;


namespace SUP.Common.Controller
{
    public  class UserOnlineInforController : AFController
    {
        IUserOnlineInforFacade proxy = null;

        public UserOnlineInforController()
        {
            proxy = AopObjectProxy.GetObject<IUserOnlineInforFacade>(new UserOnlineInforController());
        }

        public ActionResult RemoveLoginUser(string sSessionID)
        {
           bool flg = proxy.RemoveLoginUser(sSessionID);

           return Json(flg);
        }
               

        public string Refresh()
        {
            return proxy.Refresh();
        }

    }
}
