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
    public class DeviceController : Controller
    {
        private MyCMSDBContent db = new MyCMSDBContent();

        //
        // GET: /Device/

        public ActionResult Index(int DeviceTypeId = 0, string deviceNumber = "", int VendorId = 0)
        {
            

            var deviceList = from device in db.DeviceModels.Include(a=>a.vendor).Include(b=>b.deviceType).Include(c=>c.user)
                             select device;

            ViewBag.VendorId = new SelectList(db.VendorModels, "VendorId", "name");
            ViewBag.DeviceTypeId = new SelectList(db.DeviceTypes, "DeviceTypeId", "name");
            ViewBag.UserId = new SelectList(db.Users, "UserId", "name");
            ViewBag.deviceNumber = deviceNumber;

            //字段搜索功能
            if (DeviceTypeId != 0)
            {
                deviceList = deviceList.Where(s => s.DeviceTypeId.Equals(DeviceTypeId));
            }
            if (VendorId != 0)
            {
                deviceList = deviceList.Where(s => s.VendorId.Equals(VendorId));
            }
            if (!String.IsNullOrEmpty(deviceNumber))
            {
                deviceList = deviceList.Where(s => s.DeviceNumber.Contains(deviceNumber));
            }


            return View(deviceList.ToList());
        }

        //
        // GET: /Device/Details/5

        public ActionResult Details(int id = 0)
        {
            DeviceModel devicemodel = db.DeviceModels.Find(id);
            if (devicemodel == null)
            {
                return HttpNotFound();
            }
            return View(devicemodel);
        }

        //
        // GET: /Device/Create

        public ActionResult Create()
        {
            ViewBag.VendorId = new SelectList(db.VendorModels, "VendorId", "name");
            ViewBag.DeviceTypeId = new SelectList(db.DeviceTypes, "DeviceTypeId", "name");

            return View();
        }

        //
        // POST: /Device/Create

        [HttpPost]
        public ActionResult Create(DeviceModel devicemodel)
        {
            if (ModelState.IsValid)
            {
                devicemodel.CreateTime = DevTools.GetNowDateTime();
                devicemodel.ModifyTime = devicemodel.CreateTime;
                db.DeviceModels.Add(devicemodel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(devicemodel);
        }

        //
        // GET: /Device/Edit/5

        public ActionResult Edit(int id = 0)
        {
            

            DeviceModel devicemodel = db.DeviceModels.Find(id);
            if (devicemodel == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserName = devicemodel.user.Name;
            ViewBag.VendorId = new SelectList(db.VendorModels, "VendorId", "name", devicemodel.VendorId);
            ViewBag.DeviceTypeId = new SelectList(db.DeviceTypes, "DeviceTypeId", "name", devicemodel.DeviceTypeId);


            return View(devicemodel);
        }

        //
        // POST: /Device/Edit/5

        [HttpPost]
        public ActionResult Edit(DeviceModel devicemodel)
        {
            
            

            if (ModelState.IsValid)
            {
                devicemodel.ModifyTime = DevTools.GetNowDateTime();
                db.Entry(devicemodel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(devicemodel);
        }

        //
        // GET: /Device/Delete/5

        public ActionResult Delete(int id = 0)
        {
            DeviceModel devicemodel = db.DeviceModels.Find(id);
            if (devicemodel == null)
            {
                return HttpNotFound();
            }
            return View(devicemodel);
        }

        //
        // POST: /Device/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            DeviceModel devicemodel = db.DeviceModels.Find(id);
            db.DeviceModels.Remove(devicemodel);
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