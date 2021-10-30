using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OnDemandService.Models;

namespace OnDemandService.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        HomeServicesEntities db = new HomeServicesEntities();
        // GET: Professional/User
        [HttpGet]
        public ActionResult Registration()
        {
            ViewBag.Pincodes = new SelectList(db.Pincodes, "PincodeId", "Pincode1");
            
            return View();
        }

        [HttpGet]
        public ActionResult RegistrationProfessional()
        {
            ViewBag.Pincodes = new SelectList(db.Pincodes, "PincodeId", "Pincode1");
            ViewBag.Service = new SelectList(db.SubCategories, "SubCategoryId", "SubCategoryName");
            return View();
        }
        [HttpPost]
        public ActionResult RegistrationProfessional(UserViewModel userViewModel, string Pincodes, string Service)
        {
            string Message = "";
            bool isValid = true;
            if (userViewModel != null)
            {
                User user = db.Users.Where(x => x.Email == userViewModel.Email).FirstOrDefault();
                if (user != null)
                {
                    isValid = false;
                    Message = "Email Already exists";
                    ViewBag.Message = Message;
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
                    role.Roles = "Professional";
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

                    PartnerProfessional partner = new PartnerProfessional();
                    partner.Address = userViewModel.Address;
                    partner.PartnerName = userViewModel.Name;
                    partner.MobileNo = userViewModel.MobileNo;
                    partner.UserId = user1.UserId;
                    partner.Pincodedata = pincode.Pincode1;
                    partner.PincodeId = pincode.PincodeId;
                    var subcat = db.SubCategories.ToList().Where(x => x.SubCategoryId == Convert.ToInt32(Service)).First();
                    partner.SubCategory = subcat;

                    db.PartnerProfessionals.Add(partner);
                    db.SaveChanges();
                    ViewBag.Message = "Registration Success";
                    ViewBag.Status = true;

                }
            }


            ViewBag.Pincodes = new SelectList(db.Pincodes, "PincodeId", "Pincode1");
            return View(userViewModel);
        }

        [HttpPost]
        public ActionResult Registration(UserViewModel userViewModel, string Pincodes)
        {
            string Message = "";
            bool isValid = true;
            if(userViewModel != null)
            {
                User user = db.Users.Where(x => x.Email == userViewModel.Email).FirstOrDefault();
                if(user != null)
                {
                    isValid = false;
                    Message = "Email Already exists";
                    ViewBag.Message = Message;
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
                    role.Roles = "Professional";
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

                    PartnerProfessional partner = new PartnerProfessional();
                    partner.Address = userViewModel.Address;
                    partner.PartnerName = userViewModel.Name;
                    partner.MobileNo = userViewModel.MobileNo;
                    partner.UserId = user1.UserId;
                    partner.Pincodedata = pincode.Pincode1;
                    partner.PincodeId = pincode.PincodeId;

                    db.PartnerProfessionals.Add(partner);
                    db.SaveChanges();
                    ViewBag.Message = "Registration Success";
                    ViewBag.Status = true;

                }
            }
            
             
            ViewBag.Pincodes = new SelectList(db.Pincodes, "PincodeId", "Pincode1");
            return View(userViewModel);
        }

        public ActionResult LoginProfessional()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginProfessional(string username, string password)
        {
            User user = db.Users.ToList().Where(x => x.UserName.ToLower() == username.ToLower() && x.Password == password).FirstOrDefault();
            string roles = "";
            if (user != null)
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == user.UserId).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Admin")
                {
                    FormsAuthentication.SetAuthCookie(user.UserId.ToString(), true);
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return View("LoginProfessional");
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            User user = db.Users.ToList().Where(x => x.UserName.ToLower() == username.ToLower() && x.Password == password).FirstOrDefault();
            string roles = "";
            if(user != null)
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == user.UserId).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if(roles == "Professional")
                {
                    FormsAuthentication.SetAuthCookie(user.UserId.ToString(), false);
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }
    }
}