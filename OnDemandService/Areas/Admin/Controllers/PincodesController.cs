using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using OnDemandService.Models;

namespace OnDemandService.Areas.Admin.Controllers
{
    public class PincodesController : Controller
    {
        public static string apiKey = "AIzaSyCIevyFBsNxcgXbU8VBM06q59CndlwZqoA";

        private HomeServicesEntities db = new HomeServicesEntities();

        // GET: Pincodes
        public ActionResult Index()
        {
            var pincodes = db.Pincodes.Include(p => p.City);
            return View(pincodes.ToList());
        }

        // GET: Pincodes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pincode pincode = db.Pincodes.Find(id);
            if (pincode == null)
            {
                return HttpNotFound();
            }
            return View(pincode);
        }

        // GET: Pincodes/Create
        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "CityName");
            return View();
        }

        public string GetLatLngByPincode(string pincode)
        {
            string locationURL = "https://maps.googleapis.com/maps/api/geocode/json?address=" + pincode + "&key=" + apiKey;
            string r = new System.Net.WebClient().DownloadString(locationURL);
            dynamic result = JObject.Parse(r);
            string latlng = result.results[0].geometry.location.lat + "," + result.results[0].geometry.location.lng;
            return latlng;
        }

        // POST: Pincodes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PincodeId,Pincode1,CityId")] Pincode pincode)
        {
            if (ModelState.IsValid)
            {
                //string latlong = GetLatLngByPincode(pincode.Pincode1);
                //string[] arr = latlong.Split(',');
                //pincode.Latitude = Convert.ToDecimal(12.956629);
                //pincode.Longitude = 
                db.Pincodes.Add(pincode);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(db.Cities, "CityId", "CityName", pincode.CityId);
            return View(pincode);
        }

        // GET: Pincodes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pincode pincode = db.Pincodes.Find(id);
            if (pincode == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "CityName", pincode.CityId);
            return View(pincode);
        }

        // POST: Pincodes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PincodeId,Pincode1,Latitude,Longitude,CityId")] Pincode pincode)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pincode).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "CityName", pincode.CityId);
            return View(pincode);
        }

        // GET: Pincodes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pincode pincode = db.Pincodes.Find(id);
            if (pincode == null)
            {
                return HttpNotFound();
            }
            return View(pincode);
        }

        // POST: Pincodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pincode pincode = db.Pincodes.Find(id);
            db.Pincodes.Remove(pincode);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
