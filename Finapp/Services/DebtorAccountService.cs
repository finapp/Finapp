using Finapp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Finapp.Models;

namespace Finapp.Services
{
    public class DebtorAccountService : IDebtorAccount
    {
        private readonly FinapEntities _context;

        public DebtorAccountService(FinapEntities context)
        {
            _context = context;
        }

        public Debtor_Account getDebtorAccountByDebtorId(int id)
        {
            try
            {
                return _context.Debtor_Account.Where(d => d.Debtor_Id == id).FirstOrDefault();
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}