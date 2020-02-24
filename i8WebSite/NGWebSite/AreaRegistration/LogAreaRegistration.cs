using System.Web.Mvc;

//EC的contoller的dll的默认命名空间前面的部分必需与这里保持一致
//命名空间应为NG3.Log.*，如Log.Controller；
namespace NG3.Log
{

    public class LogAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "NG3.Log";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Log_default",
                "Log/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
