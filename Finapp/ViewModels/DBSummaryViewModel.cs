using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.ViewModels
{
    public class DBSummaryViewModel
    {
        public int AmountOfCreditors { get; set; }
        public int AmountOfDebtors { get; set; }
        public int AmoountOfTransactions { get; set; }
        public int AmountOfAssociations { get; set; }
    }
}