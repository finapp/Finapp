using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.ViewModels
{
    public class PeopleWithoutAssociateViewModel
    {
        public IEnumerable<DebtorViewModel> DebtorListWithoutAssociate { get; set; }
        public IEnumerable<CreditorViewModel> CreditorListWithoutAssociate { get; set; }
        public int DebetAverage { get; set; }
        public int BalanceAverage { get; set; }
        public int SavingsAverage { get; set; }
        public int ProfitsAverage { get; set; }
    }
}