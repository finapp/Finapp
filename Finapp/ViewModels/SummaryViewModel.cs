﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.ViewModels
{
    public class SummaryViewModel
    {
        public IEnumerable<AssociateViewModel> ListOfTransactions { get; set; }
        public IEnumerable<StatisticsViewModel> Summary { get; set; }
        public IEnumerable<IEnumerable<CreditorToSummaryViewModel>> Creditors { get; set; }
        public IEnumerable<IEnumerable<DebtorToSummaryViewModel>> Debtors { get; set; }
    }
}