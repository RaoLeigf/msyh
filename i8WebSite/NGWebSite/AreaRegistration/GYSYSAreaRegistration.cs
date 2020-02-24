using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SUP
{
    public class GYSYSAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "GYS/YS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "GYSYS_default",
                "GYS/YS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "GYS3.YS.Controller" }
            );
        }
    }
}