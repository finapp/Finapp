using AutoMapper;
using Finapp.IServices;
using Finapp.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class DebtorService : IDebtorService
    {
        private FinapEntities1 _context;
        private Func<FinapEntities1> dbFactory;

        public DebtorService(FinapEntities1 context, Func<FinapEntities1> dbFactory)
        {
            _context = context;
            this.dbFactory = dbFactory;
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
            _context = new FinapEntities1();
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

        public bool ModifyDebtors(IEnumerable<Debtor> debtors)
        {
            var dbContext = dbFactory.Invoke();
            var counter = 1;
            try
            {
                foreach (var item in debtors)
                {
                    counter++;
                    dbContext.Entry(item).State = EntityState.Modified;
                    //_context.Entry(item).State = EntityState.Modified;
                    //Debtor d2 = Mapper.Map(dbContext.Debtor.Where(s=>s.Debtor_Id == item.Debtor_Id).FirstOrDefault(), item);
                    //Debtor d = Mapper.Map<Debtor>(item);
                    //dbContext.Debtor.Attach(d2);
                    //dbContext.Set<Debtor>().AddOrUpdate(item);
                    if (counter % 100 == 0)
                    {
                        dbContext.SaveChanges();
                        dbContext.Dispose();
                        dbContext = new FinapEntities1();
                    }
                }
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                dbContext.Dispose();
            }
            return true;
        }

        public string GetDebtorUsernameById(int id)
        {
            try
            {
                return (from d in _context.Debtor
                        where d.Debtor_Id == id
                        select d.username)
                        .FirstOrDefault();
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

        public bool AddNewDebtors(IEnumerable<Debtor> debtors)
        {
            IEnumerable<Debtor> updateDebtors = Mapper.Map<IEnumerable<Debtor>>(debtors);

            _context = new FinapEntities1();
            int counter = 1;
            try
            {
                foreach (var item in updateDebtors)
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
            var debtors = (from d in _context.Debtor
                           select d.Queue_Date)
                           .ToList();
                
            var date = debtors.Max();

            return date ?? DateTime.Now;
        }

    }
}