using Finapp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

            var availableCreditors = _creditorService.GetAvailableCreditors();


            return true;
        }
    }
}