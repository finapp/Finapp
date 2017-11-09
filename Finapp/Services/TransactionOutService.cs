﻿using Finapp.IServices;
using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class TransactionOutService : ITransactionOutService
    {
        private readonly FinapEntities1 _context;

        public TransactionOutService(FinapEntities1 context)
        {
            _context = context;
        }

        public bool AddTransaction(Transaction_Out transaction)
        {
            try
            {
                _context.Transaction_Out.Add(transaction);
                _context.SaveChanges();

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool AddTransaction(int amount, DateTime date, int creditorAccountId, int debtorAccountId)
        {
            try
            {
                _context.Transaction_Out.Add(new Transaction_Out
                {
                    Ammount = amount,
                    Date_Of_Transaction = date,
                    Creditor_Account_Id = creditorAccountId,
                    Debtor_Account_Id = debtorAccountId
                });

                _context.SaveChanges();

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public void GetTransactionsByDebtorId(int id)
        {
            var a = _context.Transaction_Out.Join(
                _context.Debtor_Account,
                t => t.Debtor_Account_Id,
                d => d.Debtor_Id,
                (t, d) => new { Transaction_Out = t, Debtor_Account = d }
                ).ToList();

            int b = 5;
        }
    }
}