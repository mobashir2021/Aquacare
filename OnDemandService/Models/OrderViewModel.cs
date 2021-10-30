using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OnDemandService.Models
{
    public class OrderViewModel
    {
        
        [Key]
        public int OrderId { get; set; }
        public int SNo { get; set; }
        public string Service { get; set; }
        
        public string CustomerName { get; set; }
        
        public string AppointmentDate { get; set; }

        public string Status { get; set; }
        public string Amount { get; set; }
        public string Area { get; set; }

        public string PlacedOn { get; set; }

        public string StatusValue { get; set; }
        public string PartnerProfName { get; set; }

        public string Orderwithpartner { get; set; }

        public string PartnerphoneNumber { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string PartnerEmail { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }

        public int OrderRequestId { get; set; }

        

    }

    public class OrderVMData : OrderViewModel
    {
        public string TotalCost { get; set; }
        public string SpareParts { get; set; }
        public string DeniedReason { get; set; }
    }
}