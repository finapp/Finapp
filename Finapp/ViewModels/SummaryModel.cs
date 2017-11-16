using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.ViewModels
{
    public class SummaryModel
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
        public int CounterOdCreditors { get; set; }
        public int CounterOfDebtors { get; set; }
        public int Days { get; set; }
        public DateTime DateOfSummary { get; set; }
        public int AssociateId { get; set; }
    }
}