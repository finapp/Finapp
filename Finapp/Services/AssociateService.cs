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
        private readonly ICreditorService _creditorService;
        private readonly IDebtorService _debtorService;

        public AssociateService(FinapEntities1 context, ICreditorService creditorService, IDebtorService debtorService)
        {
            _context = context;
            _creditorService = creditorService;
            _debtorService = debtorService;
        }

        public bool AddNewAssociate(Associate associate)
        {
            try
            {
                _context.Associate.Add(associate);
                _context.SaveChanges();

                var creditors = _creditorService.GetAvailableCreditors(50);
                var debtors = _debtorService.GetAvailableDebtors();

                foreach (var creditor in creditors)
                {
                    creditor.Trials += 1;
                    _creditorService.ModifyCreditor(creditor);
                }

                foreach (var debtor in debtors)
                {
                    debtor.Trials += 1;
                    _debtorService.ModifyDebtor(debtor);
                }

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