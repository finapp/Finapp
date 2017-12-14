using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.ViewModels
{
    public class TestsViewModel
    {
        public string GetDebtorsTime { get; set; }
        public string GetCreditorsTime { get; set; }
        public string GetAssociationsTime { get; set; }
        public string GetDbDebtorsTime { get; set; }
        public string GetDbCreditorsTime { get; set; }
        public string GetDbTransactionsTime { get; set; }
        public int CountOfCreditors { get; set; }
        public int CountOfDebtors { get; set; }
        public int CountOfTransactions { get; set; }
        public string TimeForOneTransaction { get; set; }
    }
}