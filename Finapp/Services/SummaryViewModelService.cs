﻿using Finapp.IServices;
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
        private readonly FinapEntities1 _context;
        private readonly ITransactionOutService _transactionService;
        private readonly ICreditorService _creditorService;
        private readonly IDebtorService _debtorService;
        private readonly IAssociateViewModelService _associateService;
        private readonly IStatisticsViewModelService _statisticsService;

        public SummaryViewModelService(FinapEntities1 context, ITransactionOutService transactionService, 
            ICreditorService creditorService, IDebtorService debtorService, IAssociateViewModelService associateService, 
            IStatisticsViewModelService statisticsService)
        {
            _context = context;
            _transactionService = transactionService;
            _creditorService = creditorService;
            _debtorService = debtorService;
            _associateService = associateService;
            _statisticsService = statisticsService;
        }

        public IEnumerable<AssociateViewModel> GetTransactions()
        {
            return _associateService.GetAllTransactions();
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