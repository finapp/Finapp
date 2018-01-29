using AutoMapper;
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
        private Func<FinapEntities1> dbFactory;
        private FinapEntities1 _context;

        public CreditorService(Func<FinapEntities1> dbFactory, FinapEntities1 context)
        {
            this.dbFactory = dbFactory;
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
            _context = new FinapEntities1();
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
            var dbContext = dbFactory.Invoke();
            var counter = 1;
            _context.Dispose();
            try
            {
                foreach (var item in creditors)
                {
                    counter++;
                    dbContext.Entry(item).State = EntityState.Modified;
                    // _context.Entry(item).State = EntityState.Modified;
                    if (counter % 100 == 0)
                    {
                        dbContext.SaveChanges();
                        dbContext.Dispose();
                        dbContext = new FinapEntities1();
                    }
                }
                dbContext.SaveChanges();
                _context = new FinapEntities1();
            }
            catch(Exception e)
            {
                return false;
            }
            finally
            {
                dbContext.Dispose();
            }

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
            IEnumerable<Creditor> updateCreditors = Mapper.Map<IEnumerable<Creditor>>(creditors);
            int counter = 0;
            try
            {
                foreach (var item in updateCreditors)
                {
                    _context.Entry(item).State = EntityState.Added;
                    counter++;
                    if (counter % 100 == 0)
                    {
                        _context.SaveChanges();
                        _context.Dispose();
                        _context = new FinapEntities1();
                    }
                }
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                _context.Dispose();
            }

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

            return date ?? DateTime.Now;
        }

    }
}