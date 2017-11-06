using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.ViewModels
{
    public class CreditorTransactionViewModel
    {

        public Creditor Creditor { get; set; }

        public IEnumerable<Transaction_Out> Transactions { get; set; }
    }
}