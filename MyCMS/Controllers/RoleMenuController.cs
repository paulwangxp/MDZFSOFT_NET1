using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyCMS.Models;
using System.Web.UI;
using System.Runtime.Caching;

namespace MyCMS.Controllers
{
    [Authorize]
    public class RoleMenuController : Controller
    {
        private MyCMSDBContent db = new MyCMSDBContent();

        //
        // GET: /RoleMenu/

        public ActionResult Index(int id)
        {

            int userId = id;
            var userMenus = db.RoleMenus.Include(a => a.menus).Where(p => p.MenuId == p.menus.MenuId && p.UserRoleId == userId);
            if (userMenus == null)
            {
                return HttpNotFound();
            }


            ViewBag.MenuList = userMenus;

            return View(db.RoleMenus.Where(c=>c.UserRoleId == id).ToList());
        }

        [HttpPost]
        public ActionResult Index(int id,FormCollection form)
        {

            foreach (var menuId in form.AllKeys)
            {
                var enableState = form[menuId];
                int iMenuId = int.Parse(menuId);
                RoleMenu menu1 = db.RoleMenus.Where(p => p.MenuId == iMenuId && p.UserRoleId == id).ToList()[0];
                if (menu1 != null )
                {
                    menu1.Enable = enableState.Equals("false")?false:true;
                    db.Entry(menu1).State = EntityState.Modified;
                    //db.SaveChanges();
                }
                
            }
            db.SaveChanges();

            
            //OutputCacheAttribute.ChildActionCache = new MemoryCache("MyMenus");
            return RedirectToAction("Index");
        }

        //[OutputCache(Duration = int.MaxValue)]
        //[OutputCache(CacheProfile = "Cache1Hour")]
        //[OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult MyMenus()
        {
            //if (Session["UserRoleId"] == null)
            //    return PartialView("_Navigation", null);
            if (  String.IsNullOrEmpty( Request.Cookies["User"]["UserId"] ) || String.IsNullOrEmpty( Request.Cookies["User"]["UserRoleId"]) )
                return PartialView("_Navigation", null);

            int userId = int.Parse(Request.Cookies["User"]["UserRoleId"]);
            var userMenus = db.RoleMenus.Include(a => a.menus).Where(p => p.MenuId == p.menus.MenuId && p.UserRoleId == userId && p.Enable == true);
            if (userMenus == null)
            {
                return HttpNotFound();
            }

            //Response.Cache.SetOmitVaryStar(true);//fix .net bug

            return PartialView("_Navigation", userMenus);
        }

        //
        // GET: /RoleMenu/Details/5

        public ActionResult Details(int id = 0)
        {
            RoleMenu rolemenu = db.RoleMenus.Find(id);
            if (rolemenu == null)
            {
                return HttpNotFound();
            }
            return View(rolemenu);
        }

        //
        // GET: /RoleMenu/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /RoleMenu/Create

        [HttpPost]
        public ActionResult Create(RoleMenu rolemenu)
        {
            if (ModelState.IsValid)
            {
                db.RoleMenus.Add(rolemenu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rolemenu);
        }

        //
        // GET: /RoleMenu/Edit/5

        public ActionResult Edit(int id = 0)
        {
            RoleMenu rolemenu = db.RoleMenus.Find(id);
            if (rolemenu == null)
            {
                return HttpNotFound();
            }
            return View(rolemenu);
        }

        //
        // POST: /RoleMenu/Edit/5

        [HttpPost]
        public ActionResult Edit(RoleMenu rolemenu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rolemenu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rolemenu);
        }

        //
        // GET: /RoleMenu/Delete/5

        public ActionResult Delete(int id = 0)
        {
            RoleMenu rolemenu = db.RoleMenus.Find(id);
            if (rolemenu == null)
            {
                return HttpNotFound();
            }
            return View(rolemenu);
        }

        //
        // POST: /RoleMenu/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            RoleMenu rolemenu = db.RoleMenus.Find(id);
            db.RoleMenus.Remove(rolemenu);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}