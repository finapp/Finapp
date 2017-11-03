using Finapp.Interfaces;
using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class DebtorService : IDebtor
    {
        private readonly FinapEntities _context;

        public DebtorService(FinapEntities context)
        {
            _context = context;
        }

        public bool ModifyDebtor(Debtor debtor)
        {
            try
            {
                _context.Entry(debtor).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Debtor GetAvaialbleDebtor()
        {
            try
            {
                return _context.Debtor.Where(d => d.Available == true)
                    .OrderBy(d => d.Queue_Date)
                    .FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Debtor> GetAllDeptors()
        {
            try
            {
                return _context.Debtor
                    .OrderBy(d => d.Queue_Date)
                    .ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}