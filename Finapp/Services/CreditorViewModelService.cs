using Finapp.IServices;
using Finapp.Models;
using Finapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class CreditorViewModelService : ICreditorViewModelService
    {
        private readonly FinapEntities _context;

        public CreditorViewModelService(FinapEntities context)
        {
            _context = context;
        }

        public CreditorListViewModel CreditorsViewModel()
        {
            IEnumerable<Creditor> creditors = _context.Creditor.ToList();

            var creditorViewModel = new CreditorListViewModel();

            foreach (var creditor in creditors)
            {
                creditorViewModel.List.Add(new CreditorViewModel
                {
                    Username = creditor.username,
                    ROI = creditor.ROI,
                    EROI = creditor.EROI,
                    Balance = creditor.Balance,
                    FinappBalance = creditor.Finapp_Balance
                });
            }

            return creditorViewModel;
        }
    }
}