using System.Web.Mvc;

namespace RW
{
    public class RWAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "RW";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "RW_default",
                "RW/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }

    }
}