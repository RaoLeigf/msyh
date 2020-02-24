using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using WebApiSite;

namespace WebApiSite
{
    public class GUOApiAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "GUO";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <example>
        /// </example>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            RouteTable.Routes.MapHttpRoute(
                name: this.AreaName,
                routeTemplate: "api/GUO/{controller}/{action}/{id}",
                defaults:
                      new
                      {
                          action = RouteParameter.Optional,
                          id = RouteParameter.Optional,
                          namespaceName = new string[] { "Enterprise3.WebApi.GUO.Controller" },
                      },
                constraints: new { action = new StartWithConstraint() }
            ).RouteHandler = new WebApiConfig.SessionRouteHandler();
        }
    }
}