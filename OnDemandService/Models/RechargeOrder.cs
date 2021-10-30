//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnDemandService.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class RechargeOrder
    {
        public int RechargeOrderId { get; set; }
        public int Professionalid { get; set; }
        public Nullable<int> RechargeAmount { get; set; }
        public Nullable<System.DateTime> RechargeDate { get; set; }
        public string RechargeStatus { get; set; }
        public Nullable<int> PaymentTypeId { get; set; }
        public Nullable<int> PaymentDataId { get; set; }
    
        public virtual PaymentData PaymentData { get; set; }
        public virtual PaymentType PaymentType { get; set; }
        public virtual PartnerProfessional PartnerProfessional { get; set; }
    }
}
