using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.ViewModels
{
    public class Summary
    {
        public int DebetAverage { get; set; }
        public int BalanceAverage { get; set; }
        public int SavingsAverage { get; set; }
        public int ProfitsAverage { get; set; }
        public int SavingsAveragePercentage { get; set; }
        public int ProfitsAveragePercentage { get; set; }
        public int ProfitsSum { get; set; }
        public int SavingsSum { get; set; }
        public int BalanceSum { get; set; }
        public int DebetSum { get; set; }
    }
}