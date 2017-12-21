using Finapp.IServices;
using Finapp.Models;
using Finapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class RankViewModelService : IRankViewModelService
    {
        private readonly FinapEntities1 _context;
        private readonly IDebtorService _debtorService;
        private readonly ICreditorService _creditorService;
        private readonly IAssociateService _associateService;

        public RankViewModelService(FinapEntities1 context, IDebtorService debtorService, ICreditorService creditorService, 
            IAssociateService associateService)
        {
            _context = context;
            _debtorService = debtorService;
            _creditorService = creditorService;
            _associateService = associateService;
        }

        public IEnumerable<RankViewModel> GetCreditorsRank()
        {
            var creditors = (from c in _context.Creditor
                             select new { c.username, c.AssociateCounter, c.ROI, c.EROI, c.Trials, c.Profits })
                             .ToList();

            List < RankViewModel > listOfCreditors = new List<RankViewModel>();

            foreach (var creditor in creditors)
            {
                listOfCreditors.Add(new RankViewModel
                {
                    Username = creditor.username,
                    AssociateCounter = creditor.AssociateCounter??0,
                    ROI = creditor.ROI??0,
                    EROI = creditor.EROI??0,
                    Trials = creditor.Trials??0,
                    Money = creditor.Profits??0
                });
            }

            listOfCreditors = listOfCreditors.OrderBy(c => -c.AssociateCounter).ToList();

            return listOfCreditors;
        }

        public IEnumerable<RankViewModel> GetDebtorsRank()
        {
            var debtors = (from d in _context.Debtor
                           select new { d.username, d.AssociateCounter, d.APR, d.EAPR, d.Trials, d.Savings, d.HaveMoney })
                           .ToList();

            var days = (from a in _context.Associate
                        select a.Nr)
                        .ToList().Max();

            List<RankViewModel> listOfDebtors = new List<RankViewModel>();

            foreach (var debtor in debtors)
            {
                listOfDebtors.Add(new RankViewModel
                {
                    Username = debtor.username,
                    AssociateCounter = debtor.AssociateCounter??0,
                    APR = debtor.APR??0,
                    EAPR = debtor.EAPR??0,
                    Trials = debtor.Trials??0,
                    Money = debtor.Savings??0,
                    Days = days??0,
                    DaysWithMoney = debtor.HaveMoney??0
                });
            }

            listOfDebtors = listOfDebtors.OrderBy(c => -c.AssociateCounter).ToList();

            return listOfDebtors;
        }
    }
}