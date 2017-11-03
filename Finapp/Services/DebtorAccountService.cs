using Finapp.IServices;
using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class DebtorAccountService : IDebtorAccountService
    {
        private readonly FinapEntities _context;

        public DebtorAccountService(FinapEntities context)
        {
            _context = context;
        }

        public Debtor_Account GetAccountByDebtorId(int id)
        {
            try
            {
                return _context.Debtor_Account.Where(a => a.Debtor_Id == id)
                    .FirstOrDefault();
            }
            catch(Exception e)
            {
                return null;
            }
        }
        public int GetAccountIdByDebtorId(int id)
        {
            try
            {
                var account = _context.Debtor_Account.Where(a => a.Debtor_Id == id).FirstOrDefault();
                return account.Debtor_Account_Id;
            }
            catch (Exception e)
            {
                return -1;
            }
        }
    }
}