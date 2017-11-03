using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.ViewModels
{
    public class CreditorListViewModel
    {
        public CreditorListViewModel()
        {
            List = new List<CreditorViewModel>();
        }

        public List<CreditorViewModel> List { get; set; }
    }
}