using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.ViewModels
{
    public class CreditorViewModel
    {
        public IEnumerable<Creditor> Creditors { get; set; }
    }
}