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
    
    public partial class CustomerComplaint
    {
        public int CustomerComplaintId { get; set; }
        public Nullable<int> ComplaintId { get; set; }
        public string MiscellanousData { get; set; }
        public string Reasons { get; set; }
        public Nullable<System.DateTime> ComplaintDateTime { get; set; }
        public int CustomerId { get; set; }
    
        public virtual Complaint Complaint { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
