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
    
    public partial class IsNotificationShown
    {
        public int IsNotificationShownId { get; set; }
        public Nullable<int> OrderId { get; set; }
        public Nullable<int> ProfessionalId { get; set; }
        public Nullable<int> CustomerId { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual Order Order { get; set; }
        public virtual PartnerProfessional PartnerProfessional { get; set; }
    }
}
