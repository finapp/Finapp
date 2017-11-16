using Finapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.IServices
{
    public interface ISummaryViewModelService
    {
        IEnumerable<AssociateViewModel> GetTransactions();
        IEnumerable<StatisticsViewModel> GetSummary();
        SummaryViewModel GetAllInformations();
    }
}