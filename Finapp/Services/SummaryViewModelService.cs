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

        public SummaryViewModelService(ITransactionOutService transactionService, ICreditorService creditorService, IDebtorService debtorService, 
            IAssociateViewModelService associateViewModelService, IStatisticsViewModelService statisticsService, ICreditorViewModelService creditorViewModelService, 
            IAssociateService associateService)
        {
            _transactionService = transactionService;
            _creditorService = creditorService;
            _debtorService = debtorService;
            _associateViewModelService = associateViewModelService;
            _statisticsService = statisticsService;
            _creditorViewModelService = creditorViewModelService;
            _associateService = associateService;
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
                Summary = GetSummary()
            };
        }

    }
}