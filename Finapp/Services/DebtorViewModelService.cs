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
        private readonly FinapEntities _context;

        public DebtorViewModelService(FinapEntities context)
        {
            _context = context;
        }

        public DebtorListViewModel DebtorsViewModel()
        {
            IEnumerable<Debtor> debtors = _context.Debtor.ToList();

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