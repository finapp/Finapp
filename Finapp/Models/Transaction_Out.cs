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
        public int Transaction_Out_Id { get; set; }
        public int Ammount { get; set; }
        public Nullable<System.DateTime> Date_Of_Transaction { get; set; }
        public int Creditor_Account_Id { get; set; }
        public int Debtor_Account_Id { get; set; }
    
        public virtual Creditor_Account Creditor_Account { get; set; }
        public virtual Debtor_Account Debtor_Account { get; set; }
    }
}
