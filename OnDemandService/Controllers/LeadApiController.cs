using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using OnDemandService.Models;
using System.Data.Entity;

namespace OnDemandService.Controllers
{
    [RoutePrefix("api/LeadApi")]
    public class LeadApiController : ApiController
    {
        HomeServicesEntities db = new HomeServicesEntities();
        DatabaseOperations dbOps = new DatabaseOperations();
        string ApiUrl = "http://www.fjfgroups.com/Upload/";

        [HttpGet]
        public HttpResponseMessage LeadsWithType(string Status, int Partnerid)
        {
            List<LeadGenerationViewModel> lst = dbOps.GetLeadsViewModel(Status, Partnerid);
            try
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(lst));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;
            }
            catch(Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            
            
        }

        [HttpGet]
        public HttpResponseMessage GetCategories()
        {
            List<string> products = new List<string>();
            products.Add("Filter by Category");
            products.AddRange( db.ProductCategories.ToList().Select(x => x.ProductCategory1).ToList());
            
            try
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(products));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetProducts()
        {
            var products = db.Products.Include(y => y.ProductCategory).ToList();
            List<ProductModel> productModels = new List<ProductModel>();
            foreach(var data in products)
            {
                ProductModel p = new ProductModel();
                p.ProductId = data.ProductId; p.ProductName = data.ProductName; p.ProductImage = data.ProductImage; p.Price = data.Price.Value;
                p.ProductCategory = data.ProductCategory.ProductCategory1;
                productModels.Add(p);
            }
            try
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(productModels));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetCities()
        {
            
            try
            {
                List<string> lst = db.Cities.ToList().Select(x => x.CityName).ToList();
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(lst));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }



        }

        [HttpGet]
        public HttpResponseMessage LeadsWithTypePartner(string Status, int Partnerid)
        {
            List<LeadGenerationViewModel> lst = dbOps.GetLeadsViewModelPartner(Status, Partnerid);
            try
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(lst));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }



        }

        [HttpGet]
        public HttpResponseMessage Updatetoken(string token, int partnerid)
        {
            try
            {
                string returnvalue = "truedata";
                DeviceToken device = db.DeviceTokens.Where(x => x.Token.Trim() == token.Trim()).FirstOrDefault();
                if(device != null)
                {
                    device.PartnerId = partnerid;
                    db.Entry(device).State = EntityState.Modified;
                    db.SaveChanges();
                }
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(returnvalue));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;

            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public HttpResponseMessage UserLogin(string username, string password)
        {
            try
            {
                UserData user = dbOps.GetLoginData(username, password);
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(user));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;

            }
            catch(Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public HttpResponseMessage AcceptOrder(int LeadDetailid, int Partnerid)
        {
            try
            {
                dbOps.AcceptOrder(LeadDetailid, Partnerid);
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject("Truedata"));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;

            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

        }

        [HttpGet]
        public HttpResponseMessage RechargeData(int Amount, int Partnerid, string Paymentstatus)
        {
            try
            {
                dbOps.RechargeData(Amount, Partnerid, Paymentstatus);
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject("Truedata"));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;

            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

        }

        [HttpGet]
        public HttpResponseMessage UseRecharge(int Partnerid)
        {
            try
            {
                List<RechargeViewmodel> lst = dbOps.GetRecharge(Partnerid);
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(lst));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;

            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

        }

        [HttpGet]
        public HttpResponseMessage SaveUsername(int Partnerid, string Username)
        {
            try
            {
                if(Partnerid > 0 && !string.IsNullOrEmpty(Username))
                {
                    int userid = db.PartnerProfessionals.Where(x => x.PartnerProfessionalId == Partnerid).First().UserId.Value;
                    User user = db.Users.Where(x => x.UserId == userid).First();
                    user.UserName = Username;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                }
                string data = "truedata";
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(data));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;

            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

        }

        [HttpGet]
        public HttpResponseMessage GetBalanceautorefresh(int Professionalid)
        {
            try
            {
                int balance = 0;
                if(Professionalid > 0)
                {
                    balance = dbOps.GetBalanceRecharge(Professionalid);
                }
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(balance.ToString()));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;

            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetMaxRechargeOrder()
        {
            try
            {
                int maxvalue = 0;
                if (db.RechargeOrders.ToList().Count > 0)
                    maxvalue = db.RechargeOrders.Max(x => x.RechargeOrderId) + 1;
                else
                    maxvalue = 1;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(maxvalue.ToString()));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;

            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

        }

        [HttpGet]
        public HttpResponseMessage CompleteOrder(int LeadDetailid, int Partnerid, string Spareparts, string Totalcost)
        {
            try
            {
                dbOps.CompleteOrder(LeadDetailid, Partnerid, Spareparts, Totalcost);
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject("Truedata"));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;

            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

        }

        [HttpPost]
        public HttpResponseMessage SaveToken()
        {
            try
            {
                var request = HttpContext.Current.Request;
                var token = request.Form["Token"];
                var partnerid = request.Form["Partnerid"];
                DeviceToken deviceToken = new DeviceToken();
                deviceToken.Token = token;
                deviceToken.PartnerId = Convert.ToInt32(partnerid);
                db.DeviceTokens.Add(deviceToken);
                db.SaveChanges();
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Myerror : " + ex.Message.ToString() + "---" + ex.InnerException.Message.ToString())
                };
            }
        }

        

        
        [HttpPost]
        public HttpResponseMessage RegisterUser()
        {
            try
            {
                
                var request = HttpContext.Current.Request;
                
                var PartnerName = request.Form["PartnerName"];
                var MobileNo = request.Form["MobileNo"];
                

                HttpPostedFile CompanyImage = null;
                
                HttpPostedFile PancardImage = null;
                HttpPostedFile AadharImage = null;
                for (int i = 0; i < request.Files.Count; i++)
                {
                    var myFile = request.Files[i];
                    if (myFile == null)
                        continue;
                    if (i == 0)
                    {
                        CompanyImage = request.Files[i];
                    }
                    else if (i == 1)
                    {
                        PancardImage = request.Files[i];
                    }
                    else if (i == 2)
                    {
                        AadharImage = request.Files[i];
                    }
                    //HttpPostedFile httpPostedFile = request.Files[i];
                    //if (httpPostedFile != null)
                    //{
                    //    // Construct file save path  
                    //    //WebClient web = new WebClient();
                    //    //web.UploadFile(ApiUrl, httpPostedFile.FileName);
                    //    var filePath = HttpContext.Current.Server.MapPath("~/Upload/" + httpPostedFile.FileName);
                    //    httpPostedFile.SaveAs(filePath);
                        
                    //}
                }
                string extCompanyImage = null;
                string extPanImage = null;
                string extAadharImage = null;
                if (CompanyImage != null)
                {
                    extCompanyImage = Path.GetExtension(CompanyImage.FileName);
                    CompanyImage.SaveAs(HttpContext.Current.Server.MapPath("~/Upload/" + "CompanyImage" + MobileNo + extCompanyImage));
                }
                if(PancardImage != null)
                {
                    extPanImage = Path.GetExtension(PancardImage.FileName);
                    PancardImage.SaveAs(HttpContext.Current.Server.MapPath("~/Upload/" + "PancardImage" + MobileNo + extPanImage));
                }
                if(AadharImage != null)
                {
                    extAadharImage = Path.GetExtension(AadharImage.FileName);
                    AadharImage.SaveAs(HttpContext.Current.Server.MapPath("~/Upload/" + "AadharImage" + MobileNo + extAadharImage));
                }
                
                //var PanNo = request.Form["PancardNo"];
                var GSTNo = request.Form["GSTNo"];
                var CompanyName = request.Form["CompanyName"];
                //var AadharNo = request.Form["AadharNo"];
                var Pincodedata = request.Form["Pincodedata"];
                var Address = request.Form["Address"];
                var SubCategoryId = request.Form["SubCategoryId"];
                var Cityid = request.Form["Cityid"];
                City city = db.Cities.ToList().Where(x => x.CityName == Cityid).FirstOrDefault();

                PartnerProfessional partner = new PartnerProfessional();
                partner.PartnerName = PartnerName; 
                //partner.AadharNo = AadharNo; 
                partner.Address = Address;
                partner.Cityid = city != null ? city.CityId : 0;
                partner.CompanyName = CompanyName; partner.GSTNo = GSTNo; partner.MobileNo = MobileNo; 
                //partner.PancardNo = PanNo;
                partner.Pincodedata = Pincodedata; partner.SubCategoryId = Convert.ToInt32(SubCategoryId);
                if(extCompanyImage != null)
                    partner.CompanyImage = ApiUrl + "CompanyImage" + MobileNo + extCompanyImage;
                if(extPanImage != null)
                    partner.PancardImage = ApiUrl + "PancardImage" + MobileNo +  extPanImage;
                if(extAadharImage != null)
                    partner.AadharImage = ApiUrl + "AadharImage" + MobileNo +  extAadharImage;
                

                db.PartnerProfessionals.Add(partner);
                db.SaveChanges();





                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Myerror : " + ex.Message.ToString() + "---" + ex.InnerException.Message.ToString())
                };
            }
        }

        [HttpGet]
        public HttpResponseMessage DeniedOrder(int LeadDetailid, int Partnerid, string DenialReason)
        {
            try
            {
                dbOps.DeniedOrder(LeadDetailid, Partnerid, DenialReason);
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject("Truedata"));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;

            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

        }

        public LeadDetail Get(int id)
        {
            LeadDetail lst = db.LeadDetails.ToList().Where(x => x.LeadDetailsId == id).FirstOrDefault();
            return lst;
        }

        //public void Post([FromBody] LeadGenerationViewModel lead)
        //{
        //    if (lead != null && lead.Leadgenerationid == 0)
        //    {
        //        Customer customer = new Customer();

        //        customer.CustomerName = lead.Customeraddress;
        //        customer.Cityid = lead.Cityid;
        //        customer.Pincodedata = lead.Pincode.ToString();
        //        customer.MobileNo = lead.Customermobileno;
        //        customer.Address = lead.Customeraddress;
        //        db.Customers.Add(customer);
        //        db.SaveChanges();

        //        LeadDetail leadDetail = new LeadDetail();
        //        leadDetail.GeneratedDateTime = DateTime.Now;
        //        leadDetail.LeadStatus = "New";
        //        leadDetail.CustomerId = customer.CustomerId;
        //        leadDetail.OrderDateTime = lead.Orderdatetime;
        //        leadDetail.SubCategoryId = lead.Serviceprovided;

        //        db.LeadDetails.Add(leadDetail);
        //        db.SaveChanges();
        //    }
        //}

        //public void Put([FromBody] LeadGenerationViewModel lead)
        //{
        //    if (lead != null && lead.Leadgenerationid > 0)
        //    {
        //        if (lead.CustomerId > 0)
        //        {
        //            Customer customer = db.Customers.Where(x => x.CustomerId == lead.CustomerId).FirstOrDefault();

        //            customer.CustomerName = lead.Customeraddress;
        //            customer.Cityid = lead.Cityid;
        //            customer.Pincodedata = lead.Pincode.ToString();
        //            customer.MobileNo = lead.Customermobileno;
        //            customer.Address = lead.Customeraddress;
        //            db.SaveChanges();
        //        }
        //        LeadDetail leadDetail = db.LeadDetails.Where(x => x.LeadDetailsId == lead.Leadgenerationid).FirstOrDefault();
        //        if (leadDetail != null)
        //        {
        //            leadDetail.SubCategoryId = lead.Serviceprovided;
        //            leadDetail.OrderDateTime = lead.Orderdatetime;
        //            leadDetail.LeadStatus = lead.LeadStatus;
        //            db.SaveChanges();
        //        }


        //    }
        //}

        public void Delete(int id)
        {
            LeadDetail lead = db.LeadDetails.Where(x => x.LeadDetailsId == id).FirstOrDefault();
            db.LeadDetails.Remove(lead);
            db.SaveChanges();
        }
    }
}
