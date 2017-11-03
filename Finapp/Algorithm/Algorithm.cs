using Finapp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Finapp.Models;

namespace Finapp.Algorithm
{
    public class Algorithm : IAlgorithm
    {
        private readonly IDebtor _debtorService;
        private readonly ICreditor _creditorService;

        public Algorithm(IDebtor debtorService, ICreditor creditorService)
        {
            _debtorService = debtorService;
            _creditorService = creditorService;
        }

        public bool MergeDebtorWithCreditors()
        {
            var selectedDebtor = _debtorService.GetAvaialbleDebtor();

            selectedDebtor.Available = false;
            _debtorService.ModifyDebtor(selectedDebtor);

            IEnumerable<Creditor> availableCreditors = _creditorService.GetAvailableCreditors(selectedDebtor);

            var selectedeCreditorsToMerge =selectCreditorsToMerge(selectedDebtor, availableCreditors);

            return true;
        }

        public IEnumerable<Creditor> selectCreditorsToMerge(Debtor debtor, IEnumerable<Creditor> availablesCreditors)
        {
            var suma = 0;
            List<Creditor> selectedCreditors = new List<Creditor>();

            foreach (var creditor in availablesCreditors)
            {
                if (suma + creditor.Balance < debtor.Debet && creditor.Available == true)
                {
                    suma += creditor.Balance;

                    debtor.Finapp_Debet -= creditor.Balance;
                    _debtorService.ModifyDebtor(debtor);

                    creditor.Available = false;
                    creditor.Finapp_Balance = 0;
                    _creditorService.ModifyCreditor(creditor);

                    selectedCreditors.Add(creditor);

                }
                else if (creditor.Available == true)
                {
                    creditor.Finapp_Balance -= debtor.Debet - suma;
                    _creditorService.ModifyCreditor(creditor);

                    debtor.Finapp_Debet = 0;
                    _debtorService.ModifyDebtor(debtor);

                    selectedCreditors.Add(creditor);
                    break;
                }

            }

            return selectedCreditors;
        }
    }
}