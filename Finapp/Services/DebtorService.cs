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
                    .OrderBy(d=>d.Queue_Date)
                    .ToList(); 
            }
            catch(Exception e)
            {
                throw e;
            }       
        }

        public Debtor GetDebtorById(int id)
        {
            try
            {
                return _context.Debtor.Where(d => d.Debtor_Id == id).FirstOrDefault();
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
                return _context.Debtor.Where(d => d.Finapp_Debet > 0)
                    .OrderBy(d=>d.Queue_Date)
                    .ToList();
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<Debtor> GetAvailableDebtors()
        {
            try
            {
                return _context.Debtor.Where(d => d.Available == true)
                    .OrderBy(d=>d.Queue_Date)
                    .ToList();
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
                return _context.Debtor.Where(d => d.Debtor_Id == id)
                    .FirstOrDefault().username;
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
            catch(Exception)
            {
                throw ;
            }
        }
    }
}