﻿using Finapp.IServices;
using Finapp.Models;
using Finapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class TransactionOutService : ITransactionOutService
    {
        private readonly FinapEntities1 _context;
        private readonly ICreditorService _creditorService;

        public TransactionOutService(FinapEntities1 context, ICreditorService creditorService)
        {
            _context = context;
            _creditorService = creditorService;
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

        public IEnumerable<TransactionWithUserViewModel> GetTransactionsWithDebtorByDebtorId(int id)
        {
            var debtor = _context.Debtor.Where(d => d.Debtor_Id == id)
                .Join(_context.Debtor_Account,
                d => d.Debtor_Id,
                da => da.Debtor_Id,
                (d, da) => new { Debtor = d, Debtor_Account = da })
                .FirstOrDefault();

            if (debtor == null)
                return null;


            var accountId = debtor.Debtor_Account.Debtor_Account_Id;
            var transactions = _context.Debtor_Account
                .Where(da => da.Debtor_Id == accountId)
                .Join(_context.Transaction_Out,
                da => da.Debtor_Account_Id,
                t => t.Debtor_Account_Id,
                (da, t) => new { Debtor_Account = da, Transaction_Out = t }
                ).ToList();

            if (transactions == null)
                return null;

            List<TransactionWithUserViewModel> listOfDebtorTransactions = new List<TransactionWithUserViewModel>();

            foreach (var transaction in transactions)
            {
                var creditorAccount = _context.Transaction_Out.Where(t => t.Transaction_Out_Id == transaction.Transaction_Out.Transaction_Out_Id)
                    .Join(_context.Creditor_Account,
                    t => t.Creditor_Account_Id,
                    ca => ca.Creditor_Account_Id,
                    (t, ca) => new { Transaction_Out = t, Creditor_Account = ca }).FirstOrDefault();

                var creditor = _creditorService.GetCreditorById(creditorAccount.Creditor_Account.Creditor_Id);
                var creditorBenefits = (transaction.Transaction_Out.Ammount * (float)(transaction.Transaction_Out.ROI/100));
                var debtorBenefits = (transaction.Transaction_Out.Ammount * (float)((debtor.Debtor.APR - debtor.Debtor.EAPR) / 100));

                listOfDebtorTransactions.Add(new TransactionWithUserViewModel
                {
                    Amount = transaction.Transaction_Out.Ammount,
                    DebtorAccountFinappAmount = transaction.Transaction_Out.Finapp_Debetor??0,
                    DebtorUsername = debtor.Debtor.username,
                    Date = transaction.Transaction_Out.Date_Of_Transaction,
                    ROI = (float)transaction.Transaction_Out.ROI,
                    CreditorUsername = creditor.username,
                    CreditorAccountFinappAmount = transaction.Transaction_Out.Finapp_Creditor??100,
                    CreditorBenefits = (int)creditorBenefits,
                    DebtorBenefits = (int)debtorBenefits
                });
            }

            return listOfDebtorTransactions;
        }
    }
}