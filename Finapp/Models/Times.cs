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
    
    public partial class Times
    {
        public int Id { get; set; }
        public string GetCreditorsTime { get; set; }
        public string GetDebtorsTime { get; set; }
        public string AssociateTime { get; set; }
        public string UpdateDebtorsTime { get; set; }
        public string UpdateCreditorsTime { get; set; }
        public string UpdateTransactionsTime { get; set; }
        public Nullable<int> CountOfCreditors { get; set; }
        public Nullable<int> CountOfDebtors { get; set; }
        public Nullable<int> CountOfTransactions { get; set; }
        public string TimeForOneTransaction { get; set; }
        public Nullable<int> AllDebtors { get; set; }
        public Nullable<int> AllCreditors { get; set; }
        public string SetROI { get; set; }
    }
}
