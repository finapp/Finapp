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
                return _context.Creditor
                    .Where(c => c.Available == true && eapr > c.Delta_ROI)
                    .OrderBy(c => c.Queue_Date)
                    .ToList();
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

        public string GetCreditorUsernameById(int id)
        {
            try
            {
                return _context.Creditor
                    .Where(c => c.Creditor_Id == id)
                    .FirstOrDefault().username;
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

        public IEnumerable<Creditor> GetCreditorsWithoutTransactions()
        {
            var creditorAccounts = _context.Creditor_Account.ToList();
            var listOfCreditorWithoutTransactions = new List<Creditor>();

            foreach (var account in creditorAccounts)
            {
                var accountWithTransaction = _context.Transaction_Out
                    .Any(t => t.Creditor_Account_Id == account.Creditor_Account_Id);

                if (!accountWithTransaction)
                {
                    var creditor = _context.Creditor
                        .Where(c => c.Creditor_Id == account.Creditor_Id)
                        .FirstOrDefault();

                    listOfCreditorWithoutTransactions.Add(creditor);
                }
            }

            return listOfCreditorWithoutTransactions;
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

    }
}