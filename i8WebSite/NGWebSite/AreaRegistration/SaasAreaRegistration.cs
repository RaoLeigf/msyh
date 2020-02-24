using System.Web.Mvc;

//EC的contoller的dll的默认命名空间前面的部分必需与这里保持一致
//命名空间应为EC.*，如EC.Supplier.Controller；
namespace Saas
{

    public class PMSAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Saas";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Saas_default",
                "Saas/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
