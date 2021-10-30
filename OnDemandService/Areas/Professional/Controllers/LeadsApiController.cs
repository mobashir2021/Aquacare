using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OnDemandService.Models;

namespace OnDemandService.Areas.Professional.Controllers
{
    public class LeadsApiController : ApiController
    {
        HomeServicesEntities db = new HomeServicesEntities();
        public IEnumerable<string> GetNewLeads()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
