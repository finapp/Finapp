using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.ViewModels
{
    public class DebtorToSummaryViewModel
    {
        public string Username { get; set; }
        public int ExpSavings { get; set; }
        public int DayAccessToFunds { get; set; }
        public int TransactionCounter { get; set; }
        public int AssociateId { get; set; }
    }
}