using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.ViewModels
{
    public class RankViewModel
    {
        public string Username { get; set; }
        public int AssociateCounter { get; set; }
        public int Delta { get; set; }
        public double ROI { get; set; }
        public double EROI { get; set; }
        public double APR { get; set; }
        public double EAPR { get; set; }
        public int Trials { get; set; }
        public int Money { get; set; }
        public int Days { get; set; }
        public int DaysWithMoney { get; set; }
        public int TransactionCounter { get; set; }

        public List<AssociationToRankViewModel> Associations { get; set; }
    }
}