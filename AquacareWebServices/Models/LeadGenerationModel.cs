﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AquacareWebServices.Models
{
    public class LeadGenerationModel
    {
        [Key]
        public int Leadgenerationid { get; set; }

        public int CustomerId { get; set; }
        public string Customername { get; set; }

        public string Customermobileno { get; set; }

        public string Customeraddress { get; set; }

        public string Orderdesc { get; set; }

        public int Serviceprovided { get; set; }

        public DateTime Orderdatetime { get; set; }

        public int Cityid { get; set; }

        public int Pincode { get; set; }

        public string LeadStatus { get; set; }
    }
}