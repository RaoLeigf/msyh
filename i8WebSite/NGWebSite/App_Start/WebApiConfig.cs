using System.Web.Http;
using System.Web.Routing;
using NG3.Web.Mvc;

namespace NGWebSite
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            //支持session
            RouteTable.Routes.MapHttpRoute(name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            ).RouteHandler = new WebApiSessionHandler();

        }
    }
}
