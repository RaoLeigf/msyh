using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PSU
{
    public class PSUAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "PSU";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PSU_default",
                "PSU/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}