using Finapp.IServices;
using Finapp.Models;
using Finapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class AssociateViewModelService : IAssociateViewModelService
    {
        private readonly FinapEntities1 _context;
        private readonly ITransactionOutService _transactionService;
        private readonly IDebtorAccountService _debtorAccountService;
        private readonly ICreditorAccountService _creditorAccountService;

        public AssociateViewModelService(FinapEntities1 context, ITransactionOutService transactionService, IDebtorAccountService debtorAccountService, ICreditorAccountService creditorAccountService)
        {
            _context = context;
            _transactionService = transactionService;
            _debtorAccountService = debtorAccountService;
            _creditorAccountService = creditorAccountService;
        }

        public IEnumerable<AssociateViewModel> GetAllTransactions()
        {
            var assotiations = _context.Associate.ToList();

            AssociateViewModel oneAssociate;
            List<AssociateViewModel> returnedList = new List<AssociateViewModel>();

            foreach (var associate in assotiations)
            {
                var listOfTransactions = _context.Associate
                    .Where(a => a.Associate_Id == associate.Associate_Id)
                    .Join(_context.Transaction_Out,
                    a => a.Associate_Id,
                    t => t.Associate_Id,
                    (a, t) => new { Associate = a, Transaction_Out = t })
                    .ToList();

                oneAssociate = new AssociateViewModel();

                oneAssociate.Date = associate.Date_Of_Associating ?? DateTime.Now;
                oneAssociate.AssociateId = associate.Associate_Id;

                foreach (var transaction in listOfTransactions)
                {
                    oneAssociate.List.Add(_transactionService.CreateTransactionWithUserViewModel(transaction.Transaction_Out));
                }

                if (oneAssociate.List.Count > 0)
                    returnedList.Add(oneAssociate);
            }

            return returnedList;
        }

        public IEnumerable<AssociateViewModel> GetTransactionsByDebtorUsername(string username)
        {
            var debtorAccountId = _debtorAccountService.GetAccountIdByDebtorUsername(username);
            var assotiations = _context.Associate.ToList();
            AssociateViewModel oneAssociate;
            List<AssociateViewModel> returnedList = new List<AssociateViewModel>();

            foreach (var associate in assotiations)
            {
                var listOfTransactions = _context.Transaction_Out
                    .Where(t => t.Debtor_Account_Id == debtorAccountId && t.Associate_Id == associate.Associate_Id)
                    .Join(_context.Debtor_Account,
                    t => t.Debtor_Account_Id,
                    da => da.Debtor_Account_Id,
                    (t, da) => new { Transaction_Out = t, Debtor_Account = da })
                    .ToList();

                oneAssociate = new AssociateViewModel();

                oneAssociate.Date = associate.Date_Of_Associating ?? DateTime.Now;
                foreach (var transaction in listOfTransactions)
                {
                    oneAssociate.List.Add(_transactionService.CreateTransactionWithUserViewModel(transaction.Transaction_Out));
                }

                if (oneAssociate.List.Count > 0)
                    returnedList.Add(oneAssociate);
            }

            return returnedList;
        }

        public IEnumerable<AssociateViewModel> GetTransactionsByCreditorUsername(string username)
        {
            var creditorAccountId = _creditorAccountService.GetAccountIdByCreditorUsername(username);
            var assotiations = _context.Associate.ToList();
            AssociateViewModel oneAssociate;
            List<AssociateViewModel> returnedList = new List<AssociateViewModel>();

            foreach (var associate in assotiations)
            {
                var listOfTransactions = _context.Transaction_Out
                    .Where(t => t.Creditor_Account_Id == creditorAccountId && t.Associate_Id == associate.Associate_Id)
                    .Join(_context.Creditor_Account,
                    t => t.Creditor_Account_Id,
                    ca => ca.Creditor_Account_Id,
                    (t, ca) => new { Transaction_Out = t, Creditor_Account = ca })
                    .ToList();

                oneAssociate = new AssociateViewModel();

                oneAssociate.Date = associate.Date_Of_Associating ?? DateTime.Now;
                foreach (var transaction in listOfTransactions)
                {
                    oneAssociate.List.Add(_transactionService.CreateTransactionWithUserViewModel(transaction.Transaction_Out));
                }

                if (oneAssociate.List.Count > 0)
                    returnedList.Add(oneAssociate);
            }

            return returnedList;
        }
    }
}