using Finapp.Interfaces;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using Finapp.Models;

namespace Finapp.Services
{
    public class TransactionOutService : ITransactionOut
    {
        private readonly FinapEntities _context;

        public TransactionOutService(FinapEntities context)
        {
            _context = context;
        }

        public bool CreateTransaction(Transaction_Out newTransaction)
        {

            try
            {
                _context.Transaction_Out.Add(newTransaction);
                _context.SaveChanges();

                return true;

            }
            catch(Exception e)
            {
                throw;
            }
        }
    }
}