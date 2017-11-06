using Finapp.Interfaces;
using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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


        public IEnumerable<Creditor> GetAvailableCreditors(Debtor debtor)
        {
            try
            {
                return _context.Creditor.Where(c => c.Available == true && c.EROI < debtor.EAPR)
                    .OrderBy(c => c.Queue_Date)
                    .ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ModifyCreditor(Creditor creditor)
        {
            try
            {
                _context.Entry(creditor).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Creditor> GetAllCreditors()
        {
            try
            {
                return _context.Creditor
                    .ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Creditor GetCreditorById(int id)
        {
            try
            {
                return _context.Creditor
                    .Where(c => c.Creditor_Id == id)
                    .FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}