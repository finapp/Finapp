using Finapp.IServices;
using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class CreditorService : ICreditorService
    {
        private readonly FinapEntities1 _context;

        public CreditorService(FinapEntities1 context)
        {
            _context = context;
        }

        public IEnumerable<Creditor> GetAllCreditors()
        {
            try
            {
                return _context.Creditor
                    .OrderBy(c => c.Queue_Date)
                    .ToList();
            }
            catch (Exception e)
            {
                throw e;
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
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<Creditor> GetCreditorsWithBalance()
        {
            try
            {
                return _context.Creditor
                    .Where(c => c.Finapp_Balance > 0)
                    .ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<Creditor> GetAvailableCreditors(float eapr)
        {
            try
            {
                var creditors = _context.Creditor
                    .Where(c => c.Available == true && eapr > c.EROI)//change from eapr > c.Delta_ROI
                    .OrderBy(c => c.Queue_Date)
                    .ToList();

                return creditors;
            }
            catch (Exception e)
            {
                throw e;
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
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool ModifyCreditors(IEnumerable<Creditor> creditors)
        {

            foreach (var item in creditors)
            {
                _context.Entry(item).State = EntityState.Modified;
            }
            _context.SaveChanges();

            return true;
        }

        public string GetCreditorUsernameById(int id)
        {
            try
            {
                return (from c in _context.Creditor
                        where c.Creditor_Id == id
                        select c.username)
                        .FirstOrDefault();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool AddNewCreditor(Creditor creditor)
        {
            try
            {
                _context.Entry(creditor).State = EntityState.Added;
                _context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool AddNewCreditors(IEnumerable<Creditor> creditors)
        {

            foreach (var item in creditors)
            {
                _context.Entry(item).State = EntityState.Added;
            }
                _context.SaveChanges();

                return true;
        }

        public bool AddAssociate(Associate associate, Creditor creditor)
        {
            creditor.Associate.Add(associate);
            _context.Entry(creditor).State = EntityState.Modified;
            _context.SaveChanges();

            return true;
        }

        public IEnumerable<Creditor> GetCreditorsFromAssociate(Associate associate)
        {
            return associate.Creditor;
        }

        public DateTime GetTheOldestQueueDate()
        {
            var dates = (from c in _context.Creditor
                        select c.Queue_Date)
                        .ToList();
            var date = dates.Max();

            return date??DateTime.Now;
        }

    }
}