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
        private readonly FinapEntities _context;

        public CreditorService(FinapEntities context)
        {
            _context = context;
        }

        public IEnumerable<Creditor> GetAvailableCreditors()
        {
            try
            {
                return _context.Creditor.Where(c => c.Available == true)
                    .OrderBy(c => c.Queue_Date)
                    .ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}