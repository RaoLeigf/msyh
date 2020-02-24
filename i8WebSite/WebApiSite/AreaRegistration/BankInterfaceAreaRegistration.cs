using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;

namespace BankInterfaceWebApplication.AreaRegistrations
{
    public class BankInterfaceAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "BankInterface";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //context.MapRoute(
            //    "BankInterface_Default",
            //    "BIF/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional },
            //    new string[] { "GData.YQHL.WebApi.Controller" });

            RouteTable.Routes.MapHttpRoute(
                name: this.AreaName,
                routeTemplate: "api/BIF/{controller}/{action}/{id}",
                defaults: new {
                    action = RouteParameter.Optional,
                    id = RouteParameter.Optional,
                    namespaceName = new string[] { "GData.YQHL.WebApi.Controller" }
                }                
            );
        }
    }
}