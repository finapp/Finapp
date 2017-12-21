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
        private readonly IAssociateService _associateService;

        public AssociateViewModelService(FinapEntities1 context, ITransactionOutService transactionService, IAssociateService associateService)
        {
            _context = context;
            _transactionService = transactionService;
            _associateService = associateService;
        }

        public IEnumerable<AssociateViewModel> GetAllTransactions()
        {
            var assotiations = _associateService.GetAllAssociations();
            var number = 1;

            AssociateViewModel oneAssociate;
            List<AssociateViewModel> returnedList = new List<AssociateViewModel>();

            foreach (var associate in assotiations)
            {
                var listOfTransactions = _context.Transaction_Out
                    .Where(a => a.Associate_Id == associate.Associate_Id)
                    .ToList();

                oneAssociate = new AssociateViewModel();

                oneAssociate.Date = associate.Date_Of_Associating ?? DateTime.Now;
                oneAssociate.AssociateId = associate.Associate_Id;
                oneAssociate.Number = number;

                number++;

                foreach (var transaction in listOfTransactions)
                {
                    oneAssociate.List.Add(_transactionService.CreateTransactionWithUserViewModel(transaction));
                }

                if (oneAssociate.List.Count > 0)
                    returnedList.Add(oneAssociate);
            }

            return returnedList;
        }

        public IEnumerable<UserTransactionsViewModel> GetTransactionsByDebtorUsername(string username)
        {
            var debtorId = (from d in _context.Debtor
                            where d.username == username
                            select d.Debtor_Id).FirstOrDefault();

            var assotiations = _context.Associate.ToList();
            UserTransactionsViewModel oneAssociate;
            List<UserTransactionsViewModel> returnedList = new List<UserTransactionsViewModel>();
            int amount = 0;
            int profit = 0;

            int number = 1;

            foreach (var associate in assotiations)
            {
                var listOfTransactions = (from t in _context.Transaction_Out
                                          where t.Debtor_Id == debtorId && t.Associate_Id == associate.Associate_Id
                                          select new { t.Ammount, t.DebtorSavings })
                                          .ToList();

                oneAssociate = new UserTransactionsViewModel();

                foreach (var transaction in listOfTransactions)
                {
                    amount += transaction.Ammount;
                    profit += transaction.DebtorSavings??0;
                    
                }

                oneAssociate = _transactionService.GetUserTransactions(amount, number, profit, username, listOfTransactions.Count);

                if (oneAssociate.Amount > 0)
                    returnedList.Add(oneAssociate);

                number++;
                amount = 0;
                profit = 0;
            }

            return returnedList;
        }

        public IEnumerable<UserTransactionsViewModel> GetTransactionsByCreditorUsername(string username)
        {
            var creditorId = (from c in _context.Creditor
                              where c.username == username
                              select c.Creditor_Id).FirstOrDefault();

            var assotiations = _context.Associate.ToList();
            UserTransactionsViewModel oneAssociate;
            List<UserTransactionsViewModel> returnedList = new List<UserTransactionsViewModel>();
            int amount = 0;
            int profit = 0;

            int number = 1;

            foreach (var associate in assotiations)
            {
                var listOfTransactions = (from t in _context.Transaction_Out
                                          where t.Creditor_Id == creditorId && t.Associate_Id == associate.Associate_Id
                                          select new { t.Ammount, t.CreditorBenefits })
                                          .ToList();

                oneAssociate = new UserTransactionsViewModel();

                foreach (var transaction in listOfTransactions)
                {
                    amount += transaction.Ammount;
                    profit += transaction.CreditorBenefits ?? 0;
                }

                oneAssociate = _transactionService.GetUserTransactions(amount, number, profit, username, listOfTransactions.Count);
                returnedList.Add(oneAssociate);

                number++;
                amount = 0;
                profit = 0;
            }

            return returnedList;
        }
    }
}