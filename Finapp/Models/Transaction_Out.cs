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
    
    public partial class Transaction_Out
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Transaction_Out()
        {
            this.Return_Transaction = new HashSet<Return_Transaction>();
            this.Creditor = new HashSet<Creditor>();
            this.Debtor = new HashSet<Debtor>();
        }
    
        public int Transaction_Out_Id { get; set; }
        public int Ammount { get; set; }
        public Nullable<System.DateTime> Date_Of_Transaction { get; set; }
        public int Creditor_Account_Id { get; set; }
        public int Debtor_Account_Id { get; set; }
        public Nullable<float> ROI { get; set; }
        public Nullable<int> Finapp_Debetor { get; set; }
        public Nullable<int> Finapp_Creditor { get; set; }
        public int Day_Access_To_Funds { get; set; }
        public Nullable<int> Creditor_Benefits_Per_Annum { get; set; }
        public Nullable<int> Debtor_Benefits_Per_Annum { get; set; }
        public int Associate_Id { get; set; }
    
        public virtual Creditor_Account Creditor_Account { get; set; }
        public virtual Debtor_Account Debtor_Account { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Return_Transaction> Return_Transaction { get; set; }
        public virtual Associate Associate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Creditor> Creditor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Debtor> Debtor { get; set; }
    }
}
