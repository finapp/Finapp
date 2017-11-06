using Finapp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Finapp.Models;
using System.Threading.Tasks;

namespace Finapp.Algorithm
{
    public class Algorithm : IAlgorithm
    {
        private readonly IDebtor _debtorService;
        private readonly ICreditor _creditorService;
        private readonly ICreditorAccount _creditorAccountService;
        private readonly IDebtorAccount _debtorAccountService;
        private readonly ITransactionOut _transactionService;

        public Algorithm(IDebtor debtorService, ICreditor creditorService, ICreditorAccount creditorAccountService, IDebtorAccount debtorAccountService, ITransactionOut transactionService)
        {
            _debtorService = debtorService;
            _creditorService = creditorService;
            _creditorAccountService = creditorAccountService;
            _debtorAccountService = debtorAccountService;
            _transactionService = transactionService;
        }

        public bool MergeDebtorWithCreditors()
        {
            var selectedDebtor = _debtorService.GetAvaialbleDebtor();
            if (selectedDebtor == null) return false;

            selectedDebtor.Available = false;
            _debtorService.ModifyDebtor(selectedDebtor);

            IEnumerable<Creditor> availableCreditors = _creditorService.GetAvailableCreditors(selectedDebtor);

            var selectedeCreditorsToMerge = SelectCreditorsToMerge(selectedDebtor, availableCreditors);

            return true;
        }

        public IEnumerable<Creditor> SelectCreditorsToMerge(Debtor debtor, IEnumerable<Creditor> availablesCreditors)
        {
            var suma = 0;
            List<Creditor> selectedCreditors = new List<Creditor>();

            foreach (var creditor in availablesCreditors)
            {
                if (suma + creditor.Finapp_Balance <= debtor.Debet && creditor.Available == true)
                {
                    suma += creditor.Finapp_Balance;

                    debtor.Finapp_Debet -= creditor.Finapp_Balance;
                    _debtorService.ModifyDebtor(debtor);

                    creditor.Available = false;
                    creditor.Finapp_Balance = 0;
                    _creditorService.ModifyCreditor(creditor);

                    CreateTransactionOut(debtor, creditor, creditor.Balance);

                    selectedCreditors.Add(creditor);

                }
                else if (creditor.Available == true)
                {
                    var a = debtor.Debet - suma;
                    creditor.Finapp_Balance -= a;
                    _creditorService.ModifyCreditor(creditor);

                    CreateTransactionOut(debtor, creditor, a);

                    debtor.Finapp_Debet = 0;
                    _debtorService.ModifyDebtor(debtor);

                    selectedCreditors.Add(creditor);
                    break;
                }
            }

            return selectedCreditors;
        }

        public bool CreateTransactionOut(Debtor debtor, Creditor creditor, int amount)
        {
            var creditor_Account = _creditorAccountService.getCreditorAccountByCreditorId(creditor.Creditor_Id);

            var debtor_Account = _debtorAccountService.getDebtorAccountByDebtorId(debtor.Debtor_Id);

            if (creditor_Account == null || debtor_Account == null) return false;

            var newTransaction = new Transaction_Out
            {
                Date_Of_Transaction = DateTime.Now,
                Ammount = amount,
                Creditor_Account_Id = creditor_Account.Creditor_Account_Id,
                Debtor_Account_Id = debtor_Account.Debtor_Account_Id

            };
            _transactionService.CreateTransaction(newTransaction);

            return true;
        }


    }
}