using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.ViewModels
{
    public class StatisticsViewModel
    {
        public IEnumerable<DebtorViewModel> DebtorListWithoutAssociate { get; set; }
        public IEnumerable<CreditorViewModel> CreditorListWithoutAssociate { get; set; }
        public SummaryModel Summary { get; set; }
    }
}