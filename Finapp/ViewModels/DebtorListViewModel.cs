using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.ViewModels
{
    public class DebtorListViewModel
    {
        public DebtorListViewModel()
        {
            List = new List<DebtorViewModel>();
        }

        public List<DebtorViewModel> List { get; set; }
    }
}