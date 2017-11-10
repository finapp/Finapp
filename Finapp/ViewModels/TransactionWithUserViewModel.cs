using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.ViewModels
{
    public class TransactionWithUserViewModel
    {
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public string CreditorUsername { get; set; }
        public string DebtorUsername { get; set; }
        public float ROI { get; set; }
        public int DebtorAccountFinappAmount { get; set; }
        public int CreditorAccountFinappAmount { get; set; }
        public int CreditorBenefits { get; set; }
        public int DebtorBenefits { get; set; }
        public float RealCreditorBenefits { get; set; }
        public float RealDebtorBenefits { get; set; }
        public int DayAccessToFunds { get; set; }
        public float APR { get; set; }
    }
}