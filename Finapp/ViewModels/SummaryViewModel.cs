using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.ViewModels
{
    public class SummaryViewModel
    {
        public IEnumerable<AssociateViewModel> ListOfTransactions { get; set; }
        public IEnumerable<PeopleWithoutAssociateViewModel> Summary { get; set; }
    }
}