using Finapp.IServices;
using Finapp.Models;
using Finapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class DebtorToSummaryViewModelService : IDebtorToSummaryViewModelService
    {
        private readonly IDebtorService _debtorService;

        public DebtorToSummaryViewModelService(IDebtorService debtorService)
        {
            _debtorService = debtorService;
        }

        public DebtorToSummaryViewModel CreateViewModel(Debtor debtor, int transactions, int associateId)
        {
            return new DebtorToSummaryViewModel
            {
                DayAccessToFunds = debtor.Expiration_Date.Value.Subtract(DateTime.Now).Days,
                ExpSavings = (int)debtor.APR - (int)debtor.EAPR,
                TransactionCounter = transactions,
                Username = debtor.username,
                AssociateId = associateId
            };
        }
    }
}