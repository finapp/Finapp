using Finapp.Interfaces;
using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class CreditorService : ICreditor
    {
        FinapEntities context;

        public CreditorService(FinapEntities _context)
        {
            context = _context;
        }

        public IEnumerable<Creditor> getAvailableCreditors()
        {
            try
            {
                return context.Creditor.Where(c => c.Available == true).OrderBy(c => c.Queue_Date).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}