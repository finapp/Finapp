using Finapp.IServices;
using Finapp.Models;
using Finapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class PeopleWithoutAssociateViewModelService : IPeopleWithoutAssociateViewModelService
    {

        private readonly FinapEntities1 _context;
        private readonly ITransactionOutService _transactionService;
        private readonly ICreditorService _creditorService;
        private readonly IDebtorService _debtorService;
        private readonly IDebtorAccountService _debtorAccountService;
        private readonly IDebtorViewModelService _debtorViewModelService;
        private readonly IAssociateViewModelService _associateService;

        public PeopleWithoutAssociateViewModelService(FinapEntities1 context, ITransactionOutService transactionService, 
            ICreditorService creditorService, IDebtorService debtorService, IDebtorAccountService debtorAccountService, 
            IDebtorViewModelService debtorViewModelService, IAssociateViewModelService associateService)
        {
            _context = context;
            _transactionService = transactionService;
            _creditorService = creditorService;
            _debtorService = debtorService;
            _debtorAccountService = debtorAccountService;
            _debtorViewModelService = debtorViewModelService;
            _associateService = associateService;
        }

        public IEnumerable<PeopleWithoutAssociateViewModel> GetSummary()
        {
            var associations = _associateService.GetAllTransactions();
            var debtors = new List<Debtor>();
            var listOfDebtorAccounts = _debtorAccountService.GetAllAccounts();

            List<PeopleWithoutAssociateViewModel> returnedList = new List<PeopleWithoutAssociateViewModel>();

            foreach (var associate in associations)
            {
                debtors.Clear();

                var listOfTransactions = associate.List;
                foreach (var debtorAccount in listOfDebtorAccounts)
                {
                    var debtorHaveTransaction = _context.Transaction_Out
                    .Where(t => t.Date_Of_Transaction == associate.Date)
                    .Any(tr => tr.Debtor_Account_Id == debtorAccount.Debtor_Account_Id);

                    if (!debtorHaveTransaction)
                    {
                        var debtor = _debtorAccountService.GetDebtorByAccountId(debtorAccount.Debtor_Account_Id);

                        if(debtor.Finapp_Debet == debtor.Debet)
                            debtors.Add(debtor);
                    }
                }

                PeopleWithoutAssociateViewModel model = new PeopleWithoutAssociateViewModel();
                var debtorsViewModel = _debtorViewModelService.CreateListViewModel(debtors);
                model.DebtorListWithoutAssociate = debtorsViewModel;

                returnedList.Add(model);
                int a = 6;
            }

            return returnedList;
        }
    }
}
