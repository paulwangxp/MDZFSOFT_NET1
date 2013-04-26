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
    public class NoticeController : Controller
    {
        private MyCMSDBContent db = new MyCMSDBContent();

        //
        // GET: /Notice/

        public ActionResult Index(int page = 1)
        {
            int maxRecords = 4;//每页4条
            int currentPage = page;

            //ViewBag.HasPreviousPage = true;

            var notices = db.Notices.OrderBy(p => p.NoticeId);

            return View( notices.ToPagedList(currentPage,
            maxRecords));
        }

        public ActionResult Index2(int page = 1)
        {

            int maxRecords = 4;//每页4条
            int currentPage = page ;

            //ViewBag.HasPreviousPage = true;

            //var notices = db.Notices.Include(a=>a.NoticeId);//.OrderBy(p=>p.NoticeId);
            var notices = db.Notices.OrderBy(p=>p.NoticeId);


            return PartialView("Index", notices.ToPagedList(currentPage,
            maxRecords));
        }

        //
        // GET: /Notice/Details/5

        public ActionResult Details(int id = 0)
        {
            NoticeModel noticemodel = db.Notices.Find(id);
            if (noticemodel == null)
            {
                return HttpNotFound();
            }
            return View(noticemodel);
        }

        //
        // GET: /Notice/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Notice/Create

        [HttpPost]
        public ActionResult Create(NoticeModel noticemodel)
        {
            if (ModelState.IsValid)
            {
                db.Notices.Add(noticemodel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(noticemodel);
        }

        //
        // GET: /Notice/Edit/5

        public ActionResult Edit(int id = 0)
        {
            NoticeModel noticemodel = db.Notices.Find(id);
            if (noticemodel == null)
            {
                return HttpNotFound();
            }
            return View(noticemodel);
        }

        //
        // POST: /Notice/Edit/5

        [HttpPost]
        public ActionResult Edit(NoticeModel noticemodel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(noticemodel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(noticemodel);
        }

        //
        // GET: /Notice/Delete/5

        public ActionResult Delete(int id = 0)
        {
            NoticeModel noticemodel = db.Notices.Find(id);
            if (noticemodel == null)
            {
                return HttpNotFound();
            }
            return View(noticemodel);
        }

        //
        // POST: /Notice/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            NoticeModel noticemodel = db.Notices.Find(id);
            db.Notices.Remove(noticemodel);
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