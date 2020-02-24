using System.Web.Mvc;

//EC的contoller的dll的默认命名空间前面的部分必需与这里保持一致
//命名空间应为EC.*，如EC.Supplier.Controller；
namespace EC
{

    public class ECAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "EC";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "EC_default",
                "EC/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
