using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace OnDemandService.Models
{
    public class DatabaseOperations
    {
        private string serverKey = "AAAAEN82FKk:APA91bFFhsELKMNfs7XnrlfmfonD8mwyxKOqU_sKPcEeOSuiKCG6OOUJKi6gjQTx4p6uvr1nVgRmQyOiiw7DQSjxAy4KkD-Pdoex1xI67LrHKroE11Hilus3HRmfs1IT0eRgvjlXvQFO";
        private string senderId = "72464340137";
        private string webAddr = "https://fcm.googleapis.com/fcm/send";
        HomeServicesEntities db = new HomeServicesEntities();

        public IEnumerable<LeadDetail> GetLeads(string Status)
        {
            List<LeadDetail> lst = db.LeadDetails.ToList().Where(x => x.LeadStatus == Status).ToList();
            return lst;
        }

        public int GetBalanceRecharge(int partnerid)
        {
            int total = 0;
            int used = 0;
            if(db.RechargeOrders.Where(x => x.Professionalid == partnerid).Count() > 0)
                total = db.RechargeOrders.Where(x => x.Professionalid == partnerid).Sum(x => x.RechargeAmount.Value);
            if(db.LeadDetails.Where(x => x.ProfessionalId == partnerid).Count() > 0)
                used = db.LeadDetails.Where(x => x.ProfessionalId == partnerid).Sum(x => x.CreditUsed.Value);
            return total - used;
        }

        public void AcceptOrder(int LeadDetailid, int Partnerid)
        {
            LeadDetail lead = db.LeadDetails.Where(x => x.LeadDetailsId == LeadDetailid).First();
            lead.ProfessionalId = Partnerid;
            lead.LeadStatus = "Ongoing";
            db.SaveChanges();
        }

        public void CompleteOrder(int LeadDetailid, int Partnerid, string Spareparts, string Totalcost)
        {
            LeadDetail lead = db.LeadDetails.Where(x => x.LeadDetailsId == LeadDetailid).First();
            lead.CompletedSpareParts = Spareparts;
            lead.TotalCost = Totalcost;
            lead.LeadStatus = "Complete";
            db.SaveChanges();
        }

        public void RechargeData(int Amount, int Partnerid, string Paymentstatus)
        {
            RechargeOrder rechargeOrder = new RechargeOrder();
            rechargeOrder.RechargeStatus = Paymentstatus;
            rechargeOrder.RechargeDate = DateTime.Now;
            rechargeOrder.RechargeAmount = Amount;
            rechargeOrder.Professionalid = Partnerid;
            db.RechargeOrders.Add(rechargeOrder);
            db.SaveChanges();
        }

        public List<RechargeViewmodel> GetRecharge(int Partnerid)
        {
            List<RechargeOrder> lst = db.RechargeOrders.Where(x => x.Professionalid == Partnerid).ToList();
            List<RechargeViewmodel> retlst = new List<RechargeViewmodel>();
            foreach(var val in lst)
            {
                RechargeViewmodel re = new RechargeViewmodel();
                re.RechargeOrderId = val.RechargeOrderId; re.Rechargeamount = val.RechargeAmount.Value; re.Paymentstatus = val.RechargeStatus;
                re.Paymentdate = val.RechargeDate.Value.ToString("dd-MMM-yyyy");
                retlst.Add(re);
            }
            return retlst;
        }

        public void DeniedOrder(int LeadDetailid, int Partnerid, string DenialReason)
        {
            LeadDetail lead = db.LeadDetails.Where(x => x.LeadDetailsId == LeadDetailid).First();
            lead.LeadStatus = "Denied";
            lead.DeniedReason = DenialReason;
            db.SaveChanges();
        }


        public UserData GetLoginData(string username, string password)
        {
            User user = db.Users.Where(x => x.UserName.ToLower() == username.ToLower() && x.Password == password).FirstOrDefault();

            Role role = db.Roles.Where(x => x.User.UserId == user.UserId).FirstOrDefault();
            PartnerProfessional partner = db.PartnerProfessionals.Where(x => x.UserId == user.UserId).First();
            SubCategory sub = db.SubCategories.Where(x => x.SubCategoryId == partner.SubCategoryId).First();
            City city = db.Cities.Where(x => x.CityId == partner.Cityid).First();
            UserData userData = new UserData();
            userData.PartnerId = partner.PartnerProfessionalId; userData.Userid = user.UserId; userData.Username = user.UserName; userData.Password = user.Password;
            userData.Pincode = partner.Pincodedata; userData.Address = partner.Address; userData.City = city.CityName; userData.Service = sub.SubCategoryName;
            userData.Balance = GetBalanceRecharge(userData.PartnerId).ToString();
            userData.Mobileno = partner.MobileNo;
            return userData;
        }

        public List<LeadGenerationViewModel> GetLeadsViewModel(string Status, int Partnerid)
        {
            List<City> cities = db.Cities.ToList();
            List<LeadDetail> lst = db.LeadDetails.ToList().Where(x => x.LeadStatus == Status).ToList();
            List<LeadGenerationViewModel> viewModels = new List<LeadGenerationViewModel>();
            PartnerProfessional professional = db.PartnerProfessionals.ToList().Where(x => x.PartnerProfessionalId == Partnerid).First();
            string cityname = cities.Where(x => x.CityId == professional.Cityid).First().CityName;

            foreach (var value in lst)
            {
                LeadGenerationViewModel lead = new LeadGenerationViewModel();
                Customer cust = db.Customers.Where(x => x.CustomerId == value.CustomerId).First();
                lead.City = cities.Where(x => x.CityId == cust.Cityid).First().CityName;
                if (cityname != lead.City)
                    continue;
                lead.Leadgenerationid = value.LeadDetailsId;
                lead.CustomerId = value.CustomerId;
                lead.Customername = cust.CustomerName;
                lead.Customermobileno = cust.MobileNo;
                lead.Customeraddress = cust.Address;
                
                lead.Orderdatetime = value.OrderDateTime.Value;
                lead.ServiceName = db.SubCategories.Where(x => x.SubCategoryId == value.SubCategoryId).First().SubCategoryName;
                lead.Serviceprovided = value.SubCategoryId;
                lead.AppointedDate = value.OrderDateTime.Value.ToString("dd-MMM-yyyy HH:mm");
                lead.Pincode = Convert.ToInt32(cust.Pincodedata);
                lead.LeadStatus = Status;
                lead.Orderdesc = value.OrderDesc;
                SubCategory sub = db.SubCategories.Where(x => x.SubCategoryId == value.SubCategoryId).First();
                lead.CreditUsed = sub.LeadCredit.Value;
                viewModels.Add(lead);
            }
            return viewModels;
        }

        public List<LeadGenerationViewModel> GetLeadsViewModelWS(string Status)
        {
            List<City> cities = db.Cities.ToList();
            List<LeadDetail> lst = db.LeadDetails.ToList().Where(x => x.LeadStatus == Status).ToList();
            List<LeadGenerationViewModel> viewModels = new List<LeadGenerationViewModel>();
            

            foreach (var value in lst)
            {
                LeadGenerationViewModel lead = new LeadGenerationViewModel();
                Customer cust = db.Customers.Where(x => x.CustomerId == value.CustomerId).First();
                lead.City = cities.Where(x => x.CityId == cust.Cityid).First().CityName;
                
                lead.Leadgenerationid = value.LeadDetailsId;
                lead.CustomerId = value.CustomerId;
                lead.Customername = cust.CustomerName;
                lead.Customermobileno = cust.MobileNo;
                lead.Customeraddress = cust.Address;

                lead.Orderdatetime = value.OrderDateTime.Value;
                lead.ServiceName = db.SubCategories.Where(x => x.SubCategoryId == value.SubCategoryId).First().SubCategoryName;
                lead.Serviceprovided = value.SubCategoryId;
                lead.AppointedDate = value.OrderDateTime.Value.ToString("dd-MMM-yyyy HH:mm");
                lead.Pincode = Convert.ToInt32(cust.Pincodedata);
                lead.LeadStatus = Status;
                lead.Orderdesc = value.OrderDesc;
                SubCategory sub = db.SubCategories.Where(x => x.SubCategoryId == value.SubCategoryId).First();
                lead.CreditUsed = sub.LeadCredit.Value;
                viewModels.Add(lead);
            }
            return viewModels;
        }

        public List<LeadGenerationViewModel> GetLeadsViewModelPartner(string Status, int Partnerid)
        {
            List<LeadDetail> lst = db.LeadDetails.ToList().Where(x => x.LeadStatus == Status && x.ProfessionalId == Partnerid).ToList();
            List<LeadGenerationViewModel> viewModels = new List<LeadGenerationViewModel>();

            foreach (var value in lst)
            {
                LeadGenerationViewModel lead = new LeadGenerationViewModel();
                Customer cust = db.Customers.Where(x => x.CustomerId == value.CustomerId).First();
                lead.Leadgenerationid = value.LeadDetailsId;
                lead.CustomerId = value.CustomerId;
                lead.Customername = cust.CustomerName;
                lead.Customermobileno = cust.MobileNo;
                lead.Customeraddress = cust.Address;
                lead.City = db.Cities.Where(x => x.CityId == cust.Cityid).First().CityName;
                lead.Orderdatetime = value.OrderDateTime.Value;
                lead.ServiceName = db.SubCategories.Where(x => x.SubCategoryId == value.SubCategoryId).First().SubCategoryName;
                lead.Serviceprovided = value.SubCategoryId;
                lead.AppointedDate = value.OrderDateTime.Value.ToString("dd-MMM-yyyy HH:mm");
                lead.Pincode = Convert.ToInt32(cust.Pincodedata);
                lead.LeadStatus = Status;
                lead.Orderdesc = value.OrderDesc;
                SubCategory sub = db.SubCategories.Where(x => x.SubCategoryId == value.SubCategoryId).First();
                lead.CreditUsed = sub.LeadCredit.Value;
                viewModels.Add(lead);
            }
            return viewModels;
        }

         

        public void AddLeads(LeadGenerationViewModel lead)
        {

            if (lead != null && lead.Leadgenerationid == 0)
            {
                List<City> cities = db.Cities.ToList();
                List<int> partners = new List<int>();

                Customer customer = new Customer();

                customer.CustomerName = lead.Customeraddress;
                customer.Cityid = lead.Cityid;
                partners = db.PartnerProfessionals.Where(x => x.Cityid == customer.Cityid).Select(x => x.PartnerProfessionalId).ToList();
                customer.Pincodedata = lead.Pincode.ToString();
                customer.MobileNo = lead.Customermobileno;
                customer.Address = lead.Customeraddress;
                db.Customers.Add(customer);
                db.SaveChanges();

                LeadDetail leadDetail = new LeadDetail();
                leadDetail.GeneratedDateTime = DateTime.Now;
                leadDetail.LeadStatus = "New";
                leadDetail.CustomerId = customer.CustomerId;
                leadDetail.OrderDateTime = DateTime.Now;
                leadDetail.SubCategoryId = lead.Serviceprovided;
                leadDetail.OrderDesc = lead.Orderdesc;
                SubCategory sub = db.SubCategories.Where(x => x.SubCategoryId == lead.Serviceprovided).First();
                leadDetail.CreditUsed = sub.LeadCredit;
                
                
                leadDetail.ProfessionalId = 1;
                db.LeadDetails.Add(leadDetail);
                db.SaveChanges();

                SendNotification("New Lead", "New Lead has been generated for " + sub.SubCategoryName + " near Pincode " + lead.Pincode.ToString(), partners);
            }
        }


        public string SendNotification(string title, string msg, List<int> partners)
        {
            List<DeviceToken> totaltokens = new List<DeviceToken>();
            var devicetokens = db.DeviceTokens;
            foreach(var dev in devicetokens)
            {
                if (dev.PartnerId.HasValue && partners.Contains(dev.PartnerId.Value))
                    totaltokens.Add(dev);
            }
            string[] tokens = totaltokens.Select(x => x.Token).ToArray();
            if (tokens.Length == 0)
                return "-1";
            var result = "-1";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
            httpWebRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
            httpWebRequest.Method = "POST";

            var payload = new
            {
                //to = DeviceToken,
                registration_ids = tokens,
                priority = "high",
                content_available = true,
                notification = new
                {
                    body = msg,
                    title = title
                },
            };
            var serializer = new JavaScriptSerializer();
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = serializer.Serialize(payload);
                streamWriter.Write(json);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }

        public void UpdateLead(LeadGenerationViewModel lead)
        {
            if (lead != null && lead.Leadgenerationid > 0)
            {
                if (lead.CustomerId > 0)
                {
                    Customer customer = db.Customers.Where(x => x.CustomerId == lead.CustomerId).FirstOrDefault();

                    customer.CustomerName = lead.Customeraddress;
                    customer.Cityid = lead.Cityid;
                    customer.Pincodedata = lead.Pincode.ToString();
                    customer.MobileNo = lead.Customermobileno;
                    customer.Address = lead.Customeraddress;
                    db.SaveChanges();
                }
                LeadDetail leadDetail = db.LeadDetails.Where(x => x.LeadDetailsId == lead.Leadgenerationid).FirstOrDefault();
                if (leadDetail != null)
                {
                    leadDetail.SubCategoryId = lead.Serviceprovided;
                    leadDetail.OrderDateTime = lead.Orderdatetime;
                    leadDetail.LeadStatus = lead.LeadStatus;
                    db.SaveChanges();
                }


            }
        }

        public void DeleteLead(int id)
        {
            LeadDetail lead = db.LeadDetails.Where(x => x.LeadDetailsId == id).FirstOrDefault();
            db.LeadDetails.Remove(lead);
            db.SaveChanges();
        }
    }
}