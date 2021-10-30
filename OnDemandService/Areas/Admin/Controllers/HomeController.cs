using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnDemandService.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.OngoingApp = 0;
            ViewBag.CompletedApp = 0;
            ViewBag.CancelledApp = 0;
            ViewBag.NewApp = 0;
            ViewBag.TotalCustomers = 0;
            ViewBag.TotalProfessionals = 0;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}