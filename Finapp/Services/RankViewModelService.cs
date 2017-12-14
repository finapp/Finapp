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
            var creditors = _creditorService.GetAllCreditors();

            List<RankViewModel> listOfCreditors = new List<RankViewModel>();

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
            var debtors = _debtorService.GetAllDebtors();

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
                    Money = debtor.Savings??0
                });
            }

            listOfDebtors = listOfDebtors.OrderBy(c => -c.AssociateCounter).ToList();

            return listOfDebtors;
        }
    }
}