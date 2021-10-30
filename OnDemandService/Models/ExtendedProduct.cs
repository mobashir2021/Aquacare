using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnDemandService.Models
{
    public class ExtendedProduct : Product
    {
        public HttpPostedFileBase ImageFile { get; set; }
    }
}