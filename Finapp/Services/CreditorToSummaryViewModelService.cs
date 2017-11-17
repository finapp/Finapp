using Finapp.IServices;
using Finapp.Models;
using Finapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class CreditorToSummaryViewModelService : ICreditorToSummaryViewModelService
    {
        public CreditorToSummaryViewModel CreateViewModel(Creditor creditor, int transactions, int associateId)
        {
            return new CreditorToSummaryViewModel
            {
                DayAccessToFunds = creditor.Expiration_Date.Value.Subtract(DateTime.Now).Days,
                EAPR = (int)creditor.EROI,
                TransactionCounter = transactions,
                Username = creditor.username,
                AssociateId = associateId
            };
        }
    }
}