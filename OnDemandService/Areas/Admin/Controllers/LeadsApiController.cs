using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OnDemandService.Models;

namespace OnDemandService.Areas.Admin.Controllers
{
    public class LeadsApiController : ApiController
    {
        HomeServicesEntities db = new HomeServicesEntities();
        DatabaseOperations dbOps = new DatabaseOperations();
        public IEnumerable<LeadDetail> Get()
        {
            List<LeadDetail> lst = db.LeadDetails.ToList().Where(x => x.LeadStatus == "New").ToList();
            return lst;
        }

        public IEnumerable<LeadDetail> Get(string Status)
        {
            List<LeadDetail> lst = db.LeadDetails.ToList().Where(x => x.LeadStatus == Status).ToList();
            return lst;
        }

        public List<LeadGenerationViewModel> GetLeadsWithType(string Status, int Partnerid)
        {
            List<LeadGenerationViewModel> lst = dbOps.GetLeadsViewModel(Status, Partnerid);
            return lst;
        }

        

        public LeadDetail Get(int id)
        {
            LeadDetail lst = db.LeadDetails.ToList().Where(x => x.LeadDetailsId == id).FirstOrDefault();
            return lst;
        }

        public void Post([FromBody] LeadGenerationViewModel lead)
        {
            if (lead != null && lead.Leadgenerationid == 0)
            {
                Customer customer = new Customer();

                customer.CustomerName = lead.Customeraddress;
                customer.Cityid = lead.Cityid;
                customer.Pincodedata = lead.Pincode.ToString();
                customer.MobileNo = lead.Customermobileno;
                customer.Address = lead.Customeraddress;
                db.Customers.Add(customer);
                db.SaveChanges();

                LeadDetail leadDetail = new LeadDetail();
                leadDetail.GeneratedDateTime = DateTime.Now;
                leadDetail.LeadStatus = "New";
                leadDetail.CustomerId = customer.CustomerId;
                leadDetail.OrderDateTime = lead.Orderdatetime;
                leadDetail.SubCategoryId = lead.Serviceprovided;

                db.LeadDetails.Add(leadDetail);
                db.SaveChanges();
            }
        }

        public void Put([FromBody] LeadGenerationViewModel lead)
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

        public void Delete(int id)
        {
            LeadDetail lead = db.LeadDetails.Where(x => x.LeadDetailsId == id).FirstOrDefault();
            db.LeadDetails.Remove(lead);
            db.SaveChanges();
        }
    }
}
