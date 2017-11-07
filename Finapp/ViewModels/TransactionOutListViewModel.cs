using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.ViewModels
{
    public class TransactionOutListViewModel
    {
        public TransactionOutListViewModel()
        {
            List = new List<TransactionOutViewModel>();
        }

        public List<TransactionOutViewModel> List { get; set; }
    }
}