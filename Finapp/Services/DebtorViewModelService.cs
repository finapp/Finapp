using Finapp.IServices;
using Finapp.Models;
using Finapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class DebtorViewModelService : IDebtorViewModelService
    {
        private readonly IDebtorService _debtorService;

        public DebtorViewModelService(IDebtorService debtorService)
        {
            _debtorService = debtorService;
        }

        public DebtorListViewModel GetAllDebtorsViewModel()
        {
            IEnumerable<Debtor> debtors = _debtorService.GetAllDebtors();

            return CreateListViewModel(debtors);
        }

        public DebtorListViewModel GetWithDebetDebtorsViewModel()
        {
            IEnumerable<Debtor> debtors = _debtorService.GetDebtorsWithDebet();

            return CreateListViewModel(debtors);
        }

        private DebtorListViewModel CreateListViewModel(IEnumerable<Debtor> debtors)
        {
            var debtorsViewModel = new DebtorListViewModel();

            foreach (var debtor in debtors)
            {
                var accessDays = debtor.Expiration_Date.Value.Subtract(DateTime.Now).Days;
                var expectedSavings = (int)(((float)debtor.Delta_APR / 100) * debtor.Debet * (float)accessDays/365);
                debtorsViewModel.List.Add(new DebtorViewModel
                {
                    Username = debtor.username,
                    APR = debtor.APR,
                    EAPR = debtor.EAPR,
                    Debet = debtor.Debet,
                    FinappDebet = debtor.Finapp_Debet,
                    Expiration_Date = debtor.Expiration_Date ?? DateTime.Now,
                    Queue_Date = debtor.Queue_Date ?? DateTime.Now,
                    AccessDays = accessDays,
                    ExpectedSavings = expectedSavings
                });
            }
            return debtorsViewModel;
        }
    }
}