using Finapp.IServices;
using Finapp.Models;
using Finapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class SummaryViewModelService : ISummaryViewModelService
    {
        private readonly ITransactionOutService _transactionService;
        private readonly ICreditorService _creditorService;
        private readonly IDebtorService _debtorService;
        private readonly IAssociateViewModelService _associateViewModelService;
        private readonly IStatisticsViewModelService _statisticsService;
        private readonly ICreditorViewModelService _creditorViewModelService;
        private readonly IAssociateService _associateService;
        private readonly IDebtorToSummaryViewModelService _debtorSummaryService;
        private readonly ICreditorToSummaryViewModelService _creditorSummaryService;

        public SummaryViewModelService(ITransactionOutService transactionService, ICreditorService creditorService, IDebtorService debtorService, 
            IAssociateViewModelService associateViewModelService, IStatisticsViewModelService statisticsService, ICreditorViewModelService creditorViewModelService, 
            IAssociateService associateService, IDebtorToSummaryViewModelService debtorSummaryService, ICreditorToSummaryViewModelService creditorSummaryService)
        {
            _transactionService = transactionService;
            _creditorService = creditorService;
            _debtorService = debtorService;
            _associateViewModelService = associateViewModelService;
            _statisticsService = statisticsService;
            _creditorViewModelService = creditorViewModelService;
            _associateService = associateService;
            _debtorSummaryService = debtorSummaryService;
            _creditorSummaryService = creditorSummaryService;
        }

        public IEnumerable<AssociateViewModel> GetTransactions()
        {
            return _associateViewModelService.GetAllTransactions();
        }

        public IEnumerable<StatisticsViewModel> GetSummary()
        {
            return _statisticsService.GetAllStatistics();
        }

        public SummaryViewModel GetAllInformations()
        {
            return new SummaryViewModel
            {
                ListOfTransactions = GetTransactions(),
                Summary = GetSummary(),
                Creditors = GetCreditors(),
                Debtors = GetDebtors()
            };
        }

        public IEnumerable<IEnumerable<CreditorToSummaryViewModel>> GetCreditors()
        {
            var associations = _associateService.GetAllAssociations();
            List<List<CreditorToSummaryViewModel>> creditorsViewModel = new List<List<CreditorToSummaryViewModel>>();
            List<CreditorToSummaryViewModel> creditorsSummaryListFromOneAssociate;

            foreach (var associate in associations)
            {
                var transactions = _transactionService.GetTransactionsFromAssociate(associate);
                var creditors = _creditorService.GetCreditorsFromAssociate(associate);
                creditorsSummaryListFromOneAssociate = new List<CreditorToSummaryViewModel>();

                foreach (var creditor in creditors)
                {
                    var transactionsCounter = transactions
                        .Where(t => t.Creditor_Id == creditor.Creditor_Id)
                        .Count();

                    creditorsSummaryListFromOneAssociate.Add(_creditorSummaryService.CreateViewModel(creditor, transactionsCounter,associate.Associate_Id));
                }
                creditorsSummaryListFromOneAssociate = creditorsSummaryListFromOneAssociate
                    .OrderBy(t => -t.TransactionCounter)
                    .ToList();

                creditorsViewModel.Add(creditorsSummaryListFromOneAssociate);    
            }

            return creditorsViewModel;
        }

        public IEnumerable<IEnumerable<DebtorToSummaryViewModel>> GetDebtors()
        {
            var associations = _associateService.GetAllAssociations();
            List<List<DebtorToSummaryViewModel>> debtorsViewModel = new List<List<DebtorToSummaryViewModel>>();
            List<DebtorToSummaryViewModel> debtorsSummaryListFromOneAssociate;

            foreach (var associate in associations)
            {
                var transactions = _transactionService.GetTransactionsFromAssociate(associate);
                var debtors = _debtorService.GetDebtorsFromAssociate(associate);
                debtorsSummaryListFromOneAssociate = new List<DebtorToSummaryViewModel>();

                foreach (var debtor in debtors)
                {
                    var transactionsCounter = transactions.Where(t => t.Debtor_Id == debtor.Debtor_Id).Count();

                    debtorsSummaryListFromOneAssociate.Add(_debtorSummaryService.CreateViewModel(debtor, transactionsCounter, associate.Associate_Id));
                }
                debtorsSummaryListFromOneAssociate = debtorsSummaryListFromOneAssociate.OrderBy(t => -t.TransactionCounter).ToList();
                debtorsViewModel.Add(debtorsSummaryListFromOneAssociate);
            }

            return debtorsViewModel;
        }
    }
}