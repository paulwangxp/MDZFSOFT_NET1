using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyCMS.Models;
using PagedList;

namespace MyCMS.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private MyCMSDBContent db = new MyCMSDBContent();

        //
        // GET: /User/

        public ActionResult Index(string name = "", string userCode = "", string DepartmentId = "", int page = 1)
        {

            int maxRecords = 4;//每页4条
            int currentPage = page;


            //var users = db.Users.Include(a => a.Role).Include(a => a.Dep).OrderBy(p=>p.UserId);
            var users = from user in db.Users select user;

            string sDepid = Request.Cookies["User"]["DepID"];   

            

            //字段搜索功能
            if (!String.IsNullOrEmpty(name))
            {
                users = users.Where(s => s.Name.Contains(name));
            }
            if (!String.IsNullOrEmpty(userCode))
            {
                users = users.Where(s => s.UserCode.Contains(userCode));
            }
            if (!String.IsNullOrEmpty(DepartmentId))
            {
                int depId = int.Parse(DepartmentId);
                users = users.Where(s => s.DepartmentId == depId);
            }
            else
            {
                int ownerDepId = int.Parse(sDepid);
                users = users.Where(s => s.DepartmentId == ownerDepId);
            }
            //分页上要保留上次搜索的值
            ViewBag.name = String.IsNullOrEmpty(name) ? "" : name;
            ViewBag.userCode = String.IsNullOrEmpty(userCode) ? "" : userCode;
            //ViewBag.DepartmentId = DepartmentId;

            MyTools myTools = new MyTools();
            ViewBag.DepartmentId = new SelectList(myTools.GetDepList(sDepid), "DepartmentId", "name");

            //分页需要排序
            users = users.OrderByDescending(p => p.UserId);
            
            //return View(users.ToList());
            return View(users.ToPagedList(currentPage,
            maxRecords));
        }

        public ActionResult UserSelect(string name = "", string userCode = "", string DepartmentId = "", int page = 1)
        {

            int maxRecords = 4;//每页4条
            int currentPage = page;


            //var users = db.Users.Include(a => a.Role).Include(a => a.Dep).OrderBy(p=>p.UserId);
            var users = from user in db.Users select user;

            int ownerDepId = int.Parse(Request.Cookies["User"]["DepID"]);
            var path = from dep in db.Departments
                       where dep.DepartmentId == ownerDepId
                       select dep.Path;
            string myPath = path.ToList()[0];
            var depList = from dep in db.Departments
                          where dep.Path.Contains(myPath)
                          orderby dep.Path
                          select dep;



            //字段搜索功能
            if (!String.IsNullOrEmpty(name))
            {
                users = users.Where(s => s.Name.Contains(name));
            }
            if (!String.IsNullOrEmpty(userCode))
            {
                users = users.Where(s => s.UserCode.Contains(userCode));
            }
            if (!String.IsNullOrEmpty(DepartmentId))
            {
                int depId = int.Parse(DepartmentId);
                users = users.Where(s => s.DepartmentId == depId);
            }
            else
            {
                users = users.Where(s => s.DepartmentId == ownerDepId);
            }
            //分页上要保留上次搜索的值
            ViewBag.name = String.IsNullOrEmpty(name) ? "" : name;
            ViewBag.userCode = String.IsNullOrEmpty(userCode) ? "" : userCode;
            //ViewBag.DepartmentId = DepartmentId;




            ViewBag.DepartmentId = new SelectList(depList, "DepartmentId", "name");

            //分页需要排序
            users = users.OrderByDescending(p => p.UserId);


            return PartialView("UserSelect", users.ToPagedList(currentPage,
            maxRecords));
        }

        



        //
        // GET: /User/Details/5

        public ActionResult Details(int id = 0)
        {
            UserProfile userprofile = db.Users.Find(id);
            if (userprofile == null)
            {
                return HttpNotFound();
            }
            return View(userprofile);
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            return HttpNotFound();
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "name");
            ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "RoleName");
            return View();
        }

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Create(UserProfile userprofile)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(userprofile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "name", userprofile.DepartmentId);
            ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "RoleName", userprofile.RoleId);
            return View(userprofile);
        }

        
        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id = 0)
        {
            UserProfile userprofile = db.Users.Find(id);
            if (userprofile == null)
            {
                return HttpNotFound();
            }

            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "男", Value = "1", Selected = true });
            li.Add(new SelectListItem { Text = "女", Value = "0" });
            ViewBag.Sex = li;


            List<SelectListItem> enableLi = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "启用", Value = "1", Selected = true });
            li.Add(new SelectListItem { Text = "停用", Value = "0" });
            ViewBag.Enable = enableLi;


            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "name", userprofile.DepartmentId);
            ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "RoleName", userprofile.RoleId);
            return View(userprofile);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult Edit(UserProfile userprofile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userprofile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(userprofile);
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(int id = 0)
        {
            return HttpNotFound();

            UserProfile userprofile = db.Users.Find(id);
            if (userprofile == null)
            {
                return HttpNotFound();
            }
            return View(userprofile);
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            UserProfile userprofile = db.Users.Find(id);
            db.Users.Remove(userprofile);
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