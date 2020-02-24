using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NG3.Web.Controller;
using NG3.Web.Mvc;
using System.Web.Mvc;

namespace NGWebSite
{
    public class HomeController : AFController
    {

        public HomeController()
        { 
        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult Main()
        {
            return View("Main");
        }


        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult Index()
        {
            return View("Login");
        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult PopLogin()
        {
            return View("PopLogin");
        }

    }
}