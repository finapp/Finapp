using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finapp.Interfaces
{
    public interface IDebtorAccount
    {

        Debtor_Account getDebtorAccountByDebtorId(int id);
    }
}
