using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.ViewModels
{
    public class CreditorListViewModel
    {
        public IEnumerable<CreditorViewModel> List { get; set; }
    }
}