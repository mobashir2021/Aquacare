using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnDemandService.Models;

namespace OnDemandService.Areas.Admin.Controllers
{
    public class OrdersController : Controller
    {
        private HomeServicesEntities db = new HomeServicesEntities();

        // GET: Admin/Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Customer).Include(o => o.PaymentMethod).Include(o => o.Pincode);
            return View(orders.ToList());
        }

        // GET: Admin/Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Admin/Orders/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName");
            ViewBag.PaymentId = new SelectList(db.PaymentMethods, "Paymentid", "PaymentType");
            ViewBag.GuestPincodeId = new SelectList(db.Pincodes, "PincodeId", "Pincode1");
            return View();
        }

        // POST: Admin/Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,SubCategoryId,OrderDate,OrderTime,CustomerId,GuestName,GuestPhone,GuestEmail,GuestPincodeId,GuestCityId,IsStarted,IsDelivered,IsPaymentDone,DeliveredDateTime,IsCancelled,Rating,Description,OrderPlacedOn,IsActive,IsGoingOn,PaymentId")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", order.CustomerId);
            ViewBag.PaymentId = new SelectList(db.PaymentMethods, "Paymentid", "PaymentType", order.PaymentId);
            ViewBag.GuestPincodeId = new SelectList(db.Pincodes, "PincodeId", "Pincode1", order.GuestPincodeId);
            return View(order);
        }

        // GET: Admin/Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", order.CustomerId);
            ViewBag.PaymentId = new SelectList(db.PaymentMethods, "Paymentid", "PaymentType", order.PaymentId);
            ViewBag.GuestPincodeId = new SelectList(db.Pincodes, "PincodeId", "Pincode1", order.GuestPincodeId);
            return View(order);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderId,SubCategoryId,OrderDate,OrderTime,CustomerId,GuestName,GuestPhone,GuestEmail,GuestPincodeId,GuestCityId,IsStarted,IsDelivered,IsPaymentDone,DeliveredDateTime,IsCancelled,Rating,Description,OrderPlacedOn,IsActive,IsGoingOn,PaymentId")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", order.CustomerId);
            ViewBag.PaymentId = new SelectList(db.PaymentMethods, "Paymentid", "PaymentType", order.PaymentId);
            ViewBag.GuestPincodeId = new SelectList(db.Pincodes, "PincodeId", "Pincode1", order.GuestPincodeId);
            return View(order);
        }

        // GET: Admin/Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
