using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.WebHost;
using System.Web.Routing;
using System.Web.SessionState;
using Enterprise3.WebApi.ApiControllerBase;

namespace WebApiSite
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务


            //// Web API 路由
            //config.MapHttpAttributeRoutes();
            //website降级为4.0版本 by liming

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            if (config == null)
                return;

            //通过传递"ct"的值，创建返回类型，目前已经更改成通过FormattingOverrideFilterAttribute来进行处理，f Or format == xml/x Or j/js/json
            //config.Formatters.XmlFormatter.MediaTypeMappings.Add(new QueryStringMapping("ct", "xml", "application/xml"));
            //config.Formatters.JsonFormatter.MediaTypeMappings.Add(new QueryStringMapping("ct", "json", "application/json"));
            config.Filters.Add(new MethodExceptionFilterAttribute());

            config.MessageHandlers.Add(new TraceMessageHandler()); //添加跟踪消息处理
            config.MessageHandlers.Add(new CompressionHandler()); //添加内容压缩处理
            //config.MessageHandlers.Add(new SetAppInfoBaseHandler()); //添加AppInfoBase信息

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{action}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            //支持session
            RouteTable.Routes.MapHttpRoute(name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new{ id = RouteParameter.Optional }
            ).RouteHandler = new SessionRouteHandler();

            // 取消注释下面的代码行可对具有 IQueryable 或 IQueryable<T> 返回类型的操作启用查询支持。
            // 若要避免处理意外查询或恶意查询，请使用 QueryableAttribute 上的验证设置来验证传入查询。
            // 有关详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=279712。
            //config.EnableQuerySupport();

            // 若要在应用程序中禁用跟踪，请注释掉或删除以下代码行
            // 有关详细信息，请参阅: http://www.asp.net/web-api
            //config.EnableSystemDiagnosticsTracing();

        }

        public class SessionControllerHandler : HttpControllerHandler, IRequiresSessionState
        {
            public SessionControllerHandler(RouteData routeData)
                : base(routeData)
            {

            }
        }


        public class SessionRouteHandler : IRouteHandler
        {
            public System.Web.IHttpHandler GetHttpHandler(RequestContext context)
            {
                if (context == null)
                    return null;

                return new SessionControllerHandler(context.RouteData);
            }
        }
    }
}
