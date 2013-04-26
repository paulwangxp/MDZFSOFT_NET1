using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Data.Entity;
using MyCMS.Models;

namespace MyCMS.Filters
{
    public class MyCMSAttributeFilter : System.Web.Mvc.ActionFilterAttribute
    {
        private LogDBContent db = new LogDBContent();

        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{ //在Action执行前执行
        //}

        private string GetIP()
        {
            string result = String.Empty;

            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            if (string.IsNullOrEmpty(result))
            {
                return "127.0.0.1";
            }
            return result;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        { //在Action执行之后执行

            return;
            //home,index-登录成功

            try
            {



                ActionLogModel logs = new ActionLogModel();

                logs.IP = GetIP();
                logs.UserId = int.Parse(filterContext.RequestContext.HttpContext.Request.Cookies["User"]["UserId"]);
                logs.DepartmentName = filterContext.RequestContext.HttpContext.Request.Cookies["User"]["DepName"];
                logs.RoleName = filterContext.RequestContext.HttpContext.Request.Cookies["User"]["UserRoleName"];
                logs.VisitTime = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day,
                    System.DateTime.Now.Hour, System.DateTime.Now.Minute, System.DateTime.Now.Second);
                logs.Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                logs.Action = filterContext.ActionDescriptor.ActionName;
                logs.Discription = "111";
                logs.BrowserType = filterContext.RequestContext.HttpContext.Request.Browser.Type;
                logs.BrowserVersion = filterContext.RequestContext.HttpContext.Request.Browser.Version;


                db.ActionLogs.Add(logs);
                db.SaveChanges();
            }
            catch (Exception)
            {
                
                ;
            }            

            base.OnActionExecuted(filterContext);
            //filterContext.ActionDescriptor.ControllerDescriptor.ControllerName
        }

        //public override void OnResultExecuted(ResultExecutedContext filterContext)
        //{ //在Result执行之后 
        //    base.OnResultExecuted(filterContext);
        //    //filterContext.RequestContext.HttpContext.Request.
        //}

        //public override void OnResultExecuting(ResultExecutingContext filterContext)
        //{ //在Result执行之前
        //    base.OnResultExecuting(filterContext);
        //    //todo:
        //}
    }

    
}