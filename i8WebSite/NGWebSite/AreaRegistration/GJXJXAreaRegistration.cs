using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SUP
{
    public class GJXJXAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "GJX/JX";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "GJXJX_default",
                "GJX/JX/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "GJX3.JX.Controller" }
            );
        }
    }
}