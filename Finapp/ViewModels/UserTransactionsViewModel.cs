using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.ViewModels
{
    public class UserTransactionsViewModel
    {
        public int Amount { get; set; }
        public int AssociateNr { get; set; }
        public int Profits { get; set; }
        public string Username { get; set; }
        public int Transactions { get; set; }
    }
}