using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.ViewModels
{
    public class TransactionOutListViewModel
    {
        public IEnumerable<TransactionOutViewModel> List { get; set; }
    }
}