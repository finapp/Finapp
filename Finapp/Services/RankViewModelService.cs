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

            var associations = _associateService.GetAllAssociations();

            var counter = 0;
            List<RankViewModel> listOfCreditors = new List<RankViewModel>();

            foreach (var creditor in creditors)
            {
                counter = 0;
                foreach (var associate in associations)
                {
                    var transactions = _context.Transaction_Out
                        .Where(t => t.Associate_Id == associate.Associate_Id && t.Creditor_Id == creditor.Creditor_Id)
                        .ToList()
                        .Count();

                    if (transactions > 0)
                        counter++;
                }
                listOfCreditors.Add(new RankViewModel
                {
                    Username = creditor.username,
                    Delta = (int)creditor.Delta_ROI,
                    AssociateCounter = counter
                });
            }

            listOfCreditors = listOfCreditors.OrderBy(c => -c.AssociateCounter).ToList();

            return listOfCreditors;
        }

        public IEnumerable<RankViewModel> GetDebtorsRank()
        {
            var debtors = _debtorService.GetAllDebtors();

            var associations = _associateService.GetAllAssociations();

            var counter = 0;
            List<RankViewModel> listOfDebtors = new List<RankViewModel>();

            foreach (var debtor in debtors)
            {
                counter = 0;
                foreach (var associate in associations)
                {
                    var transactions = _context.Transaction_Out
                        .Where(t => t.Associate_Id == associate.Associate_Id && t.Debtor_Id == debtor.Debtor_Id)
                        .ToList()
                        .Count();

                    if (transactions > 0)
                        counter++;
                }
                listOfDebtors.Add(new RankViewModel
                {
                    Username = debtor.username,
                    Delta = (int)debtor.Delta_APR,
                    AssociateCounter = counter
                });
            }

            listOfDebtors = listOfDebtors.OrderBy(c => -c.AssociateCounter).ToList();

            return listOfDebtors;
        }
    }
}