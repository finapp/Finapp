using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class CreditorService
    {
        private readonly FinapEntities _context;

        public CreditorService(FinapEntities context)
        {
            _context = context;
        }

        public IEnumerable<Creditor> GetAllCreditors()
        {
            try
            {
                return _context.Creditor.ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Creditor GetCreditorById(int id)
        {
            try
            {
                return _context.Creditor.Where(d => d.Creditor_Id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public IEnumerable<Creditor> GetDebtorsWithDebet()
        {
            try
            {
                return _context.Creditor.Where(d => d.Finapp_Balance > 0).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}