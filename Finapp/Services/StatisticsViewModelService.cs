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
        private readonly IDebtorViewModelService _debtorViewModelService;
        private readonly IAssociateViewModelService _associateViewModelService;
        private readonly IAssociateService _associateService;
        private readonly ISummaryService _summaryService;

        public StatisticsViewModelService(FinapEntities1 context, ITransactionOutService transactionService, ICreditorService creditorService, 
            IDebtorService debtorService, IDebtorViewModelService debtorViewModelService, 
            IAssociateViewModelService associateViewModelService, IAssociateService associateService, ISummaryService summaryService)
        {
            _context = context;
            _transactionService = transactionService;
            _creditorService = creditorService;
            _debtorService = debtorService;
            _debtorViewModelService = debtorViewModelService;
            _associateViewModelService = associateViewModelService;
            _associateService = associateService;
            _summaryService = summaryService;
        }

       
        public SummaryModel CreateLastSummary()
        {
            var summaries = _summaryService.GetAllSummaries();

            var debetAverage = 0;
            var balanceAverage = 0;
            var savingsAverage = 0;
            var profitsAverage = 0;
            var savingsAveragePercentage = 0;
            var profitsAveragePercentage = 0;
            var profitsSum = 0;
            var savingsSum = 0;
            var balanceSum = 0;
            var debetSum = 0;
            var days = 0;
            var turnover = 0;

            int counter = 0;

            foreach (var summary in summaries)
            {
                if (summary.Days > 0)
                {
                    counter++;
                    debetAverage += summary.Debet_Average ?? 0;
                    balanceAverage += summary.Balance_Average ?? 0;
                    savingsAverage += summary.Savings_Average ?? 0;
                    profitsAverage += summary.Profits_Average ?? 0;
                    savingsAveragePercentage += summary.Savings_Average_Percentage ?? 0;
                    profitsAveragePercentage += summary.Profits_Average_Percentage ?? 0;
                    profitsSum += summary.Profits_Sum ?? 0;
                    savingsSum += summary.Savings_Sum ?? 0;
                    balanceSum += summary.Balance_Sum ?? 0;
                    debetSum += summary.Debet_Sum ?? 0;
                    days += summary.Days ?? 0;
                    turnover += summary.Turnover ?? 0;
                }

            }

            if (counter > 0)
            {
                debetAverage /= counter;
                balanceAverage /= counter;
                savingsAverage /= counter;
                profitsAverage /= counter;
                savingsAveragePercentage /= counter;
                profitsAveragePercentage /= counter;
                days /= counter;
            }

            return new SummaryModel
            {
                DebetAverage = debetAverage,
                BalanceAverage = balanceAverage,
                SavingsAverage = savingsAverage,
                ProfitsAverage = profitsAverage,
                SavingsAveragePercentage = savingsAveragePercentage,
                ProfitsAveragePercentage =  profitsAveragePercentage,
                ProfitsSum = profitsSum,
                SavingsSum = savingsSum,
                BalanceSum = balanceSum,
                DebetSum = debetSum,
                Days = days,
                Turnover = turnover
            };
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
                AssociateId = summary.Associate_Id,
                Turnover = summary.Turnover??0
            };
        }

        public IEnumerable<StatisticsViewModel> GetAllStatistics()
        {
            var associations = _associateService.GetAllAssociations();
            List<StatisticsViewModel> statistics = new List<StatisticsViewModel>();

            foreach (var associate in associations)
            {
                var statistic = _summaryService.GetSummaryByAssociate(associate);

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
