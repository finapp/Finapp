using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.IServices
{
    public interface IDebtorAccountService
    {
        Debtor_Account GetAccountByDebtorId(int id);
        int GetAccountIdByDebtorId(int id);
        int GetAccountIdByDebtorUsername(string username);
        int GetDebtorIdByAccountId(int id);
    }
}