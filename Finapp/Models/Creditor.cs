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
            this.Creditor_Account = new HashSet<Creditor_Account>();
            this.Associate = new HashSet<Associate>();
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
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Creditor_Account> Creditor_Account { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Associate> Associate { get; set; }
    }
}
