using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using OnDemandService.Models;
using System.Data.Entity;
//using PushSharp.Google;

namespace OnDemandService.Areas.Admin.Controllers
{
    public class LeadsController : Controller
    {
        private string serverKey = "AAAAEN82FKk:APA91bFFhsELKMNfs7XnrlfmfonD8mwyxKOqU_sKPcEeOSuiKCG6OOUJKi6gjQTx4p6uvr1nVgRmQyOiiw7DQSjxAy4KkD-Pdoex1xI67LrHKroE11Hilus3HRmfs1IT0eRgvjlXvQFO";
        private string senderId = "72464340137";
        private string webAddr = "https://fcm.googleapis.com/fcm/send";
        HomeServicesEntities db = new HomeServicesEntities();
        DatabaseOperations dbOps = new DatabaseOperations();
        // GET: Admin/Leads
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewLeads()
        {
            ViewBag.City = new SelectList(db.Cities, "CityId", "CityName");
            ViewBag.Categories = new SelectList(db.SubCategories, "SubCategoryId", "SubCategoryName");
            return View();
        }

        [HttpPost]
        public ActionResult NewLeads(LeadGenerationViewModel leadGeneration, int Serviceprovided, int City)
        {
            leadGeneration.Serviceprovided = Serviceprovided;
            leadGeneration.Cityid = City;
            leadGeneration.LeadStatus = "New";
            dbOps.AddLeads(leadGeneration);

            return RedirectToAction("ViewLeads");
        }

        public ActionResult DeductBalance()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DeductBalance(string Username, string Rechargeamount)
        {
            var user = db.Users.Where(x => x.UserName.ToLower().Replace(" ", "") == Username.ToLower().Replace(" ", "")).FirstOrDefault();
            if (user != null)
            {
                var prof = db.PartnerProfessionals.Where(x => x.UserId == user.UserId).First();
                RechargeOrder order = new RechargeOrder();
                order.Professionalid = prof.PartnerProfessionalId;
                order.RechargeAmount = 0 - Convert.ToInt32(Rechargeamount);
                order.RechargeDate = DateTime.Now;
                order.PaymentTypeId = 2;
                order.RechargeStatus = "Success";
                db.RechargeOrders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("DeductBalance");
            }
        }

        public ActionResult Manualrecharge()
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult Manualrecharge(string Username, string Rechargeamount)
        {
            var user = db.Users.Where(x => x.UserName.ToLower().Replace(" ", "") == Username.ToLower().Replace(" ", "")).FirstOrDefault();
            if(user != null)
            {
                var prof = db.PartnerProfessionals.Where(x => x.UserId == user.UserId).First();
                RechargeOrder order = new RechargeOrder();
                order.Professionalid = prof.PartnerProfessionalId;
                order.RechargeAmount = Convert.ToInt32(Rechargeamount);
                order.RechargeDate = DateTime.Now;
                order.RechargeStatus = "Success";
                order.PaymentTypeId = 1;  //1 for Manual Recharge //2 for deduction
                db.RechargeOrders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Manualrecharge");
            }
        }

        public ActionResult Viewrecharge()
        {

            return View();
        }

        public JsonResult ViewRechargeJson()
        {
            var resultdata = db.RechargeOrders.ToList();
            
            var partners = db.PartnerProfessionals.ToList();

            List<RechargeViewModelData> data = new List<RechargeViewModelData>();
            int ij = 1;
            foreach (var tempdata in resultdata)
            {
                
                PartnerProfessional partner = partners.Where(x => x.PartnerProfessionalId == tempdata.Professionalid).FirstOrDefault();
                RechargeViewModelData rc = new RechargeViewModelData();
                rc.RechargeOrderId = tempdata.RechargeOrderId;
                rc.Rechargeamount = tempdata.RechargeAmount.Value;
                rc.Partnername = partner.PartnerName;
                if (tempdata.PaymentTypeId.HasValue)
                {
                    rc.Paymentstatus = tempdata.PaymentTypeId.Value == 1 ? "Admin Recharge" : "Recharge Deduction";
                }
                else
                {
                    rc.Paymentstatus = "App Vendor Recharge";
                }
                rc.Paymentdate = tempdata.RechargeDate.HasValue ? tempdata.RechargeDate.Value.ToString("dd-MMM-yyyy hh:mm") : "";
                data.Add(rc);

            }


            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }





