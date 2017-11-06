using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.ViewModels
{
    public class TransactionOutViewModel
    {
        public int Amount { get; set; }
        public DateTime? Date { get; set; }
        public string CreditorUsername { get; set; }
        public string DebtorUsername { get; set; }
    }
}