using NG3.Web.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SUP.Common.Controller
{
    public  class ErrorPageController : AFController
    {

        public ActionResult Index()
        {
            ViewBag.Title = "错误提示";
            ViewBag.Msg = System.Web.HttpContext.Current.Request.Params["msg"];
            return View("ErrorPage");
        }
    }
}
