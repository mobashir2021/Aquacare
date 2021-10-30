using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OnDemandService.Models;
using FiftyOne.Foundation;
using System.Text.RegularExpressions;

namespace OnDemandService.Controllers
{
    public class HomeController : Controller
    {
        HomeServicesEntities db = new HomeServicesEntities();
        // GET: Home
        public ActionResult Index(bool isActive = false)
        {
            if (isActive)
            {
                if (PincodeStaticModel.lstStaticPincodes == null || PincodeStaticModel.lstStaticPincodes.Count == 0)
                {
                    PincodeStaticModel.lstStaticPincodes = db.Pincodes.ToList();
                }
                ViewBag.Services = new SelectList(db.SubCategories, "SubCategoryId", "SubCategoryName");
                ViewBag.Pincodes = new SelectList(db.Pincodes, "PincodeId", "Pincode1");
                string roles = "";
                if (!string.IsNullOrEmpty(User.Identity.Name))
                {
                    Role role = db.Roles.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).FirstOrDefault();
                    if (role != null)
                        roles = role.Roles;
                    if (roles == "Customer")
                    {
                        ViewBag.UserLoggedin = true;
                    }
                }
                return View();
            }
            else
            {
                string userAgent = Request.ServerVariables["HTTP_USER_AGENT"];
                Regex OS = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                Regex device = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                string device_info = string.Empty;
                if (OS.IsMatch(userAgent))
                {
                    device_info = OS.Match(userAgent).Groups[0].Value;
                }
                if (device.IsMatch(userAgent.Substring(0, 4)))
                {
                    device_info += device.Match(userAgent).Groups[0].Value;
                }
                if (!string.IsNullOrEmpty(device_info))
                {
                    ViewBag.DeviceInfo = "You are using a Mobile device. " + device_info;
                    UserViewModel userView = new UserViewModel();
                    return View("LoginMobileUser", userView);
                }
                else
                {
                    ViewBag.DeviceInfo = "You are using a Desktop device.";
                    if (PincodeStaticModel.lstStaticPincodes == null || PincodeStaticModel.lstStaticPincodes.Count == 0)
                    {
                        PincodeStaticModel.lstStaticPincodes = db.Pincodes.ToList();
                    }
                    ViewBag.Services = new SelectList(db.SubCategories, "SubCategoryId", "SubCategoryName");
                    ViewBag.Pincodes = new SelectList(db.Pincodes, "PincodeId", "Pincode1");
                    string roles = "";
                    if (!string.IsNullOrEmpty(User.Identity.Name))
                    {
                        Role role = db.Roles.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).FirstOrDefault();
                        if (role != null)
                            roles = role.Roles;
                        if (roles == "Customer")
                        {
                            ViewBag.UserLoggedin = true;
                        }
                    }
                    return View();

                }

            }
            

        }

        public ActionResult LoginMobileUser(UserViewModel userViewModel)
        {
            if (userViewModel == null)
                userViewModel = new UserViewModel();
            return View(userViewModel);
        }

        [HttpPost]
        public ActionResult LoginMobileUser(UserViewModel userViewModel, string username, string password, string SubCategoryId, string Pincodes, string loginvalue, string registervalue)
        {
            if (registervalue != null && registervalue == "Register")
            {
                UserViewModel userView = new UserViewModel();
                userView.SubCategoryId = SubCategoryId;
                userView.PincodeId = Pincodes;
                ViewBag.Pincodes = new SelectList(db.Pincodes, "PincodeId", "Pincode1");
                return View("RegisterMobileUser", userView);
            }
            var userlist = db.Users.ToList();
            User user = db.Users.ToList().Where(x => x.UserName.ToLower().Replace(" ", "") == username.ToLower().Replace(" ", "") && x.Password == password).FirstOrDefault();

            string roles = "";
            if (user != null)
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == user.UserId).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Customer")
                {
                    FormsAuthentication.SetAuthCookie(user.UserId.ToString(), false);

                    if (!string.IsNullOrEmpty(SubCategoryId))
                    {
                        var customer = db.Customers.ToList().Where(x => x.UserId == user.UserId).First();
                        Order order = new Order();
                        order.SubCategoryId = Convert.ToInt32(SubCategoryId);
                        order.Customer = customer;
                        order.OrderPlacedOn = DateTime.Now;
                        order.OrderDate = DateTime.Now;
                        db.Orders.Add(order);
                        db.SaveChanges();
                        ViewBag.Message = "Order Submitted Successfully! Our professional will notify with date and time availibility.";
                        ViewBag.Status = true;
                        ViewBag.UserLoggedin = true;
                        return View("OrderPlaced");
                    }
                    else
                    {

                        ViewBag.UserLoggedin = true;
                        return RedirectToAction("Index", "Home", new { isActive = true });
                    }
                }

            }

            return View();
        }

        [HttpPost]
        public JsonResult GetPincodes(string Prefix)
        {
            //Note : you can bind same list from database  
            List<Pincode> ObjList = PincodeStaticModel.lstStaticPincodes.ToList();
            //Searching records from list using LINQ query  
            var CityList = (from N in ObjList
                            where N.Pincode1.StartsWith(Prefix)
                            select new { N.Pincode1 });
            return Json(CityList, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public ActionResult SubmitOrder(string firstname, string email, string mobileno, string address, string Pincodes, string Services)
        //{
        //    if(!string.IsNullOrEmpty(firstname) && !string.IsNullOrEmpty(mobileno) && !string.IsNullOrEmpty(address))
        //    {
        //        var customer = db.Customers.ToList().Where(x => x.MobileNo == mobileno && x.CustomerName.ToLower() == firstname.ToLower()).FirstOrDefault();
        //        Customer cs = new Customer();
        //        Pincode pincode = db.Pincodes.ToList().Where(x => x.Pincode1 == Pincodes).First();
        //        if (customer == null)
        //        {
        //            cs.CustomerName = firstname; cs.Address = address; cs.MobileNo = mobileno; cs.Pincode = pincode;
        //            db.Customers.Add(cs);
        //            db.SaveChanges();
        //        }
        //        else
        //        {
        //            cs = customer;
        //        }

        //        Order order = new Order();
        //        order.Customer = cs; order.IsActive = true;
        //        order.OrderDate = DateTime.Now;
        //        //order.OrderDate = Convert.ToDateTime(datepicker); order.OrderTime = timepicker;
        //        order.Pincode = pincode; order.SubCategoryId = Convert.ToInt32(Services); order.OrderPlacedOn = DateTime.Now;order.IsPaymentDone = false;
        //        db.Orders.Add(order);
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public ActionResult SubmitOrder(string Services)
        {
            UserViewModel userView = new UserViewModel();
            
            userView.SubCategoryId = Services;

            string roles = "";
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Customer")
                {

                    var customer = db.Customers.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).First();
                    Order order = new Order();
                    order.SubCategoryId = Convert.ToInt32(Services);
                    order.Customer = customer;
                    order.OrderPlacedOn = DateTime.Now;
                    order.OrderDate = DateTime.Now;
                    db.Orders.Add(order);
                    db.SaveChanges();
                    ViewBag.Message = "Order Submitted Successfully! Our professional will notify with date and time availibility.";
                    ViewBag.Status = true;
                    ViewBag.UserLoggedin = true;

                    return View("OrderPlaced");
                }
            }

            return RedirectToAction("LoginUser", userView);
        }

        [HttpGet]
        public ActionResult SubmitOrderGet(string Services)
        {
            UserViewModel userView = new UserViewModel();
            var subcagtegories = db.SubCategories.ToList();
            int serviceid = 0;
            var subcat = subcagtegories.Where(x => x.SubCategoryName.ToLower().Replace(" ", "") == Services.Replace(" ", "").ToLower()).FirstOrDefault();
            if (subcat == null)
            {
                ViewBag.Services = new SelectList(db.SubCategories, "SubCategoryId", "SubCategoryName");
                ViewBag.Pincodes = new SelectList(db.Pincodes, "PincodeId", "Pincode1");
                string rolestest = "";
                if (!string.IsNullOrEmpty(User.Identity.Name))
                {
                    Role role = db.Roles.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).FirstOrDefault();
                    if (role != null)
                        rolestest = role.Roles;
                    if (rolestest == "Customer")
                    {
                        ViewBag.UserLoggedin = true;
                    }
                }
                return View("Index",  new { isActive = true });
            }
            serviceid = subcat.SubCategoryId;

            userView.SubCategoryId = serviceid.ToString();

            string roles = "";
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Customer")
                {

                    var customer = db.Customers.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).First();
                    Order order = new Order();
                    order.SubCategoryId = Convert.ToInt32(Services);
                    order.Customer = customer;
                    order.OrderPlacedOn = DateTime.Now;
                    order.OrderDate = DateTime.Now;
                    db.Orders.Add(order);
                    db.SaveChanges();
                    ViewBag.Message = "Order Submitted Successfully! Our professional will notify with date and time availibility.";
                    ViewBag.Status = true;
                    ViewBag.UserLoggedin = true;

                    return View("OrderPlaced");
                }
            }

            return RedirectToAction("LoginUser", userView);
        }

        public JsonResult GetNotification()
        {
            string roles = "";
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Customer")
                {
                    var userobj = db.Users.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).First();
                    var customer = db.Customers.ToList().Where(x => x.UserId == userobj.UserId).First();

                    var lstOrderReqs = db.OrderRequests.Where(x => x.Customer.CustomerId == customer.CustomerId &&
                       (!x.IsCustomerNotify.HasValue || x.IsCustomerNotify.Value == 0)).ToList();

                    var partners = db.PartnerProfessionals.ToList();


                    var subcategories = db.SubCategories.ToList();
                    List<OrderViewModel> orderViews = new List<OrderViewModel>();

                    foreach (var objs in lstOrderReqs)
                    {
                        OrderViewModel order = new OrderViewModel();
                        order.OrderId = objs.OrderId;
                        order.OrderRequestId = objs.OrderRequestId;
                        if (objs.Status == "Accepted")
                        {
                            var prof = partners.Where(x => x.PartnerProfessionalId == objs.PartnerProfessional.PartnerProfessionalId).First();
                            order.PartnerProfName = prof.PartnerName;
                            order.PartnerphoneNumber = prof.MobileNo;
                            order.AppointmentDate = objs.SelectedDate.Value.ToString("dd-MMM-yyyy") + " " + objs.SelectedTime;
                        }

                        order.Service = subcategories.Where(x => x.SubCategoryId == objs.Order.SubCategoryId).First().SubCategoryName;

                        orderViews.Add(order);

                    }

                    orderViews = orderViews.OrderByDescending(x => x.OrderId).ToList();
                    string outputdata = string.Empty;
                    foreach (var objs in orderViews)
                    {
                        if (string.IsNullOrEmpty(objs.PartnerphoneNumber))
                        {
                            outputdata = outputdata + " <li><a href=\"/Home/SelectNotify/" + objs.OrderId.ToString() + "-" + objs.OrderRequestId.ToString() + "\">" +
                                "<small>" + objs.Service + "</small>" +
                                "<strong><em>" + objs.PartnerProfName + "</em></strong>" +
                                "</a></li> ";
                        }
                        else
                        {
                            outputdata = outputdata + " <li><a href=\"/Home/SelectNotify/" + objs.OrderId.ToString() + "-" + objs.OrderRequestId.ToString() + "\">" +
                                "<small>" + objs.Service + "</small>" +
                                "<strong><em>" + objs.PartnerProfName + "-" + objs.PartnerphoneNumber + "</em></strong><br/>" +
                                "</a></li> ";
                        }
                    }

                    if (orderViews.Count == 0)
                    {
                        outputdata = " <li><a href=\"#\">" +
                                "<strong>No Notification Found</strong>" +
                                "</a></li> ";
                    }

                    NotificationModel notification = new NotificationModel();
                    notification.Notification = outputdata;
                    notification.count = orderViews.Count;

                    return Json(notification, JsonRequestBehavior.AllowGet);
                }
            }
            NotificationModel model = new NotificationModel();
            model.Notification = "";
            model.count = 0;
            return Json(model, JsonRequestBehavior.AllowGet);

        }

        public ActionResult SelectNotify(string id)
        {
            string[] arr = id.Split('-');
            int orderid = Convert.ToInt32(arr[0]);
            int orderrequestid = Convert.ToInt32(arr[1]);
            
            var orderrequest = db.OrderRequests.Where(x => x.OrderRequestId == orderrequestid).First();

            
                orderrequest.IsCustomerNotify = 1;
                db.Entry(orderrequest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Orders", "Home");
                //return View("ApprovedAppointments");


        }

        public ActionResult AboutUs()
        {
            string roles = "";
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Customer")
                {
                    ViewBag.UserLoggedin = true;
                }
            }
            return View();
        }

        public ActionResult ContactUs()
        {
            string roles = "";
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Customer")
                {
                    ViewBag.UserLoggedin = true;
                }
            }
            return View();
        }

        public ActionResult Services()
        {
            string roles = "";
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Customer")
                {
                    ViewBag.UserLoggedin = true;
                }
            }
            return View();
        }

        public ActionResult TrackServiceStatus()
        {
            string roles = "";
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Customer")
                {
                    ViewBag.UserLoggedin = true;
                }
            }
            return View();
        }

        public ActionResult Orders()
        {
            string roles = "";
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Customer")
                {
                    ViewBag.UserLoggedin = true;
                }
            }

            return View();
        }

        public ActionResult ProfessionalDetails(int id)
        {
            string roles = "";
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Customer")
                {
                    ViewBag.UserLoggedin = true;
                }
            }
            OrderViewModel order = new OrderViewModel();
            var orderrequest = db.OrderRequests.ToList().Where(x => x.OrderId == id && x.IsApproved.HasValue && x.IsApproved.Value).First();
            var partner = db.PartnerProfessionals.ToList().Where(x => x.PartnerProfessionalId == orderrequest.PartnerProfessional.PartnerProfessionalId).First();
            order.OrderId = id;
            order.PartnerProfName = partner.PartnerName; order.PartnerphoneNumber = partner.MobileNo;
            order.AppointmentDate = orderrequest.SelectedDate.Value.ToString("dd-MMM-yyyy") + " " + orderrequest.SelectedTime;


            return View(order);
        }

        public ActionResult ProfessionalDetailsJson()
        {
            var orders = db.Orders.ToList();
            var userobj = db.Users.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).First();
            var customerdata = db.Customers.ToList().Where(x => x.UserId == userobj.UserId).First();

            List<OrderViewModel> data = new List<OrderViewModel>();
            var resultdata = orders.Where(x => x.CustomerId == customerdata.CustomerId).ToList();
            var orderrequestdetails = db.OrderRequests.ToList();
            int ij = 1;
            foreach (var tempdata in resultdata)
            {

                OrderViewModel model = new OrderViewModel();
                model.SNo = ij;
                var subcats = db.SubCategories.ToList().Where(x => x.SubCategoryId == tempdata.SubCategoryId).First();
                model.Service = subcats.SubCategoryName;
                model.Amount = subcats.Price.Value.ToString();
                model.PlacedOn = tempdata.OrderPlacedOn.Value.ToString("dd-MMM-yyyy HH:mm");
                //model.CustomerName = tempdata.Customer.CustomerName;
                //model.AppointmentDate = tempdata.OrderPlacedOn.Value.ToString("dd-MMM-yyyy") + " " + tempdata.OrderTime.ToString();
                model.OrderId = tempdata.OrderId;
                if (tempdata.IsDelivered.HasValue && tempdata.IsDelivered.Value == true)
                    model.Status = "Completed";
                else if (tempdata.IsCancelled.HasValue && tempdata.IsCancelled.Value == true)
                    model.Status = "Cancelled";
                else
                {
                    var orderrqs = orderrequestdetails.Where(x => x.OrderId == tempdata.OrderId).ToList();
                    int cnt = orderrqs.Where(x => x.Status == "Accepted").Count();
                    if (cnt > 0)
                    {
                        //see professional details
                        model.Status = "Accepted";
                    }
                    else
                    {
                        model.Status = "SeeRequests";
                    }

                }
                model.StatusValue = model.Status + "--" + model.OrderId.ToString();
                model.Area = tempdata.Pincode.Pincode1;
                data.Add(model);

            }


            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ApproveProfessional(string id)
        {
            string[] arr = id.Split('-');
            int orderid = Convert.ToInt32(arr[0]);
            int professionalid = Convert.ToInt32(arr[1]);
            var orderFull = db.OrderRequests.Where(x => x.Order.OrderId == orderid).ToList();
            var orderrequest = orderFull.Where(x => x.PartnerProfessional.PartnerProfessionalId == professionalid).First();
            var exceptRequest = orderFull.Where(x => x.OrderRequestId != orderrequest.OrderRequestId).ToList();
            //OrderRequest orderrequest = db.OrderRequests.ToList().Where(x => x.OrderId == orderid && x.PartnerProfessional.PartnerProfessionalId == professionalid).First();
            orderrequest.IsApproved = true;

            db.Entry(orderrequest).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            foreach (var objectreq in exceptRequest)
            {
                db.OrderRequests.Remove(objectreq);
            }
            db.SaveChanges();

            var order = db.Orders.Where(x => x.OrderId == orderrequest.OrderId).First();
            order.IsLocked = 1;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();


            ViewBag.Message = "Your order has been approved and send for acceptance.Kindly wait for notification";
            string roles = "";
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Customer")
                {
                    ViewBag.UserLoggedin = true;
                }
            }
            return View("OrderPlaced");
        }

        public ActionResult OrderRequests(int id)
        {
            ViewBag.OrderId = id;
            string roles = "";
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Customer")
                {
                    ViewBag.UserLoggedin = true;
                }
            }
            return View();
        }

        public ActionResult OrderRequestJson(int id)
        {
            var resultdata = db.OrderRequests.ToList().Where(x => x.OrderId == id).ToList();
            var profs = db.PartnerProfessionals.ToList();
            List<OrderViewModel> data = new List<OrderViewModel>();
            
            
            int ij = 1;
            foreach (var tempdata in resultdata)
            {

                OrderViewModel model = new OrderViewModel();
                model.SNo = ij;
                var partner = profs.Where(x => x.PartnerProfessionalId == tempdata.PartnerProfessional.PartnerProfessionalId).First();
                
                model.OrderId = tempdata.OrderId;
                model.PartnerProfName = partner.PartnerName;
                model.AppointmentDate = tempdata.SelectedDate.Value.ToString("dd-MMM-yyyy") + " " + tempdata.SelectedTime;
                model.Orderwithpartner = tempdata.OrderId.ToString() + "-" + partner.PartnerProfessionalId.ToString();
                data.Add(model);

            }


            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OrdersJson()
        {
            var orders = db.Orders.ToList();
            var userobj = db.Users.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).First();
            var customerdata = db.Customers.ToList().Where(x => x.UserId == userobj.UserId).First();
            
            List<OrderViewModel> data = new List<OrderViewModel>();
            var resultdata = orders.Where(x => x.CustomerId == customerdata.CustomerId).ToList();
            var orderrequestdetails = db.OrderRequests.ToList();
            int ij = 1;
            foreach (var tempdata in resultdata)
            {
                
                OrderViewModel model = new OrderViewModel();
                model.SNo = ij;
                var subcats = db.SubCategories.ToList().Where(x => x.SubCategoryId == tempdata.SubCategoryId).First();
                model.Service = subcats.SubCategoryName;
                model.Amount = subcats.Price.Value.ToString();
                model.PlacedOn = tempdata.OrderPlacedOn.Value.ToString("dd-MMM-yyyy HH:mm");
                //model.CustomerName = tempdata.Customer.CustomerName;
                //model.AppointmentDate = tempdata.OrderPlacedOn.Value.ToString("dd-MMM-yyyy") + " " + tempdata.OrderTime.ToString();
                model.OrderId = tempdata.OrderId;
                if (tempdata.IsDelivered.HasValue && tempdata.IsDelivered.Value == true)
                    model.Status = "Completed";
                else if (tempdata.IsCancelled.HasValue && tempdata.IsCancelled.Value == true)
                    model.Status = "Cancelled";
                else
                {
                    var orderrqs = orderrequestdetails.Where(x => x.OrderId == tempdata.OrderId).ToList();
                    int cnt = orderrqs.Where(x => x.Status == "Accepted").Count();
                    if (cnt > 0)
                    {
                        //see professional details
                        model.Status = "Accepted";
                    }
                    else
                    {
                        model.Status = "See Requests";
                    }

                }
                model.StatusValue = model.Status + "--" + model.OrderId.ToString();
                model.Area = customerdata.Pincode.Pincode1;
                data.Add(model);
                
            }


            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoginUser(UserViewModel userViewModel)
        {
            if (userViewModel == null)
                userViewModel = new UserViewModel();
            return View(userViewModel);
        }

        [HttpPost]
        public ActionResult LoginUser(UserViewModel userViewModel, string username, string password, string SubCategoryId, string Pincodes, string loginvalue, string registervalue)
        {
            if(registervalue != null && registervalue == "Register")
            {
                UserViewModel userView = new UserViewModel();
                userView.SubCategoryId = SubCategoryId;
                userView.PincodeId = Pincodes;
                ViewBag.Pincodes = new SelectList(db.Pincodes, "PincodeId", "Pincode1");
                return View("RegisterUser", userView);
            }
            var userlist = db.Users.ToList();
            User user = db.Users.ToList().Where(x => x.UserName.ToLower().Replace(" ", "") == username.ToLower().Replace(" ", "") && x.Password == password).FirstOrDefault();
            
            string roles = "";
            if (user != null)
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == user.UserId).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Customer")
                {
                    FormsAuthentication.SetAuthCookie(user.UserId.ToString(), false);
                    
                    if (!string.IsNullOrEmpty(SubCategoryId))
                    {
                        var customer = db.Customers.ToList().Where(x => x.UserId == user.UserId).First();
                        Order order = new Order();
                        order.SubCategoryId = Convert.ToInt32(SubCategoryId);
                        order.Customer = customer;
                        order.OrderPlacedOn = DateTime.Now;
                        order.OrderDate = DateTime.Now;
                        db.Orders.Add(order);
                        db.SaveChanges();
                        ViewBag.Message = "Order Submitted Successfully! Our professional will notify with date and time availibility.";
                        ViewBag.Status = true;
                        ViewBag.UserLoggedin = true;
                        return View("OrderPlaced");
                    }
                    else
                    {

                        ViewBag.UserLoggedin = true;
                        return RedirectToAction("Index", new { isActive = true });
                    }
                }
                
            }

            return View();
        }

        public ActionResult OrderPlaced()
        {
            string roles = "";
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Customer")
                {
                    ViewBag.UserLoggedin = true;
                }
            }
            return View();
        }

        public ActionResult RegisterMobileUser()
        {
            ViewBag.Pincodes = new SelectList(db.Pincodes, "PincodeId", "Pincode1");
            return View();
        }

        [HttpPost]
        public ActionResult RegisterMobileUser(UserViewModel userViewModel, string Pincodes)
        {

            string Message = "";
            bool isValid = true;
            if (userViewModel != null)
            {
                User user = db.Users.Where(x => x.UserName.ToLower().Replace(" ", "") == userViewModel.userName.ToLower().Replace(" ", "")).FirstOrDefault();
                if (user != null)
                {
                    isValid = false;
                    Message = "Username Already exists";
                    ViewBag.StatusMessage = Message;
                    ViewBag.Status = false;
                }
                else if (string.IsNullOrEmpty(userViewModel.Password))
                {
                    isValid = false;
                    Message = "Kindly enter Password";
                    ViewBag.StatusMessage = Message;
                    ViewBag.Status = false;
                }
                else if (string.IsNullOrEmpty(userViewModel.ConfirmPassword) && userViewModel.Password != userViewModel.ConfirmPassword)
                {
                    isValid = false;
                    Message = "Confirm Password does not match";
                    ViewBag.StatusMessage = Message;
                    ViewBag.Status = false;
                }
                else if (string.IsNullOrEmpty(userViewModel.MobileNo))
                {
                    isValid = false;
                    Message = "Kindly provide Mobile No";
                    ViewBag.StatusMessage = Message;
                    ViewBag.Status = false;
                }
                else
                {
                    User user1 = new User();
                    user1.Email = userViewModel.Email;
                    user1.Password = userViewModel.Password;
                    user1.UserName = userViewModel.userName;
                    db.Users.Add(user1);
                    db.SaveChanges();

                    Role role = new Role();
                    role.Roles = "Customer";
                    role.User = user1;
                    db.Roles.Add(role);
                    db.SaveChanges();



                    //Customer cs = new Customer();
                    //cs.Address = userViewModel.Address;
                    //cs.CustomerName = userViewModel.Name;
                    //cs.MobileNo = userViewModel.MobileNo;

                    Pincode pincode = db.Pincodes.ToList().Where(x => x.PincodeId == Convert.ToInt32(Pincodes)).FirstOrDefault();
                    //cs.Pincode = pincode;
                    //cs.UserId = user1.UserId;

                    Customer customer = new Customer();
                    customer.Address = userViewModel.Address;
                    customer.CustomerName = userViewModel.Name;
                    customer.MobileNo = userViewModel.MobileNo;
                    customer.UserId = user1.UserId;
                    customer.Pincode = pincode;
                    db.Customers.Add(customer);
                    db.SaveChanges();
                    ViewBag.UserLoggedin = true;
                    FormsAuthentication.SetAuthCookie(user1.UserId.ToString(), false);

                    if (!string.IsNullOrEmpty(userViewModel.SubCategoryId))
                    {
                        Order order = new Order();
                        order.OrderDate = DateTime.Now;
                        order.SubCategoryId = Convert.ToInt32(userViewModel.SubCategoryId);
                        order.Customer = customer;
                        order.OrderPlacedOn = DateTime.Now;

                        db.Orders.Add(order);
                        db.SaveChanges();
                        ViewBag.Message = "Order Submitted Successfully! Our professional will notify with date and time availibility.";
                        ViewBag.Status = true;
                        return View("OrderPlaced");
                    }
                    else
                    {
                        return View("Orders");
                    }







                }
            }


            ViewBag.Pincodes = new SelectList(db.Pincodes, "PincodeId", "Pincode1");
            return View(userViewModel);
        }

        public ActionResult RegisterUser()
        {
            ViewBag.Pincodes = new SelectList(db.Pincodes, "PincodeId", "Pincode1");
            return View();
        }

        [HttpPost]
        public ActionResult RegisterUser(UserViewModel userViewModel, string Pincodes)
        {

            string Message = "";
            bool isValid = true;
            if (userViewModel != null)
            {
                User user = db.Users.Where(x => x.UserName.ToLower().Replace(" ","") == userViewModel.userName.ToLower().Replace(" ", "")).FirstOrDefault();
                if (user != null)
                {
                    isValid = false;
                    Message = "Username Already exists";
                    ViewBag.StatusMessage = Message;
                    ViewBag.Status = false;
                }
                else if(string.IsNullOrEmpty(userViewModel.Password))
                {
                    isValid = false;
                    Message = "Kindly enter Password";
                    ViewBag.StatusMessage = Message;
                    ViewBag.Status = false;
                }
                else if(string.IsNullOrEmpty(userViewModel.ConfirmPassword) && userViewModel.Password != userViewModel.ConfirmPassword)
                {
                    isValid = false;
                    Message = "Confirm Password does not match";
                    ViewBag.StatusMessage = Message;
                    ViewBag.Status = false;
                }
                else if(string.IsNullOrEmpty(userViewModel.MobileNo))
                {
                    isValid = false;
                    Message = "Kindly provide Mobile No";
                    ViewBag.StatusMessage = Message;
                    ViewBag.Status = false;
                }
                else
                {
                    User user1 = new User();
                    user1.Email = userViewModel.Email;
                    user1.Password = userViewModel.Password;
                    user1.UserName = userViewModel.userName;
                    db.Users.Add(user1);
                    db.SaveChanges();

                    Role role = new Role();
                    role.Roles = "Customer";
                    role.User = user1;
                    db.Roles.Add(role);
                    db.SaveChanges();

                    

                    //Customer cs = new Customer();
                    //cs.Address = userViewModel.Address;
                    //cs.CustomerName = userViewModel.Name;
                    //cs.MobileNo = userViewModel.MobileNo;

                    Pincode pincode = db.Pincodes.ToList().Where(x => x.PincodeId == Convert.ToInt32(Pincodes)).FirstOrDefault();
                    //cs.Pincode = pincode;
                    //cs.UserId = user1.UserId;

                    Customer customer = new Customer();
                    customer.Address = userViewModel.Address;
                    customer.CustomerName = userViewModel.Name;
                    customer.MobileNo = userViewModel.MobileNo;
                    customer.UserId = user1.UserId;
                    customer.Pincode = pincode;
                    db.Customers.Add(customer);
                    db.SaveChanges();
                    ViewBag.UserLoggedin = true;
                    FormsAuthentication.SetAuthCookie(user1.UserId.ToString(), false);

                    if (!string.IsNullOrEmpty(userViewModel.SubCategoryId))
                    {
                        Order order = new Order();
                        order.OrderDate = DateTime.Now;
                        order.SubCategoryId = Convert.ToInt32(userViewModel.SubCategoryId);
                        order.Customer = customer;
                        order.OrderPlacedOn = DateTime.Now;

                        db.Orders.Add(order);
                        db.SaveChanges();
                        ViewBag.Message = "Order Submitted Successfully! Our professional will notify with date and time availibility.";
                        ViewBag.Status = true;
                        return View("OrderPlaced");
                    }
                    else
                    {
                        return View("Orders");
                    }

                    
                    
                    

                    

                }
            }


            ViewBag.Pincodes = new SelectList(db.Pincodes, "PincodeId", "Pincode1");
            return View(userViewModel);
        }

        public ActionResult Logout()
        {

            FormsAuthentication.SignOut();
            if (PincodeStaticModel.lstStaticPincodes == null || PincodeStaticModel.lstStaticPincodes.Count == 0)
            {
                PincodeStaticModel.lstStaticPincodes = db.Pincodes.ToList();
            }
            ViewBag.Services = new SelectList(db.SubCategories, "SubCategoryId", "SubCategoryName");
            ViewBag.Pincodes = new SelectList(db.Pincodes, "PincodeId", "Pincode1");
            return RedirectToAction("Index");
        }
    }
}