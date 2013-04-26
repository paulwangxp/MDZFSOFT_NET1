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
    public class VendorController : Controller
    {
        private MyCMSDBContent db = new MyCMSDBContent();

        //
        // GET: /Vendor/

        public ActionResult Index()
        {
            return View(db.VendorModels.ToList());
        }

        //
        // GET: /Vendor/Details/5

        public ActionResult Details(int id = 0)
        {
            VendorModel vendormodel = db.VendorModels.Find(id);
            if (vendormodel == null)
            {
                return HttpNotFound();
            }
            return View(vendormodel);
        }

        //
        // GET: /Vendor/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Vendor/Create

        [HttpPost]
        public ActionResult Create(VendorModel vendormodel)
        {
            if (ModelState.IsValid)
            {
                vendormodel.CreateTime = DevTools.GetNowDateTime();
                vendormodel.ModifyTime = vendormodel.CreateTime;
                db.VendorModels.Add(vendormodel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vendormodel);
        }

        //
        // GET: /Vendor/Edit/5

        public ActionResult Edit(int id = 0)
        {
            VendorModel vendormodel = db.VendorModels.Find(id);
            if (vendormodel == null)
            {
                return HttpNotFound();
            }
            return View(vendormodel);
        }

        //
        // POST: /Vendor/Edit/5

        [HttpPost]
        public ActionResult Edit(VendorModel vendormodel)
        {
            if (ModelState.IsValid)
            {
                vendormodel.ModifyTime = DevTools.GetNowDateTime();
                db.Entry(vendormodel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vendormodel);
        }

        //
        // GET: /Vendor/Delete/5

        public ActionResult Delete(int id = 0)
        {
            VendorModel vendormodel = db.VendorModels.Find(id);
            if (vendormodel == null)
            {
                return HttpNotFound();
            }
            return View(vendormodel);
        }

        //
        // POST: /Vendor/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            VendorModel vendormodel = db.VendorModels.Find(id);
            db.VendorModels.Remove(vendormodel);
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