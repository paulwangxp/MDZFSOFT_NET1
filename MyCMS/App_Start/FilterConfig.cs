using System.Web;
using System.Web.Mvc;
using MyCMS.Filters;
using MyCMS.Models;

namespace MyCMS
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new MyCMSAttributeFilter());//访问日志AOP
            //filters.Add(new MyUserAuthorizeAttribute());//权限检查
        }
    }
}