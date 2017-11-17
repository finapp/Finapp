using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finapp.IServices
{
    public interface ICreditorRankService
    {
        bool AddAssociateToCreditor(Creditor_Rank creditorRank);
    }
}
