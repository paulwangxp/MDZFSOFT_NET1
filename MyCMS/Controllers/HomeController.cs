using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyCMS.Models;
using System.Data.Entity;

namespace MyCMS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private MyCMSDBContent db = new MyCMSDBContent();

        public ActionResult Index()
        {
            //根据当前登录用户信息，返回用户表数据
            var user1 = from u in db.Users
                        where u.UserName.Equals(User.Identity.Name)
                        select u;
            if (user1 == null)
            {
                return HttpNotFound();
            }

            return View();

            return View(user1.ToList()[0]);
        }

        public ActionResult About()
        {
            ViewBag.Message = "你的应用程序说明页。";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "你的联系方式页。";

            return View();
        }
    }
}
