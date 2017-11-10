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

        public AssociateViewModelService(FinapEntities1 context, ITransactionOutService transactionService)
        {
            _context = context;
            _transactionService = transactionService;
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
                foreach (var transaction in listOfTransactions)
                {
                    oneAssociate.List.Add(_transactionService.CreateTransactionWithUserViewModel(transaction.Transaction_Out));
                }

                returnedList.Add(oneAssociate);
            }

            return returnedList;
        }
    }
}