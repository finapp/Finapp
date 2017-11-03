using Finapp.Interfaces;
using Finapp.IServices;
using Finapp.Models;
using Finapp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.Implementations
{
    public class Algorithms : IAlgorithms
    {
        private readonly FinapEntities _context;
        private readonly ICreditorService _creditorService;
        private readonly IDebtorService _debtorService;

        public Algorithms(FinapEntities context, ICreditorService creditorService, IDebtorService debtorService)
        {
            _context = context;
            _creditorService = creditorService;
            _debtorService = debtorService;
        }

        public IEnumerable<Debtor> AddDebtorsToQueue()
        {
            var debtorsList = _debtorService.GetAvailableDebtors();

            if (debtorsList == null)
                return null;

            debtorsList.OrderBy(d => d.Queue_Date);

            return debtorsList;
        }

        public IEnumerable<Creditor> AddCreditorsToQueue()
        {
            var creditorsList = _creditorService.GetAvailableCreditors();

            if (creditorsList == null)
                return null;

            creditorsList.OrderBy(d => d.Queue_Date);

            return creditorsList;
        }

        public bool Associating()
        {
            IEnumerable<Debtor> debtors = AddDebtorsToQueue();
            IEnumerable<Creditor> creditors = AddCreditorsToQueue();

            if (debtors == null || creditors == null)
                return false;

            foreach (var debtor in debtors)
            {
                foreach (var creditor in creditors)
                {
                    if (debtor.EAPR > creditor.EROI)
                    {
                        if (creditor.Finapp_Balance > debtor.Finapp_Debet)
                        {
                            creditor.Finapp_Balance -= debtor.Finapp_Debet;
                            _creditorService.ModifyCreditor(creditor);

                            debtor.Finapp_Debet = 0;
                            debtor.Available = false;
                            _debtorService.ModifyDebtor(debtor);

                            break;
                        }
                        else
                        {
                            debtor.Finapp_Debet -= creditor.Finapp_Balance;
                            _debtorService.ModifyDebtor(debtor);

                            creditor.Finapp_Balance = 0;
                            creditor.Available = false;
                            _creditorService.ModifyCreditor(creditor);

                            if (debtor.Finapp_Debet == 0)
                                break;
                        }
                    }
                }
            }

            return true;
        }


    }
}