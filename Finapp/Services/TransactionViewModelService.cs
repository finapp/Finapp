using Finapp.IServices;
using Finapp.Models;
using Finapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class TransactionViewModelService : ITransactionViewModelService
    {
        private readonly FinapEntities _context;
        private readonly IDebtorAccountService _debtorAccountService;
        private readonly ICreditorAccountService _creditorAccountService;
        private readonly IDebtorService _debtorService;
        private readonly ICreditorService _creditorService;

        public TransactionViewModelService(FinapEntities context, IDebtorAccountService debtorAccountService, ICreditorAccountService creditorAccountService,
            IDebtorService debtorService, ICreditorService creditorService)
        {
            _context = context;
            _debtorAccountService = debtorAccountService;
            _creditorAccountService = creditorAccountService;
            _debtorService = debtorService;
            _creditorService = creditorService;
        }

        public TransactionOutListViewModel GetAllTransactions()
        {

            var transactions = _context.Transaction_Out.ToList();
            var transactionsViewModel = new TransactionOutListViewModel();

            foreach (var transaction in transactions)
            {
                transactionsViewModel.List.Add(new TransactionOutViewModel
                {
                    Amount = transaction.Ammount,
                    Date = transaction.Date_Of_Transaction,
                    DebtorUsername = _debtorService.GetDebtorUsernameById(_debtorAccountService.GetDebtorIdByAccountId(transaction.Debtor_Account_Id)),
                    CreditorUsername = _creditorService.GetCreditorUsernameById(_creditorAccountService.GetCreditorIdByAccountId(transaction.Creditor_Account_Id))
                });
            }

            return transactionsViewModel;
        }

        public TransactionOutListViewModel GetTransactionByDeptorId(int id)
        {
            var accountId = _debtorAccountService.GetAccountIdByDebtorId(id);
            var transactions = _context.Transaction_Out.Where(t => t.Debtor_Account_Id == accountId).ToList();
            var transactionsViewModel = new TransactionOutListViewModel();

            foreach (var transaction in transactions)
            {
                transactionsViewModel.List.Add(new TransactionOutViewModel
                {
                    Amount = transaction.Ammount,
                    Date = transaction.Date_Of_Transaction,
                    DebtorUsername = _debtorService.GetDebtorUsernameById(_debtorAccountService.GetDebtorIdByAccountId(transaction.Debtor_Account_Id)),
                    CreditorUsername = _creditorService.GetCreditorUsernameById(_creditorAccountService.GetCreditorIdByAccountId(transaction.Creditor_Account_Id))
                });
            }

            return transactionsViewModel;
        }

        public TransactionOutListViewModel GetTransactionByCreditorId(int id)
        {
            var accountId = _creditorAccountService.GetAccountIdByCreditorId(id);
            var transactions = _context.Transaction_Out.Where(t => t.Creditor_Account_Id == accountId).ToList();
            var transactionsViewModel = new TransactionOutListViewModel();

            foreach (var transaction in transactions)
            {
                transactionsViewModel.List.Add(new TransactionOutViewModel
                {
                    Amount = transaction.Ammount,
                    Date = transaction.Date_Of_Transaction,
                    DebtorUsername = _debtorService.GetDebtorUsernameById(_debtorAccountService.GetDebtorIdByAccountId(transaction.Debtor_Account_Id)),
                    CreditorUsername = _creditorService.GetCreditorUsernameById(_creditorAccountService.GetCreditorIdByAccountId(transaction.Creditor_Account_Id))
                });
            }

            return transactionsViewModel;
        }

        public TransactionOutListViewModel GetTransactionByCreditorUsername(string username)
        {
            var accountId = _creditorAccountService.GetAccountIdByCreditorUsername(username);
            var transactions = _context.Transaction_Out.Where(t => t.Creditor_Account_Id == accountId).ToList();
            var transactionsViewModel = new TransactionOutListViewModel();

            foreach (var transaction in transactions)
            {
                transactionsViewModel.List.Add(new TransactionOutViewModel
                {
                    Amount = transaction.Ammount,
                    Date = transaction.Date_Of_Transaction,
                    DebtorUsername = _debtorService.GetDebtorUsernameById(_debtorAccountService.GetDebtorIdByAccountId(transaction.Debtor_Account_Id)),
                    CreditorUsername = _creditorService.GetCreditorUsernameById(_creditorAccountService.GetCreditorIdByAccountId(transaction.Creditor_Account_Id))
                });
            }

            return transactionsViewModel;
        }

        public TransactionOutListViewModel GetTransactionByDebtorUsername(string username)
        {
            var accountId = _debtorAccountService.GetAccountIdByDebtorUsername(username);
            var transactions = _context.Transaction_Out.Where(t => t.Debtor_Account_Id == accountId).ToList();
            var transactionsViewModel = new TransactionOutListViewModel();

            foreach (var transaction in transactions)
            {
                transactionsViewModel.List.Add(new TransactionOutViewModel
                {
                    Amount = transaction.Ammount,
                    Date = transaction.Date_Of_Transaction,
                    DebtorUsername = _debtorService.GetDebtorUsernameById(_debtorAccountService.GetDebtorIdByAccountId(transaction.Debtor_Account_Id)),
                    CreditorUsername = _creditorService.GetCreditorUsernameById(_creditorAccountService.GetCreditorIdByAccountId(transaction.Creditor_Account_Id))
                });
            }

            return transactionsViewModel;
        }
    }
}