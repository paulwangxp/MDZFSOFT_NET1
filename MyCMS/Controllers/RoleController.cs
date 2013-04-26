using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyCMS.Models;

namespace MyCMS.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private MyCMSDBContent db = new MyCMSDBContent();

        //
        // GET: /Role/

        public ActionResult Index()
        {
            return View(db.Roles.ToList());
        }

        //
        // GET: /Role/Details/5

        public ActionResult Details(int id = 0)
        {
            webpages_Role webpages_role = db.Roles.Find(id);
            if (webpages_role == null)
            {
                return HttpNotFound();
            }
            return View(webpages_role);
        }

        //
        // GET: /Role/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Role/Create

        [HttpPost]
        public ActionResult Create(webpages_Role webpages_role)
        {
            if (ModelState.IsValid)
            {
                db.Roles.Add(webpages_role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(webpages_role);
        }

        //
        // GET: /Role/Edit/5

        public ActionResult Edit(int id = 0)
        {
            webpages_Role webpages_role = db.Roles.Find(id);
            if (webpages_role == null)
            {
                return HttpNotFound();
            }
            return View(webpages_role);
        }

        //
        // POST: /Role/Edit/5

        [HttpPost]
        public ActionResult Edit(webpages_Role webpages_role)
        {
            if (ModelState.IsValid)
            {
                db.Entry(webpages_role).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(webpages_role);
        }

        //
        // GET: /Role/Delete/5

        public ActionResult Delete(int id = 0)
        {
            webpages_Role webpages_role = db.Roles.Find(id);
            if (webpages_role == null)
            {
                return HttpNotFound();
            }
            return View(webpages_role);
        }

        //
        // POST: /Role/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {

            var users = from user in db.Users
                        where user.RoleId == id
                        select user;
            if (users.ToList().Count > 0)//有用户属于此角色不能删除
            {
                ModelState.AddModelError("", "有用户属于此角色，无法删除！");
                return View(db.Roles.Find(id));
            }

            webpages_Role webpages_role = db.Roles.Find(id);
            db.Roles.Remove(webpages_role);
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