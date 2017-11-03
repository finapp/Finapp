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
        private readonly ITransactionOutService _transactionOutService;
        private readonly ICreditorAccountService _creditorAccountService;
        private readonly IDebtorAccountService _debtorAccountService;

        public Algorithms(FinapEntities context, ICreditorService creditorService, IDebtorService debtorService,
            ITransactionOutService transactionOutService, ICreditorAccountService creditorAccountService,
            IDebtorAccountService debtorAccountService)
        {
            _context = context;
            _creditorService = creditorService;
            _debtorService = debtorService;
            _transactionOutService = transactionOutService;
            _creditorAccountService = creditorAccountService;
            _debtorAccountService = debtorAccountService;
        }

        public bool Associating()
        {
            IEnumerable<Debtor> debtors = AddDebtorsToQueue();

            if (debtors == null)
                return false;

            foreach (var debtor in debtors)
            {
                IEnumerable<Creditor> creditors = AddCreditorsToQueue();
                CreateTransaction(debtor, creditors);
            }

            return true;
        }

        private bool CreateTransaction(Debtor debtor, IEnumerable<Creditor> creditors)
        {
            foreach (var creditor in creditors)
            {
                if (debtor.EAPR > creditor.EROI)
                {
                    if (creditor.Finapp_Balance > debtor.Finapp_Debet)
                    {
                        _transactionOutService.AddTransaction(new Transaction_Out
                        {
                            Ammount = debtor.Finapp_Debet,
                            Date_Of_Transaction = DateTime.Now,
                            Creditor_Account_Id = _creditorAccountService.GetAccountIdByCreditorId(creditor.Creditor_Id),
                            Debtor_Account_Id = _debtorAccountService.GetAccountIdByDebtorId(debtor.Debtor_Id)
                        });

                        creditor.Finapp_Balance -= debtor.Finapp_Debet;
                        _creditorService.ModifyCreditor(creditor);

                        debtor.Finapp_Debet = 0;
                        debtor.Available = false;
                        _debtorService.ModifyDebtor(debtor);

                        break;
                    }
                    else
                    {
                        _transactionOutService.AddTransaction(new Transaction_Out
                        {
                            Ammount = creditor.Finapp_Balance,
                            Date_Of_Transaction = DateTime.Now,
                            Creditor_Account_Id = _creditorAccountService.GetAccountIdByCreditorId(creditor.Creditor_Id),
                            Debtor_Account_Id = _debtorAccountService.GetAccountIdByDebtorId(debtor.Debtor_Id)
                        });

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

            return true;
        }

        private IEnumerable<Debtor> AddDebtorsToQueue()
        {
            var debtorsList = _debtorService.GetAvailableDebtors();

            if (debtorsList == null)
                return null;

            debtorsList.OrderBy(d => d.Queue_Date);

            return debtorsList;
        }

        private IEnumerable<Creditor> AddCreditorsToQueue()
        {
            var creditorsList = _creditorService.GetAvailableCreditors();

            if (creditorsList == null)
                return null;

            creditorsList.OrderBy(d => d.Queue_Date);

            return creditorsList;
        }

    }
}