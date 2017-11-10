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
        private readonly ICreditorService _creditorService;

        public CreditorViewModelService(ICreditorService creditorService)
        {
            _creditorService = creditorService;
        }

        public CreditorListViewModel GetAllCreditorsViewModel()
        {
            IEnumerable<Creditor> creditors = _creditorService.GetAllCreditors();

            return CreateCreditorsViewModel(creditors);
        }

        public CreditorListViewModel GetWithBalanceCreditorsViewModel()
        {
            IEnumerable<Creditor> creditors = _creditorService.GetCreditorsWithBalance();

            return CreateCreditorsViewModel(creditors);
        }

        private CreditorListViewModel CreateCreditorsViewModel(IEnumerable<Creditor> creditors)
        {
            var creditorViewModel = new CreditorListViewModel();

            foreach (var creditor in creditors)
            {
                var accessDays = creditor.Expiration_Date.Value.Subtract(DateTime.Now).Days;
                creditorViewModel.List.Add(new CreditorViewModel
                {
                    Username = creditor.username,
                    ROI = (float)creditor.ROI,
                    EROI = (float)creditor.EROI,
                    Balance = creditor.Balance,
                    FinappBalance = creditor.Finapp_Balance,
                    Expiration_Date = creditor.Expiration_Date ?? DateTime.Now,
                    Queue_Date = creditor.Queue_Date ?? DateTime.Now,
                    AccessDays = accessDays
                });
            }

            return creditorViewModel;
        }
    }
}