using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SUP
{
    public class GQTQTAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "GQT/QT";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "GQTQT_default",
                "GQT/QT/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "GQT3.QT.Controller" }
            );
        }
    }
}