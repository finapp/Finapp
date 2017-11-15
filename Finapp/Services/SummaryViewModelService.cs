using Finapp.IServices;
using Finapp.Models;
using Finapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class SummaryViewModelService
    {
        private readonly FinapEntities1 _context;
        private readonly ITransactionOutService _transactionService;
        private readonly ICreditorService _creditorService;
        private readonly IDebtorService _debtorService;
        private readonly IAssociateViewModelService _associateService;

        public SummaryViewModelService(FinapEntities1 context, ITransactionOutService transactionService, ICreditorService creditorService, IDebtorService debtorService, IAssociateViewModelService associateService)
        {
            _context = context;
            _transactionService = transactionService;
            _creditorService = creditorService;
            _debtorService = debtorService;
            _associateService = associateService;
        }

        public IEnumerable<AssociateViewModel> GetTransactions()
        {
            return _associateService.GetAllTransactions();
        }

        public IEnumerable<PeopleWithoutAssociateViewModel> GetSummary()
        {
            return null;
        }
    }
}