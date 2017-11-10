using Finapp.IServices;
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
        private readonly IDebtorService _debtorService;

        public TransactionOutService(FinapEntities1 context, ICreditorService creditorService, IDebtorService debtorService)
        {
            _context = context;
            _creditorService = creditorService;
            _debtorService = debtorService;
        }

        public bool AddTransaction(Transaction_Out transaction)
        {
            try
            {
                _context.Transaction_Out.Add(transaction);
                _context.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                throw e;
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
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<TransactionWithUserViewModel> GetTransactionsWithDebtorByDebtorUsername(string username)
        {
            var debtor = _context.Debtor.Where(d => d.username == username)
                .Join(_context.Debtor_Account,
                d => d.Debtor_Id,
                da => da.Debtor_Id,
                (d, da) => new { Debtor = d, Debtor_Account = da })
                .FirstOrDefault();

            if (debtor == null)
                return null;

            var accountId = debtor.Debtor_Account.Debtor_Account_Id;
            var transactions = _context.Debtor_Account
                .Where(da => da.Debtor_Account_Id == accountId)
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
                var creditorBenefits = transaction.Transaction_Out.Creditor_Benefits_Per_Annum;
                var debtorBenefits = transaction.Transaction_Out.Debtor_Benefits_Per_Annum;
                var days = transaction.Transaction_Out.Day_Access_To_Funds;
                var partOfYear = (float)((float)days / 365);
                var realCreditorBenefits = ((int)((float)partOfYear * creditorBenefits));
                var realDebtorBenefits = ((int)((float)partOfYear * debtorBenefits));

                listOfDebtorTransactions.Add(new TransactionWithUserViewModel
                {
                    Amount = transaction.Transaction_Out.Ammount,
                    DebtorAccountFinappAmount = transaction.Transaction_Out.Finapp_Debetor ?? 0,
                    DebtorUsername = debtor.Debtor.username,
                    Date = transaction.Transaction_Out.Date_Of_Transaction ?? DateTime.Now,
                    ROI = (float)transaction.Transaction_Out.ROI,
                    APR = debtor.Debtor.Delta_APR,
                    CreditorUsername = creditor.username,
                    CreditorAccountFinappAmount = transaction.Transaction_Out.Finapp_Creditor ?? 100,
                    CreditorBenefits = creditorBenefits ?? 0,
                    DebtorBenefits = debtorBenefits ?? 0,
                    RealCreditorBenefits = realCreditorBenefits,
                    RealDebtorBenefits = realDebtorBenefits,
                    DayAccessToFunds = transaction.Transaction_Out.Day_Access_To_Funds
                });
            }

            return listOfDebtorTransactions;
        }

        public IEnumerable<TransactionWithUserViewModel> GetTransactionWithCreditorByCreditorUsername(string username)
        {
            var creditor = _context.Creditor.Where(c => c.username == username)
                .Join(_context.Creditor_Account,
                c => c.Creditor_Id,
                ca => ca.Creditor_Id,
                (c, ca) => new { Creditor = c, Creditor_Account = ca })
                .FirstOrDefault();

            if (creditor == null)
                return null;

            var accountId = creditor.Creditor_Account.Creditor_Account_Id;
            var transactions = _context.Creditor_Account
                .Where(ca => ca.Creditor_Account_Id == accountId)
                .Join(_context.Transaction_Out,
                ca => ca.Creditor_Account_Id,
                t => t.Creditor_Account_Id,
                (ca, t) => new { Creditor_Account = ca, Transaction_Out = t }
                ).ToList();

            if (transactions == null)
                return null;

            List<TransactionWithUserViewModel> listOfDebtorTransactions = new List<TransactionWithUserViewModel>();

            foreach (var transaction in transactions)
            {
                var debtorAccount = _context.Transaction_Out.Where(t => t.Transaction_Out_Id == transaction.Transaction_Out.Transaction_Out_Id)
                    .Join(_context.Debtor_Account,
                    t => t.Debtor_Account_Id,
                    da => da.Debtor_Account_Id,
                    (t, da) => new { Transaction_Out = t, Debtor_Account = da }).FirstOrDefault();

                var debtor = _debtorService.GetDebtorById(debtorAccount.Debtor_Account.Debtor_Id);
                var creditorBenefits = transaction.Transaction_Out.Creditor_Benefits_Per_Annum;
                var debtorBenefits = transaction.Transaction_Out.Debtor_Benefits_Per_Annum;
                var days = transaction.Transaction_Out.Day_Access_To_Funds;
                var partOfYear = (float)((float)days / 365);
                var realCreditorBenefits = ((int)((float)partOfYear * creditorBenefits));
                var realDebtorBenefits = ((int)((float)partOfYear * debtorBenefits));
                //var creditorBenefits = (transaction.Transaction_Out.Ammount * (float)(transaction.Transaction_Out.ROI / 100));
                // var debtorBenefits = (transaction.Transaction_Out.Ammount * (float)((debtor.APR - debtor.EAPR) / 100));

                listOfDebtorTransactions.Add(new TransactionWithUserViewModel
                {
                    Amount = transaction.Transaction_Out.Ammount,
                    DebtorAccountFinappAmount = transaction.Transaction_Out.Finapp_Debetor ?? 0,
                    DebtorUsername = debtor.username,
                    Date = transaction.Transaction_Out.Date_Of_Transaction ?? DateTime.Now,
                    ROI = (float)transaction.Transaction_Out.ROI,
                    APR = debtor.Delta_APR,
                    CreditorUsername = creditor.Creditor.username,
                    CreditorAccountFinappAmount = transaction.Transaction_Out.Finapp_Creditor ?? 100,
                    CreditorBenefits = creditorBenefits ?? 0,
                    DebtorBenefits = debtorBenefits ?? 0,
                    RealCreditorBenefits = realCreditorBenefits,
                    RealDebtorBenefits = realDebtorBenefits,
                    DayAccessToFunds = transaction.Transaction_Out.Day_Access_To_Funds
                });
            }

            return listOfDebtorTransactions;

        }

        public IEnumerable<TransactionWithUserViewModel> GetTransactions()
        {
            var debtors = _context.Debtor
               .Join(_context.Debtor_Account,
               d => d.Debtor_Id,
               da => da.Debtor_Id,
               (d, da) => new { Debtor = d, Debtor_Account = da })
               .ToList();

            if (debtors == null)
                return null;

            List<TransactionWithUserViewModel> listOfDebtorTransactions = new List<TransactionWithUserViewModel>();

            foreach (var debtor in debtors)
            {
                var accountId = debtor.Debtor_Account.Debtor_Account_Id;
                var transactions = _context.Debtor_Account
                    .Where(da => da.Debtor_Account_Id == accountId)
                    .Join(_context.Transaction_Out,
                    da => da.Debtor_Account_Id,
                    t => t.Debtor_Account_Id,
                    (da, t) => new { Debtor_Account = da, Transaction_Out = t }
                    ).ToList();

                if (transactions == null)
                    return null;

                foreach (var transaction in transactions)
                {
                    var creditorAccount = _context.Transaction_Out
                        .Where(t => t.Transaction_Out_Id == transaction.Transaction_Out.Transaction_Out_Id)
                        .Join(_context.Creditor_Account,
                        t => t.Creditor_Account_Id,
                        ca => ca.Creditor_Account_Id,
                        (t, ca) => new { Transaction_Out = t, Creditor_Account = ca }).FirstOrDefault();

                    var creditor = _creditorService.GetCreditorById(creditorAccount.Creditor_Account.Creditor_Id);
                    var creditorBenefits = transaction.Transaction_Out.Creditor_Benefits_Per_Annum;
                    var debtorBenefits = transaction.Transaction_Out.Debtor_Benefits_Per_Annum;
                    var days = transaction.Transaction_Out.Day_Access_To_Funds;
                    var partOfYear = ((float)days / 365);
                    var realCreditorBenefits = ((int)((float)partOfYear * creditorBenefits));
                    var realDebtorBenefits = ((int)((float)partOfYear * debtorBenefits));

                    listOfDebtorTransactions.Add(new TransactionWithUserViewModel
                    {
                        Amount = transaction.Transaction_Out.Ammount,
                        DebtorAccountFinappAmount = transaction.Transaction_Out.Finapp_Debetor ?? 0,
                        DebtorUsername = debtor.Debtor.username,
                        Date = transaction.Transaction_Out.Date_Of_Transaction ?? DateTime.Now,
                        ROI = (float)transaction.Transaction_Out.ROI,
                        APR = debtor.Debtor.Delta_APR,
                        CreditorUsername = creditor.username,
                        CreditorAccountFinappAmount = transaction.Transaction_Out.Finapp_Creditor ?? 100,
                        CreditorBenefits = creditorBenefits ?? 0,
                        DebtorBenefits = debtorBenefits ?? 0,
                        RealCreditorBenefits = realCreditorBenefits,
                        RealDebtorBenefits = realDebtorBenefits,
                        DayAccessToFunds = transaction.Transaction_Out.Day_Access_To_Funds
                    });
                }
            }

            return listOfDebtorTransactions;
        }

        public TransactionWithUserViewModel CreateTransactionWithUserViewModel(Transaction_Out transaction)
        {
            var transactionCreditor = _context.Transaction_Out
                .Where(t => t.Transaction_Out_Id == transaction.Transaction_Out_Id)
                .Join(_context.Creditor_Account,
                t => t.Creditor_Account_Id,
                ca => ca.Creditor_Account_Id,
                (t, ca) => new { Transaction_Out = t, Creditor_Account = ca })
                .FirstOrDefault();

            var transactionDebtor = _context.Transaction_Out
                .Where(t => t.Transaction_Out_Id == transaction.Transaction_Out_Id)
                .Join(_context.Debtor_Account,
                t => t.Debtor_Account_Id,
                da => da.Debtor_Account_Id,
                (t, da) => new { Transaction_Out = t, Debtor_Account = da })
                .FirstOrDefault();

            var debtor = _debtorService.GetDebtorById(transactionDebtor.Debtor_Account.Debtor_Id);
            var creditor = _creditorService.GetCreditorById(transactionCreditor.Creditor_Account.Creditor_Id);
            var days = transaction.Day_Access_To_Funds;
            var partOfYear = (float)((float)days / 365);
            var realCreditorBenefits = ((int)((float)partOfYear * transaction.Creditor_Benefits_Per_Annum ?? 0));
            var realDebtorBenefits = ((int)((float)partOfYear * transaction.Debtor_Benefits_Per_Annum ?? 0));

            return new TransactionWithUserViewModel
            {
                Amount = transaction.Ammount,
                DebtorAccountFinappAmount = transaction.Finapp_Debetor ?? 0,
                DebtorUsername = debtor.username,
                Date = transaction.Date_Of_Transaction ?? DateTime.Now,
                ROI = (float)transaction.ROI,
                APR = debtor.Delta_APR,
                CreditorUsername = creditor.username,
                CreditorAccountFinappAmount = transaction.Finapp_Creditor ?? 100,
                CreditorBenefits = transaction.Creditor_Benefits_Per_Annum ?? 0,
                DebtorBenefits = transaction.Debtor_Benefits_Per_Annum ?? 0,
                RealCreditorBenefits = realCreditorBenefits,
                RealDebtorBenefits = realDebtorBenefits,
                DayAccessToFunds = transaction.Day_Access_To_Funds
            };
        }
    }
}