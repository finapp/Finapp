using Finapp.IServices;
using Finapp.Models;
using Finapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class StatisticsViewModelService : IStatisticsViewModelService
    {

        private readonly FinapEntities1 _context;
        private readonly ITransactionOutService _transactionService;
        private readonly ICreditorService _creditorService;
        private readonly IDebtorService _debtorService;
        private readonly IDebtorAccountService _debtorAccountService;
        private readonly IDebtorViewModelService _debtorViewModelService;
        private readonly IAssociateViewModelService _associateService;

        public StatisticsViewModelService(FinapEntities1 context, ITransactionOutService transactionService,
            ICreditorService creditorService, IDebtorService debtorService, IDebtorAccountService debtorAccountService,
            IDebtorViewModelService debtorViewModelService, IAssociateViewModelService associateService)
        {
            _context = context;
            _transactionService = transactionService;
            _creditorService = creditorService;
            _debtorService = debtorService;
            _debtorAccountService = debtorAccountService;
            _debtorViewModelService = debtorViewModelService;
            _associateService = associateService;
        }

        public IEnumerable<StatisticsViewModel> GetSummary()
        {
            var associations = _associateService.GetAllTransactions();
            var debtors = new List<Debtor>();
            var listOfDebtorAccounts = _debtorAccountService.GetAllAccounts();

            List<StatisticsViewModel> returnedList = new List<StatisticsViewModel>();

            foreach (var associate in associations)
            {
                debtors.Clear();

                var listOfTransactions = associate.List;
                foreach (var debtorAccount in listOfDebtorAccounts)
                {
                    var debtorHaveTransaction = _context.Transaction_Out
                    .Where(t => t.Date_Of_Transaction == associate.Date)
                    .Any(tr => tr.Debtor_Account_Id == debtorAccount.Debtor_Account_Id);

                    if (!debtorHaveTransaction)
                    {
                        var debtor = _debtorAccountService.GetDebtorByAccountId(debtorAccount.Debtor_Account_Id);

                        if (debtor.Finapp_Debet == debtor.Debet)
                            debtors.Add(debtor);
                    }
                }

                StatisticsViewModel model = new StatisticsViewModel();
                var debtorsViewModel = _debtorViewModelService.CreateListViewModel(debtors);
                model.DebtorListWithoutAssociate = debtorsViewModel;

                returnedList.Add(model);
            }

            return returnedList;
        }

        public SummaryModel CreateStatisticViewModel(Summary summary)
        {
            if (summary == null)
                return null;

            return new SummaryModel
            {
                DebetAverage = summary.Debet_Average ?? 0,
                BalanceAverage = summary.Balance_Average ?? 0,
                SavingsAverage = summary.Savings_Average ?? 0,
                ProfitsAverage = summary.Profits_Average ?? 0,
                SavingsAveragePercentage = summary.Savings_Average_Percentage ?? 0,
                ProfitsAveragePercentage = summary.Profits_Average_Percentage ?? 0,
                ProfitsSum = summary.Profits_Sum ?? 0,
                SavingsSum = summary.Savings_Sum ?? 0,
                BalanceSum = summary.Balance_Sum ?? 0,
                DebetSum = summary.Debet_Sum ?? 0,
                Days = summary.Days??0,
                DateOfSummary = summary.DateOfSummary??DateTime.Now,
                AssociateId = summary.Associate_Id
            };
        }

        public IEnumerable<StatisticsViewModel> GetAllStatistics()
        {
            var associations = _context.Associate.ToList();
            List<StatisticsViewModel> statistics = new List<StatisticsViewModel>();

            foreach (var associate in associations)
            {
                var statistic = _context.Summary
                    .Where(s => s.Associate_Id == associate.Associate_Id)
                    .FirstOrDefault();

                var summary = CreateStatisticViewModel(statistic);
                statistics.Add(new StatisticsViewModel
                {
                    Summary = summary
                });

            }

            return statistics;
        }
    }
}
