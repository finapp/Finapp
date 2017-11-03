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
        private readonly IDebtorAccountService _debtorService;
        private readonly ICreditorAccountService _creditorService;

        public TransactionViewModelService(FinapEntities context, IDebtorAccountService debtorService, ICreditorAccountService creditorService)
        {
            _context = context;
            _debtorService = debtorService;
            _creditorService = creditorService;
        }

        public TransactionOutListViewModel GetTransactionByDeptorId(int id)
        {
            var accountId = _debtorService.GetAccountIdByDebtorId(id);
            var transactions = _context.Transaction_Out.Where(t => t.Debtor_Account_Id == accountId).ToList();
            var transactionsViewModel = new TransactionOutListViewModel();

            foreach (var transaction in transactions)
            {
                transactionsViewModel.List.Add(new TransactionOutViewModel
                {
                    Amount = transaction.Ammount,
                    Date = transaction.Date_Of_Transaction
                });
            }

            return transactionsViewModel;
        }

        public TransactionOutListViewModel GetTransactionByCreditorId(int id)
        {
            var accountId = _creditorService.GetAccountIdByCreditorId(id);
            var transactions = _context.Transaction_Out.Where(t => t.Creditor_Account_Id == accountId).ToList();
            var transactionsViewModel = new TransactionOutListViewModel();

            foreach (var transaction in transactions)
            {
                transactionsViewModel.List.Add(new TransactionOutViewModel
                {
                    Amount = transaction.Ammount,
                    Date = transaction.Date_Of_Transaction
                });
            }

            return transactionsViewModel;
        }
    }
}