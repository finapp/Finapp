//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Finapp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Creditor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Creditor()
        {
            this.Associate = new HashSet<Associate>();
            this.Transaction_Out1 = new HashSet<Transaction_Out>();
        }
    
        public int Creditor_Id { get; set; }
        public string username { get; set; }
        public Nullable<double> ROI { get; set; }
        public Nullable<double> EROI { get; set; }
        public int Balance { get; set; }
        public bool Available { get; set; }
        public int Finapp_Balance { get; set; }
        public Nullable<System.DateTime> Queue_Date { get; set; }
        public Nullable<System.DateTime> Expiration_Date { get; set; }
        public Nullable<float> Delta_ROI { get; set; }
        public Nullable<int> Trials { get; set; }
        public Nullable<int> AssociateCounter { get; set; }
        public Nullable<int> LastAssociate { get; set; }
        public Nullable<int> ActualCreditorBenefits { get; set; }
        public Nullable<int> Profits { get; set; }
        public Nullable<int> ActualMoney { get; set; }
        public Nullable<int> AccessDays { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Associate> Associate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transaction_Out> Transaction_Out1 { get; set; }
    }
}
