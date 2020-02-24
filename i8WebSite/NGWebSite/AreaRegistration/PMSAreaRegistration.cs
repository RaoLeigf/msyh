using System.Web.Mvc;

//EC的contoller的dll的默认命名空间前面的部分必需与这里保持一致
//命名空间应为EC.*，如EC.Supplier.Controller；
namespace PMS
{

    public class PMSAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "PMS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PMS_default",
                "PMS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
