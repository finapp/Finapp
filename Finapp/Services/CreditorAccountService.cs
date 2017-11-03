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
        private readonly FinapEntities _context;

        public CreditorAccountService(FinapEntities context)
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
    }
}
