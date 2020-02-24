using System.Web;
using System.Web.Mvc;

namespace WebApiSite
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            if (filters == null)
                return;

            filters.Add(new HandleErrorAttribute());

            filters.Add(new CustomHandleErrorAttribute()); //添加普通的Controller的异常过滤
        }
    }
}
