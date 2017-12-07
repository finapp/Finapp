using Finapp.IServices;
using Finapp.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class DebtorService : IDebtorService
    {
        private readonly FinapEntities1 _context;

        public DebtorService(FinapEntities1 context)
        {
            _context = context;
        }

        public IEnumerable<Debtor> GetAllDebtors()
        {
            try
            {
                return _context.Debtor
                    .OrderBy(d => d.Queue_Date)
                    .ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Debtor GetDebtorById(int id)
        {
            try
            {
                return _context.Debtor
                    .Where(d => d.Debtor_Id == id)
                    .FirstOrDefault();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Debtor GetDebtorByUsername(string username)
        {
            try
            {
                return _context.Debtor
                    .Where(d => d.username == username)
                    .FirstOrDefault();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<Debtor> GetDebtorsWithDebet()
        {
            try
            {
                return _context.Debtor
                    .Where(d => d.Finapp_Debet > 0)
                    .ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<Debtor> GetAvailableDebtors()
        {
            try
            {
                var debtors = _context.Debtor
                    .Where(d => d.Available == true)
                    .OrderBy(d => d.Queue_Date)
                    .ToList();

                return debtors; 
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool ModifyDebtor(Debtor debtor)
        {
            try
            {
                _context.Entry(debtor).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string GetDebtorUsernameById(int id)
        {
            try
            {
                return _context.Debtor
                    .Where(d => d.Debtor_Id == id)
                    .FirstOrDefault()
                    .username;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool AddNewDebtor(Debtor debtor)
        {
            try
            {
                _context.Entry(debtor).State = EntityState.Added;
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Debtor> GetDebtorsWithoutTransactions()
        {
            var debtorAccounts = _context.Debtor_Account.ToList();
            var listOfDebtorWithoutTransactions = new List<Debtor>();

            foreach (var account in debtorAccounts)
            {
                var accountWithTransaction = _context.Transaction_Out
                    .Any(t => t.Debtor_Account_Id == account.Debtor_Account_Id);

                if (!accountWithTransaction)
                {
                    var debtor = _context.Debtor
                        .Where(d => d.Debtor_Id == account.Debtor_Id)
                        .FirstOrDefault();

                    listOfDebtorWithoutTransactions.Add(debtor);
                }
            }

            return listOfDebtorWithoutTransactions;
        }

        public bool AddAssociate(Associate associate, Debtor debtor)
        {
            debtor.Associate.Add(associate);
            _context.Debtor.Attach(debtor);
            _context.Entry(debtor).State = EntityState.Modified;
            _context.SaveChanges();

            return true;
        }

        public IEnumerable<Debtor> GetDebtorsFromAssociate(Associate associate)
        {
            return associate.Debtor;
        }
        public DateTime GetTheOldestQueueDate()
        {
            var debtors = _context.Debtor.ToList();
            var date = debtors.Max(c => c.Queue_Date);

            return date ?? DateTime.Now;
        }

    }
}