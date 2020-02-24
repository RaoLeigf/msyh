using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SUP
{
    public class GXMXMAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "GXM/XM";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "GXMXM_default",
                "GXM/XM/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "GXM3.XM.Controller" }
            );
        }
    }
}