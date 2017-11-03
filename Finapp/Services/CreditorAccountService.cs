using Finapp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Finapp.Models;

namespace Finapp.Services
{
    public class CreditorAccountService : ICreditorAccount
    {
        private readonly FinapEntities _context;

        public CreditorAccountService(FinapEntities context)
        {
            _context = context;
        }

        public Creditor_Account getCreditorAccountByCreditorId(int id)
        {
            try
            {
                return _context.Creditor_Account.Where(c => c.Creditor_Id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}