using Finapp.IServices;
using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class ReturnTransactionService : IReturnTransactionService
    {
        private readonly FinapEntities1 _context;

        public ReturnTransactionService(FinapEntities1 context)
        {
            _context = context;
        }

        public bool AddReturnTransaction(Return_Transaction returnTransaction)
        {
            try
            {
                _context.Return_Transaction.Add(returnTransaction);
                _context.SaveChanges();

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public bool AddReturnTransaction(Transaction_Out transactionOut)
        {
            try
            {
                _context.Return_Transaction.Add(new Return_Transaction
                {
                    Transaction_Out_Id = transactionOut.Transaction_Out_Id,
                    Execution_Date = DateTime.Now.AddDays(30),
                    Debtor_Account_Id = transactionOut.Debtor_Account_Id,
                    Creditor_Account_Id = transactionOut.Creditor_Account_Id,
                    Return_Transaction_Status = "Pending",
                    Amount = transactionOut.Ammount
                });
                _context.SaveChanges();

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}