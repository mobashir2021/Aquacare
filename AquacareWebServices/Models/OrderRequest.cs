//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AquacareWebServices.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderRequest
    {
        public int OrderRequestId { get; set; }
        public int OrderId { get; set; }
        public Nullable<int> Partnerid { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<System.DateTime> SelectedDate { get; set; }
        public string SelectedTime { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public Nullable<bool> IsCancelled { get; set; }
        public string Status { get; set; }
        public Nullable<int> NotifyStatus { get; set; }
        public Nullable<int> IsCustomerNotify { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual Order Order { get; set; }
        public virtual PartnerProfessional PartnerProfessional { get; set; }
    }
}
