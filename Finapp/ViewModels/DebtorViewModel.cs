using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.ViewModels
{
    public class DebtorViewModel
    {
        public string Username { get; set; }
        public float? APR { get; set; }
        public float? EAPR { get; set; }
        public int Debet { get; set; }
        public int FinalDebet { get; set; }
    }
}