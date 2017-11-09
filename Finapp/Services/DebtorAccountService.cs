using Finapp.IServices;
using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class DebtorAccountService : IDebtorAccountService
    {
        private readonly FinapEntities1 _context;

        public DebtorAccountService(FinapEntities1 context)
        {
            _context = context;
        }

        public bool AddDebtorAccount(Debtor_Account debtorAccount)
        {
            try
            {
                _context.Entry(debtorAccount).State = EntityState.Added;
                _context.SaveChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
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
                throw e;
            }
        }

        public int GetAccountIdByDebtorUsername(string username)
        {
            try
            {
                var debtor = _context.Debtor.Where(d => d.username == username).FirstOrDefault();
                var account = _context.Debtor_Account.Where(a => a.Debtor_Id == debtor.Debtor_Id).FirstOrDefault();
                return account.Debtor_Account_Id;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int GetDebtorIdByAccountId(int id)
        {
            try
            {
                return _context.Debtor_Account.Where(t => t.Debtor_Account_Id == id).FirstOrDefault().Debtor_Id;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}