        [HttpPost]
        public JsonResult GetUsernameAuto(string Prefix)
        {
            Prefix = Prefix.ToLower().Replace(" ", "");
            List<User> users = new List<User>();
            var totalusers = db.Users.ToList();
            var roles = db.Roles.ToList();
            foreach(var data in totalusers)
            {
                int count = roles.Where(x => x.UserId == data.UserId && x.Roles == "Professional").Count();
                if(count > 0)
                {
                    users.Add(data);
                }
            }


            
            //Searching records from list using LINQ query  
            var catList = (
                            from adata in users
                            where adata.UserName.ToLower().Contains(Prefix)
                            select new { adata.UserName });
            return Json(catList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OngoingAppointment()
        {
            var resultdata = db.LeadDetails.Where(x => x.LeadStatus == "Ongoing").ToList();
            var customers = db.Customers.ToList();
            var partners = db.PartnerProfessionals.ToList();

            List<OrderVMData> data = new List<OrderVMData>();
            int ij = 1;
            foreach (var tempdata in resultdata)
            {
                Customer cs = customers.Where(x => x.CustomerId == tempdata.CustomerId).FirstOrDefault();
                PartnerProfessional partner = partners.Where(x => x.PartnerProfessionalId == tempdata.ProfessionalId).FirstOrDefault();
                OrderVMData model = new OrderVMData();
                model.SNo = ij;
                var subcats = db.SubCategories.ToList().Where(x => x.SubCategoryId == tempdata.SubCategoryId).First();
                model.Service = subcats.SubCategoryName;
                model.Amount = subcats.LeadCredit.Value.ToString();
                if(cs != null)
                {
                    model.CustomerName = cs.CustomerName;
                    model.CustomerPhoneNumber = cs.MobileNo;
                    model.Area = cs.Pincodedata;
                }
                if(partner != null)
                {
                    model.PartnerProfName = partner.PartnerName;
                }
                
                model.AppointmentDate = tempdata.GeneratedDateTime.HasValue ? tempdata.GeneratedDateTime.Value.ToString("dd-MMM-yyyy hh:mm") : "";
                model.OrderId = tempdata.LeadDetailsId;
                model.Status = "Ongoing";
                
                data.Add(model);
                
            }


            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult NewAppointmentJson()
        {
            var resultdata = db.LeadDetails.Where(x => x.LeadStatus == "New").ToList();
            var customers = db.Customers.ToList();
            //var partners = db.PartnerProfessionals.ToList();

            List<OrderVMData> data = new List<OrderVMData>();
            int ij = 1;
            foreach (var tempdata in resultdata)
            {
                Customer cs = customers.Where(x => x.CustomerId == tempdata.CustomerId).FirstOrDefault();
                //PartnerProfessional partner = partners.Where(x => x.PartnerProfessionalId == tempdata.ProfessionalId).FirstOrDefault();
                OrderVMData model = new OrderVMData();
                model.SNo = ij;
                var subcats = db.SubCategories.ToList().Where(x => x.SubCategoryId == tempdata.SubCategoryId).First();
                model.Service = subcats.SubCategoryName;
                model.Amount = subcats.LeadCredit.Value.ToString();
                if(cs != null)
                {
                    model.CustomerName = cs.CustomerName;
                    model.CustomerPhoneNumber = cs.MobileNo;
                    model.Area = cs.Pincodedata;
                }
                
                //model.PartnerProfName = partner.PartnerName;
                model.AppointmentDate = tempdata.GeneratedDateTime.HasValue ? tempdata.GeneratedDateTime.Value.ToString("dd-MMM-yyyy hh:mm") : "";
                model.OrderId = tempdata.LeadDetailsId;
                model.Status = "Ongoing";
                
                data.Add(model);

            }


            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CompletedAppointmentJson()
        {
            var resultdata = db.LeadDetails.Where(x => x.LeadStatus == "Complete").ToList();
            var customers = db.Customers.ToList();
            var partners = db.PartnerProfessionals.ToList();

            List<OrderVMData> data = new List<OrderVMData>();
            int ij = 1;
            foreach (var tempdata in resultdata)
            {
                Customer cs = customers.Where(x => x.CustomerId == tempdata.CustomerId).FirstOrDefault();
                PartnerProfessional partner = partners.Where(x => x.PartnerProfessionalId == tempdata.ProfessionalId).FirstOrDefault();
                OrderVMData model = new OrderVMData();
                model.SNo = ij;
                var subcats = db.SubCategories.ToList().Where(x => x.SubCategoryId == tempdata.SubCategoryId).First();
                model.Service = subcats.SubCategoryName;
                model.TotalCost = tempdata.TotalCost;
                model.SpareParts = tempdata.CompletedSpareParts;
                model.Amount = subcats.LeadCredit.Value.ToString();
                if (cs != null)
                {
                    model.CustomerName = cs.CustomerName;
                    model.CustomerPhoneNumber = cs.MobileNo;
                    model.Area = cs.Pincodedata;
                }
                if (partner != null)
                {
                    model.PartnerProfName = partner.PartnerName;
                }
                model.AppointmentDate = tempdata.GeneratedDateTime.HasValue ? tempdata.GeneratedDateTime.Value.ToString("dd-MMM-yyyy hh:mm") : "";
                model.OrderId = tempdata.LeadDetailsId;
                model.Status = "Ongoing";
                
                data.Add(model);

            }


            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeniedAppointmentJson()
        {
            var resultdata = db.LeadDetails.Where(x => x.LeadStatus == "Denied").ToList();
            var customers = db.Customers.ToList();
            var partners = db.PartnerProfessionals.ToList();

            List<OrderVMData> data = new List<OrderVMData>();
            int ij = 1;
            foreach (var tempdata in resultdata)
            {
                Customer cs = customers.Where(x => x.CustomerId == tempdata.CustomerId).FirstOrDefault();
                PartnerProfessional partner = partners.Where(x => x.PartnerProfessionalId == tempdata.ProfessionalId).FirstOrDefault();
                OrderVMData model = new OrderVMData();
                model.SNo = ij;
                var subcats = db.SubCategories.ToList().Where(x => x.SubCategoryId == tempdata.SubCategoryId).First();
                model.Service = subcats.SubCategoryName;
                //model.TotalCost = tempdata.TotalCost;
                //model.SpareParts = tempdata.CompletedSpareParts;
                model.DeniedReason = tempdata.DeniedReason;
                model.Amount = subcats.LeadCredit.Value.ToString();
                if(cs != null)
                {
                    model.CustomerName = cs.CustomerName;
                    model.CustomerPhoneNumber = cs.MobileNo;
                    model.Area = cs.Pincodedata;
                }
                if(partner != null)
                {
                    model.PartnerProfName = partner.PartnerName;
                }
                
                model.AppointmentDate = tempdata.GeneratedDateTime.HasValue ? tempdata.GeneratedDateTime.Value.ToString("dd-MMM-yyyy hh:mm") : "";
                model.OrderId = tempdata.LeadDetailsId;
                model.Status = "Denied";
                
                data.Add(model);

            }


            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OngoingLeads()
        {
            return View();
        }
        public ActionResult CompletedLeads()
        {
            return View();
        }

        public ActionResult DeniedLeads()
        {
            return View();
        }




        public void PushNotification(string message)
        {
            
            var applicationID = "AIza---------4GcVJj4dI";

            var senderId = "72464340137";

            string deviceId = "euxqdp------ioIdL87abVL";

            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");

            tRequest.Method = "post";

            tRequest.ContentType = "application/json";

            var data = new

            {

                to = deviceId,

                notification = new

                {

                    body = message,

                    //title = obj.TagMsg,

                    icon = "Water solutions"

                }
            };

            var serializer = new JavaScriptSerializer();

            var json = serializer.Serialize(data);

            Byte[] byteArray = Encoding.UTF8.GetBytes(json);

            tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));

            tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));

            tRequest.ContentLength = byteArray.Length;


            using (Stream dataStream = tRequest.GetRequestStream())
            {

                dataStream.Write(byteArray, 0, byteArray.Length);


                using (WebResponse tResponse = tRequest.GetResponse())
                {

                    using (Stream dataStreamResponse = tResponse.GetResponseStream())
                    {

                        using (StreamReader tReader = new StreamReader(dataStreamResponse))
                        {

                            String sResponseFromServer = tReader.ReadToEnd();

                            string str = sResponseFromServer;

                        }
                    }
                }
            }
        }

        





        //private void SendNotification(string RegisterID, string Message)
        //{
        //    try
        //    {
        //        var SenderID = "";//ProjectID
        //        var GoogleAppID = "";

        //        //Configuration
        //        var Config = new GcmConfiguration(SenderID, GoogleAppID, null);

        //        //Create a new broker
        //        var gcmBroker = new GcmServiceBroker(Config);

        //        gcmBroker.OnNotificationFailed += GcmBroker_OnNotificationFailed;
        //        gcmBroker.OnNotificationSucceeded += GcmBroker_OnNotificationSucceeded;

        //        gcmBroker.Start();

        //        gcmBroker.QueueNotification(new GcmNotification
        //        {
        //            RegistrationIds = new List<string>
        //            {
        //                RegisterID
        //            },
        //            Data = JObject.Parse(("{ \"aps\":{\"badge\":1, \"sound\":\"oven.caf\",\"alert\":\"" + (Message + "\"}}")))
        //        });

        //        gcmBroker.Stop();
        //    }
        //    catch(Exception ex)
        //    {

        //    }
        //}

        //private void GcmBroker_OnNotificationSucceeded(GcmNotification notification)
        //{
        //    throw new NotImplementedException();
        //}

        //private void GcmBroker_OnNotificationFailed(GcmNotification notification, AggregateException exception)
        //{
        //    throw new NotImplementedException();
        //}

        public JsonResult ViewNewLeadsJson()
        {
            List<LeadGenerationViewModel> leadGenerations = dbOps.GetLeadsViewModelWS("New");
            return Json(new { data = leadGenerations }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ViewLeads()
        {
            
            return View();
        }

        
    }
}