﻿using Finapp.IServices;
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
                    .OrderBy(c=>c.Queue_Date)
                    .ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Creditor GetCreditorById(int id)
        {
            try
            {
                return _context.Creditor.Where(c => c.Creditor_Id == id)
                    .FirstOrDefault();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public IEnumerable<Creditor> GetCreditorsWithBalance()
        {
            try
            {
                return _context.Creditor.Where(c => c.Finapp_Balance > 0)
                    .ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public IEnumerable<Creditor> GetAvailableCreditors(float eapr)
        {
            try
            {
                return _context.Creditor.Where(c => c.Available == true && eapr>c.EROI)
                    .ToList();
            }
            catch(Exception e)
            {
                return null;
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
            catch(Exception e)
            {
                return false;
            }
        }

        public string GetCreditorUsernameById(int id)
        {
            try
            {
                return _context.Creditor.Where(c => c.Creditor_Id == id)
                    .FirstOrDefault().username;
            }
            catch (Exception e)
            {
                return null;
            }
        }

    }
}