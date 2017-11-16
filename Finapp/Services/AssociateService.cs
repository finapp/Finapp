using Finapp.IServices;
using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class AssociateService : IAssociateService
    {
        private readonly FinapEntities1 _context;

        public AssociateService(FinapEntities1 context)
        {
            _context = context;
        }

        public bool AddNewAssociate(Associate associate)
        {
            try
            {
                _context.Associate.Add(associate);
                _context.SaveChanges();

                return true;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public bool AddCreditor(Associate associate, Creditor creditor)
        {
            associate.Creditor.Add(creditor);
            _context.Associate.Attach(associate);
            _context.Entry(associate).State = EntityState.Modified;
            _context.SaveChanges();
            return true;
        }

        public bool AddDebtor(Associate associate, Debtor debtor)
        {
            associate.Debtor.Add(debtor);
            _context.Associate.Attach(associate);
            _context.Entry(associate).State = EntityState.Modified;
            _context.SaveChanges();

            return true;
        }

        public IEnumerable<Associate> GetAllAssociations()
        {
            try
            {
                return _context.Associate.ToList();
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}