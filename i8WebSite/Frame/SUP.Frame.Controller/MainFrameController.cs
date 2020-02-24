using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Web.Mvc;
using NG3.Web.Controller;
using System.Web.Mvc;
using System.Web.SessionState;

namespace SUP.Frame.Controller
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class MainFrameController : AFController
    {
        public ActionResult MainFrameView()
        {
            string logid = NG3.AppInfoBase.LoginID;
            
            return View("MainFrameView");
            //return View("AddStuView"); 
        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult Index()
        {
            return View("Main");
        }

    }
}
