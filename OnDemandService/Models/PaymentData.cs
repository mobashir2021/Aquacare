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
    
    public partial class PaymentData
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PaymentData()
        {
            this.RechargeOrders = new HashSet<RechargeOrder>();
        }
    
        public int PaymentDataId { get; set; }
        public string Dataname { get; set; }
        public string Datatype { get; set; }
        public Nullable<int> PaymentTypeId { get; set; }
    
        public virtual PaymentType PaymentType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RechargeOrder> RechargeOrders { get; set; }
    }
}
