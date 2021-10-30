using OnDemandService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;

namespace OnDemandService.Areas.Professional.Controllers
{
    public class HomeController : Controller
    {
        private HomeServicesEntities db = new HomeServicesEntities();
        // GET: Professional/Home
        public ActionResult Index()
        {
            ViewBag.OngoingApp = 0;
            ViewBag.CompletedApp = 0;
            ViewBag.CancelledApp = 0;
            ViewBag.NewApp = 0;
            GetNotificationLoad();
            return View();
        }


        public JsonResult GetNotificationLoad()
        {
            //if(User.Identity.Name == "")
            //{
            //    NotificationModel notificationtemp = new NotificationModel();
            //    notificationtemp.Notification = "";
            //    notificationtemp.count = 0;

            //    return Json(notificationtemp, JsonRequestBehavior.AllowGet);
            //}
            //var userobj = db.Users.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).First();
            //var professional = db.PartnerProfessionals.Include(y => y.Pincode).ToList().Where(x => x.UserId == userobj.UserId).First();
            //var pincodes = db.Pincodes.ToList();
            //var subcategories = db.SubCategories.ToList();
            //var lstOrderReqs = db.OrderRequests.Include(o => o.Customer).Include(o => o.Order).Where(x => x.PartnerProfessional.PartnerProfessionalId == professional.PartnerProfessionalId &&
            //   (!x.NotifyStatus.HasValue || x.NotifyStatus.Value == 0)).ToList();

            //var isapprovedButNotAccepted = lstOrderReqs.Where(x => x.IsApproved.HasValue && x.IsApproved.Value && x.Status != "Accepted").ToList();
            //var newOrderslst = db.Orders.Where(x => x.IsLocked.HasValue == false || x.IsLocked.Value == 0).ToList();
            //var ids = newOrderslst.Select(x => x.OrderId).Except(lstOrderReqs.Select(x => x.Order.OrderId)).ToList();
            //var serviceprovided = db.ServiceProvideds.ToList().Where(x => x.PartnerProfessional.PartnerProfessionalId == professional.PartnerProfessionalId).ToList();
            //List<Order> tempnewOrder = new List<Order>();
            //foreach (var neworder in newOrderslst)
            //{
            //    int cntdata = serviceprovided.Where(x => x.SubCategory.SubCategoryId == neworder.SubCategoryId).Count();
            //    if (cntdata > 0)
            //        tempnewOrder.Add(neworder);
            //}
            //newOrderslst = tempnewOrder;

            //var newOrdersAll = (from ord in newOrderslst
            //                    join id in ids on ord.OrderId equals id
            //                    select ord).ToList();

            //var dbcustomers = db.Customers.ToList();
            //var notificatonshowns = db.IsNotificationShowns
            //    .Where(x => x.PartnerProfessional.PartnerProfessionalId == professional.PartnerProfessionalId).Select(x => x.Order.OrderId).ToList();
            //List<Order> newOrders = new List<Order>();
            //foreach (var ordd in newOrdersAll)
            //{
            //    if (notificatonshowns.Contains(ordd.OrderId))
            //        continue;
            //    var customer = dbcustomers.Where(x => x.CustomerId == ordd.Customer.CustomerId).First();
            //    Pincode orderpincode = pincodes.Where(x => x.PincodeId == customer.Pincode.PincodeId).First();
            //    Pincode profpincode = pincodes.Where(x => x.PincodeId == professional.Pincode.PincodeId).First();
            //    var distance = GetDistanceBetweenPoints(orderpincode.Latitude.Value, orderpincode.Longitude.Value, profpincode.Latitude.Value, profpincode.Longitude.Value);
            //    if (Math.Round(distance) <= 5)
            //    {
            //        newOrders.Add(ordd);
            //    }
            //}

            //List<OrderViewModel> orderViews = new List<OrderViewModel>();

            //foreach (var objs in isapprovedButNotAccepted)
            //{
            //    OrderViewModel order = new OrderViewModel();
            //    order.OrderId = objs.OrderId;
            //    order.CustomerName = objs.Customer.CustomerName;
            //    order.CustomerPhoneNumber = objs.Customer.MobileNo;
            //    order.CustomerAddress = objs.Customer.Address;
            //    order.Service = subcategories.Where(x => x.SubCategoryId == objs.Order.SubCategoryId).First().SubCategoryName;
            //    order.AppointmentDate = objs.SelectedDate.Value.ToString("dd-MMM-yyyy") + " " + objs.SelectedTime;
            //    orderViews.Add(order);

            //}
            //foreach (var objs in newOrders)
            //{
            //    OrderViewModel order = new OrderViewModel();
            //    order.OrderId = objs.OrderId;
            //    order.CustomerName = objs.Customer.CustomerName;
            //    order.Service = subcategories.Where(x => x.SubCategoryId == objs.SubCategoryId).First().SubCategoryName;
            //    orderViews.Add(order);

            //}

            //orderViews = orderViews.OrderByDescending(x => x.OrderId).ToList();
            //string outputdata = string.Empty;
            //foreach (var objs in orderViews)
            //{
            //    if (string.IsNullOrEmpty(objs.CustomerPhoneNumber))
            //    {
            //        outputdata = outputdata + " <li><a href=\"../../Professional/Orders/SelectNotify/" + objs.OrderId.ToString() + "\">" +
            //            "<small>" + objs.Service + "</small>" +
            //            "<strong><em>" + objs.CustomerName + "</em></strong>" +
            //            "</a></li> ";
            //    }
            //    else
            //    {
            //        outputdata = outputdata + " <li><a href=\"../../Professional/Orders/SelectNotify/" + objs.OrderId.ToString() + "\">" +
            //            "<small>" + objs.Service + "</small>" +
            //            "<strong><em>" + objs.CustomerName + "-" + objs.CustomerPhoneNumber + "</em></strong><br/>" +
            //            "<small>" + objs.CustomerAddress + "</small>" +
            //            "</a></li> ";
            //    }
            //}

            //if (orderViews.Count == 0)
            //{
            //    outputdata = " <li><a href=\"#\">" +
            //            "<strong>No Notification Found</strong>" +
            //            "</a></li> ";
            //}

            NotificationModel notification = new NotificationModel();
            notification.Notification = "";
            notification.count = 0;//orderViews.Count;

            return Json(notification, JsonRequestBehavior.AllowGet);
        }

