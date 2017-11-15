using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int FinappDebet { get; set; }
        public DateTime Expiration_Date { get; set; }
        public DateTime Queue_Date { get; set; }
        public int AccessDays { get; set; }
        public int ExpectedSavings { get; set; }
    }
}