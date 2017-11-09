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

        public DebtorListViewModel DebtorsViewModel()
        {
            IEnumerable<Debtor> debtors = _debtorService.GetAllDebtors();

            var debtorsViewModel = new DebtorListViewModel();

            foreach (var debtor in debtors)
            {
                debtorsViewModel.List.Add(new DebtorViewModel
                {
                    Username = debtor.username,
                    APR = debtor.APR,
                    EAPR = debtor.EAPR,
                    Debet = debtor.Debet,
                    FinappDebet = debtor.Finapp_Debet
                });
            }

            return debtorsViewModel;
        }
    }
}