        public double GetDistanceBetweenPoints(decimal lat1param, decimal long1param, decimal lat2param, decimal long2param)
        {
            double lat1 = Convert.ToDouble(lat1param);
            double long1 = Convert.ToDouble(long1param);
            double lat2 = Convert.ToDouble(lat2param);
            double long2 = Convert.ToDouble(long2param);

            double distance = 0;

            double dLat = (lat2 - lat1) / 180 * Math.PI;
            double dLong = (long2 - long1) / 180 * Math.PI;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                        + Math.Cos(lat2) * Math.Sin(dLong / 2) * Math.Sin(dLong / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            //Calculate radius of earth
            // For this you can assume any of the two points.
            double radiusE = 6378135; // Equatorial radius, in metres
            double radiusP = 6356750; // Polar Radius

            //Numerator part of function
            double nr = Math.Pow(radiusE * radiusP * Math.Cos(lat1 / 180 * Math.PI), 2);
            //Denominator part of the function
            double dr = Math.Pow(radiusE * Math.Cos(lat1 / 180 * Math.PI), 2)
                            + Math.Pow(radiusP * Math.Sin(lat1 / 180 * Math.PI), 2);
            double radius = Math.Sqrt(nr / dr);

            //Calaculate distance in metres.
            distance = radius * c;
            return distance;
        }


    }
}