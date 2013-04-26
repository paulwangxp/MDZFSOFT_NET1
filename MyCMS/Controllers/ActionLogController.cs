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
    public class ActionLogController : Controller
    {
        private LogDBContent db = new LogDBContent();

        //
        // GET: /ActionLog/

        public ActionResult Index(int page =1)
        {
            //var logs = db.ActionLogs.Include(a => a.Role).Include(a => a.Dep);
            //return View(logs.ToList());

            int maxRecords = 10;//每页4条
            int currentPage = page;

            var logs = from log in db.ActionLogs select log;
            logs = logs.OrderByDescending(a => a.ActionLogId);

            return View(logs.ToPagedList(currentPage,
            maxRecords));
        }

        //
        // GET: /ActionLog/Details/5

        public ActionResult Details(int id = 0)
        {
            ActionLogModel actionlogmodel = db.ActionLogs.Find(id);
            if (actionlogmodel == null)
            {
                return HttpNotFound();
            }
            return View(actionlogmodel);
        }

        //
        // GET: /ActionLog/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ActionLog/Create

        [HttpPost]
        public ActionResult Create(ActionLogModel actionlogmodel)
        {
            if (ModelState.IsValid)
            {
                db.ActionLogs.Add(actionlogmodel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(actionlogmodel);
        }

        //
        // GET: /ActionLog/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ActionLogModel actionlogmodel = db.ActionLogs.Find(id);
            if (actionlogmodel == null)
            {
                return HttpNotFound();
            }
            return View(actionlogmodel);
        }

        //
        // POST: /ActionLog/Edit/5

        [HttpPost]
        public ActionResult Edit(ActionLogModel actionlogmodel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(actionlogmodel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(actionlogmodel);
        }

        //
        // GET: /ActionLog/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ActionLogModel actionlogmodel = db.ActionLogs.Find(id);
            if (actionlogmodel == null)
            {
                return HttpNotFound();
            }
            return View(actionlogmodel);
        }

        //
        // POST: /ActionLog/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ActionLogModel actionlogmodel = db.ActionLogs.Find(id);
            db.ActionLogs.Remove(actionlogmodel);
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