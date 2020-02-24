using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SUP
{
    public class SUPAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SUP";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SUP_default",
                "SUP/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}