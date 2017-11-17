using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finapp.IServices
{
    public interface IDebtorRankService
    {
        bool AddAssociateToDebtor(Debtor_Rank debtorRank);
        IEnumerable<Debtor_Rank> GetAllRanks();
    }
}
