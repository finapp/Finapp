﻿using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finapp.IServices
{
    public interface ICreditorAccountService
    {
        Creditor_Account GetAccountByCreditorId(int id);
        int GetAccountIdByCreditorId(int id);
        int GetAccountIdByCreditorUsername(string username);
        int GetCreditorIdByAccountId(int id);
        bool AddCreditorAccount(Creditor_Account creditor_Account);
    }
}
