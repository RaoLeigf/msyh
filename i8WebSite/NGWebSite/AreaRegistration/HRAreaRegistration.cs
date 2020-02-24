using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR
{
    public class HRAreaRegistration : AreaRegistration
    {

        public override string AreaName
        {
            get
            {
                return "HR";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "HR_default",
                "HR/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }

    }
}