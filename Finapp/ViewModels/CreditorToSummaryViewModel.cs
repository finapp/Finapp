using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.ViewModels
{
    public class CreditorToSummaryViewModel
    {
        public string Username { get; set; }
        public int EAPR { get; set; }
        public int DayAccessToFunds { get; set; }
        public int TransactionCounter { get; set; }
        public int AssociateId { get; set; }
    }
}