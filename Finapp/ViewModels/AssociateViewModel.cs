using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.ViewModels
{
    public class AssociateViewModel
    {
        public AssociateViewModel()
        {
            List = new List<TransactionWithUserViewModel>();
        }

        public DateTime Date { get; set; }
        public int AssociateId { get; set; }
        public List<TransactionWithUserViewModel> List { get; set; }
    }
}