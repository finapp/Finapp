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
        private readonly IAssociateViewModelService _associateViewModelService;
        private readonly IStatisticsViewModelService _statisticsService;

        public SummaryViewModelService(IAssociateViewModelService associateViewModelService, 
            IStatisticsViewModelService statisticsService)
        {
            _associateViewModelService = associateViewModelService;
            _statisticsService = statisticsService;
        }

        public IEnumerable<AssociateViewModel> GetTransactions()
        {
            return _associateViewModelService.GetAllTransactions();
        }

        public IEnumerable<StatisticsViewModel> GetSummary()
        {
            return _statisticsService.GetAllStatistics();
        }

        public SummaryModel GetLastSummary()
        {
            return _statisticsService.CreateLastSummary();
        }

        public SummaryViewModel GetAllInformations()
        {
            return new SummaryViewModel
            {
                ListOfTransactions = GetTransactions(),
                Summary = GetSummary(),
                LastSummary = GetLastSummary()
            };
        }

    }
}