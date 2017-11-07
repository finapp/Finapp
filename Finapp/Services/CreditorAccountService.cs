using Finapp.IServices;
using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finapp.Services
{
    public class CreditorAccountService : ICreditorAccountService
    {
        private readonly FinapEntities1 _context;

        public CreditorAccountService(FinapEntities1 context)
        {
            _context = context;
        }

        public Creditor_Account GetAccountByCreditorId(int id)
        {
            try
            {
                return _context.Creditor_Account.Where(a => a.Creditor_Id == id)
                    .FirstOrDefault();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public int GetAccountIdByCreditorId(int id)
        {
            try
            {
                var account = _context.Creditor_Account.Where(a => a.Creditor_Id == id).FirstOrDefault();
                return account.Creditor_Account_Id;
            }
            catch(Exception e)
            {
                return -1;
            }
        }

        public int GetAccountIdByCreditorUsername(string username)
        {
            try
            {
                var creditor = _context.Creditor.Where(c => c.username == username).FirstOrDefault();
                var account = _context.Creditor_Account.Where(a => a.Creditor_Id == creditor.Creditor_Id).FirstOrDefault();
                return account.Creditor_Account_Id;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public int GetCreditorIdByAccountId(int id)
        {
            try
            {
                return _context.Creditor_Account.Where(t => t.Creditor_Account_Id == id).FirstOrDefault().Creditor_Id;
            }
            catch (Exception e)
            {
                return -1;
            }
        }
    }
}
