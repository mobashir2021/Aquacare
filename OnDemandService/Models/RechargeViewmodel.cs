using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnDemandService.Models
{
    public class RechargeViewmodel
    {
        public int RechargeOrderId { get; set; }
        public int Rechargeamount { get; set; }
        
        public string Paymentstatus { get; set; }
        public string Paymentdate { get; set; }

        
    }

    public class RechargeViewModelData : RechargeViewmodel
    {
        public string Partnername { get; set; }
    }
}