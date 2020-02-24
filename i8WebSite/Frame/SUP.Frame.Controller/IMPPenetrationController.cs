using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Web.Mvc;
using NG3.Aop.Transaction;
using SUP.Frame.Facade;
using NG3.Web.Controller;
using NG3;
using SUP.Common.Base;
using SUP.Frame.DataEntity;
using Newtonsoft.Json;
using System.Web.SessionState;

namespace SUP.Frame.Controller
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class IMPPenetrationController : AFController
    {
        public ActionResult IMPPenetration()
        {
            string buscode = System.Web.HttpContext.Current.Request.Params["buscode"];
            string text = System.Web.HttpContext.Current.Request.Params["text"];
            string temppkitem = System.Web.HttpContext.Current.Request.Params["temppkitem"];
            string temppkvalue = System.Web.HttpContext.Current.Request.Params["temppkvalue"];
            ViewBag.buscode = buscode;
            ViewBag.text = text;
            ViewBag.temppkitem = temppkitem;
            ViewBag.temppkvalue = temppkvalue;            
            return View("IMPPenetration");
        }
    }
}
