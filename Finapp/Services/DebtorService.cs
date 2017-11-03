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
        private readonly FinapEntities _context;

        public DebtorService(FinapEntities context)
        {
            _context = context;
        }

        public IEnumerable<Debtor> GetAllDebtors()
        {
            try
            {
                return _context.Debtor.ToList(); ;
            }
            catch(Exception e)
            {
                return null;
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
                return null;
            }
        }

        public IEnumerable<Debtor> GetDebtorsWithDebet()
        {
            try
            {
                return _context.Debtor.Where(d => d.Finapp_Debet > 0).ToList();
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public IEnumerable<Debtor> GetAvailableDebtors()
        {
            try
            {
                return _context.Debtor.Where(d => d.Available == true)
                    .ToList();
            }
            catch (Exception e)
            {
                return null;
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
                return false;
            }
        }

    }
}