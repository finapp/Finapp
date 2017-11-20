using Finapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.IServices
{
    public interface IRankViewModelService
    {
        IEnumerable<RankViewModel> GetCreditorsRank();
        IEnumerable<RankViewModel> GetDebtorsRank();
    }
}