using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using System.Web.Http;

namespace NGWebSite
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //忽略weform页面,老页面可正常打开
            routes.IgnoreRoute("{resource}.aspx/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                new[] { "NGWebSite" }
            );


            foreach (Route route in routes)
            {
                route.RouteHandler = new NG3.Web.Mvc.NGMvcRouteHandler();
            }
        }

        protected void Application_Start()
        {

            
            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configuration.MessageHandlers.Add(new MessageHandler());
            
            ControllerBuilder.Current.SetControllerFactory(new NG3.Web.Controller.NGControllerFactory());

            //RegisterGlobalFilters(GlobalFilters.Filters);
            //RegisterRoutes(RouteTable.Routes);
            
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            WebApiConfig.Register(GlobalConfiguration.Configuration);

            RouteConfig.RegisterRoutes(RouteTable.Routes);      

            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            //AuthConfig.RegisterAuth();

        }

        /// <summary>
        /// 防止WebForm的Session丢失
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_Start(Object sender, EventArgs e)
        {
            try
            {
                HttpContext.Current.Session.Add("__MyAppSession", string.Empty);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        
    }
}