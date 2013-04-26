using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace MyCMS.Models
{
    public class MyUserAuthorizeAttribute : AuthorizeAttribute
    {
        MyCMSDBContent MyAuthorizeDBContent = null;


        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            if(filterContext.RouteData.Values["controller"].Equals("Account"))
            {
                return;
            }

            MyAuthorizeDBContent = new MyCMSDBContent();

            string userName = WebSecurity.CurrentUserName;

            var isAllowed = false;

            if (userName.Length > 0)//用户已经登录
            {
                var controller = filterContext.RouteData.Values["controller"].ToString();
                var action = filterContext.RouteData.Values["action"].ToString();
                isAllowed = this.IsAllowed(userName, controller, action);
            }
            else
            {
                filterContext.HttpContext.Response.Redirect(new UrlHelper(filterContext.RequestContext).Action("Login", "Account"),true);
                filterContext.RequestContext.HttpContext.Response.End();
                //filterContext.Result = new EmptyResult();
                return;
            }

            if (!isAllowed)
            {
                filterContext.RequestContext.HttpContext.Response.Write("无权访问");
                filterContext.RequestContext.HttpContext.Response.End();
            }

        }

        /// <summary>
        /// 判断是否允许访问
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="controller">控制器</param>
        /// <param name="action">action</param>
        /// <returns>是否允许访问</returns>
        public bool IsAllowed(string userName, string controller, string action)
        {
            //得到用户的角色
            var userRole = from role in MyAuthorizeDBContent.Roles
                               join us in MyAuthorizeDBContent.Users
                               on role.RoleId equals us.RoleId
                               where us.UserName.Equals(userName)
                               select new 
                               {
                                   role.RoleName,
                                   role.RoleId
                               };


            if (userRole.Count() <= 0)
                return false;

            //管理员角色没有任何限制
            if (userRole.ToList()[0].RoleName.Equals("admin"))
                return true;

            int roleId = userRole.ToList()[0].RoleId;

            
            //根据menus里的menuid去rolemenus表查询对应的role是否存在记录(并且要包含定义的controller=>menuUrl)，不存在即为未定义，返回false

            var menuItem = from m in MyAuthorizeDBContent.Menus
                           join rm in MyAuthorizeDBContent.RoleMenus
                           on m.MenuId equals rm.MenuId
                           where rm.UserRoleId == roleId
                           && m.MenuUrl.Equals("/"+controller)                           
                           select new
                           {
                               m.MenuUrl,
                               m.MenuName
                           };

            //未找到角色对应的controller
            if (menuItem.Count() <= 0)
                return false;

            //找到了，有权限
            return true;


        }
            

    }
    
}