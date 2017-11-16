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
        private readonly IAssociateService _associateService;
        private readonly FinapEntities1 _context;

        public CreditorViewModelService(ICreditorService creditorService, IAssociateService associateService, FinapEntities1 context)
        {
            _creditorService = creditorService;
            _associateService = associateService;
            _context = context;
        }

        public IEnumerable<CreditorViewModel> GetAllCreditorsViewModel()
        {
            IEnumerable<Creditor> creditors = _creditorService.GetAllCreditors();

            return CreateCreditorsViewModel(creditors);
        }

        public IEnumerable<CreditorViewModel> GetWithBalanceCreditorsViewModel()
        {
            IEnumerable<Creditor> creditors = _creditorService.GetCreditorsWithBalance();

            return CreateCreditorsViewModel(creditors);
        }

        private IEnumerable<CreditorViewModel> CreateCreditorsViewModel(IEnumerable<Creditor> creditors)
        {
            var creditorViewModel = new List<CreditorViewModel>();

            foreach (var creditor in creditors)
            {
                var accessDays = creditor.Expiration_Date.Value.Subtract(DateTime.Now).Days;
                var expectedProfits = (int)((float)creditor.Delta_ROI / 100 * creditor.Balance * (float)accessDays / 365);
                creditorViewModel.Add(new CreditorViewModel
                {
                    Username = creditor.username,
                    ROI = (float)creditor.ROI,
                    EROI = (float)creditor.EROI,
                    Balance = creditor.Balance,
                    FinappBalance = creditor.Finapp_Balance,
                    Expiration_Date = creditor.Expiration_Date ?? DateTime.Now,
                    Queue_Date = creditor.Queue_Date ?? DateTime.Now,
                    AccessDays = accessDays,
                    ExpectedProfits = expectedProfits
                });
            }

            return creditorViewModel;
        }

        public IEnumerable<IEnumerable<CreditorViewModel>> GetTheWorstCreditors()
        {
            List<IEnumerable<CreditorViewModel>> theWorstCreditors = new List<IEnumerable<CreditorViewModel>>();

            var associations = _associateService.GetAllAssociations();

            foreach (var associate in associations)
            {
                var creditors = GetCreditorsWithoutTransactions(associate);
                theWorstCreditors.Add(creditors);
            }

            return null;
        }

        public IEnumerable<CreditorViewModel> GetCreditorsWithoutTransactions(Associate associate)
        {
            var creditors = _creditorService.GetAllCreditors();
            List<Creditor> theWorstCreditors = new List<Creditor>();
            bool isInAssociate;

            foreach (var creditor in creditors)
            {
                isInAssociate = false;
                foreach (var a in creditor.Associate)
                {
                    if (a.Associate_Id == associate.Associate_Id)
                    {
                        isInAssociate = true;
                        break;
                    }
                }
                if (!isInAssociate)
                {
                    var addedCreditor = _creditorService;
                    theWorstCreditors.Add(creditor);
                }

            }

            return CreateCreditorsViewModel(theWorstCreditors);
        }
    }
}