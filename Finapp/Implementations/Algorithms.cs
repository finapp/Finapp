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
        private readonly FinapEntities1 _context;
        private readonly ICreditorService _creditorService;
        private readonly IDebtorService _debtorService;
        private readonly ITransactionOutService _transactionOutService;
        private readonly ICreditorAccountService _creditorAccountService;
        private readonly IDebtorAccountService _debtorAccountService;
        private readonly IReturnTransactionService _returnTransactionService;

        public Algorithms(FinapEntities1 context, ICreditorService creditorService, IDebtorService debtorService,
            ITransactionOutService transactionOutService, ICreditorAccountService creditorAccountService,
            IDebtorAccountService debtorAccountService, IReturnTransactionService returnTransactionService)
        {
            _context = context;
            _creditorService = creditorService;
            _debtorService = debtorService;
            _transactionOutService = transactionOutService;
            _creditorAccountService = creditorAccountService;
            _debtorAccountService = debtorAccountService;
            _returnTransactionService = returnTransactionService;
        }

        public bool Associating()
        {
            IEnumerable<Debtor> debtors = AddDebtorsToQueue();

            if (debtors == null)
                return false;

            foreach (var debtor in debtors)
            {
                IEnumerable<Creditor> creditors = AddCreditorsToQueue(debtor.EAPR??0);
                CreateTransaction(debtor, creditors);
            }

            return true;
        }

        private bool CreateTransaction(Debtor debtor, IEnumerable<Creditor> creditors)
        {
            var EROI = creditors.Max(e=>e.EROI);

            foreach (var creditor in creditors)
            {
                if (creditor.Finapp_Balance > debtor.Finapp_Debet)
                {
                    var t_out = new Transaction_Out
                    {
                        Ammount = debtor.Finapp_Debet,
                        Date_Of_Transaction = DateTime.Now,
                        Creditor_Account_Id = _creditorAccountService.GetAccountIdByCreditorId(creditor.Creditor_Id),
                        Debtor_Account_Id = _debtorAccountService.GetAccountIdByDebtorId(debtor.Debtor_Id),
                        ROI = (float)EROI,
                        Finapp_Debetor = 0,
                        Finapp_Creditor = creditor.Finapp_Balance - debtor.Finapp_Debet,
                        Day_Access_To_Funds = creditor.Expiration_Date.Value.Subtract(DateTime.Now).Days,
                        Creditor_Benefits_Per_Annum = (int)(debtor.Finapp_Debet * ((float)((creditor.EROI - creditor.ROI)/100))),
                        Debtor_Benefits_Per_Annum = (int)(debtor.Finapp_Debet * (float)((debtor.APR - debtor.EAPR)/100))
                    };

                    _transactionOutService.AddTransaction(t_out);
                    // _returnTransactionService.AddReturnTransaction(t_out);

                    creditor.Finapp_Balance -= debtor.Finapp_Debet;
                    _creditorService.ModifyCreditor(creditor);

                    debtor.Finapp_Debet = 0;
                    debtor.Available = false;
                    _debtorService.ModifyDebtor(debtor);

                    break;
                }
                else
                {
                    var t_out = new Transaction_Out
                    {
                        Ammount = creditor.Finapp_Balance,
                        Date_Of_Transaction = DateTime.Now,
                        Creditor_Account_Id = _creditorAccountService.GetAccountIdByCreditorId(creditor.Creditor_Id),
                        Debtor_Account_Id = _debtorAccountService.GetAccountIdByDebtorId(debtor.Debtor_Id),
                        ROI = (float)EROI,
                        Finapp_Debetor = debtor.Finapp_Debet - creditor.Finapp_Balance,
                        Finapp_Creditor = 0,
                        Day_Access_To_Funds = creditor.Expiration_Date.Value.Subtract(DateTime.Now).Days,
                        Creditor_Benefits_Per_Annum = (int)(debtor.Finapp_Debet * ((float)((creditor.EROI - creditor.ROI) / 100))),
                        Debtor_Benefits_Per_Annum = (int)(debtor.Finapp_Debet * (float)((debtor.APR - debtor.EAPR) / 100))
                    };

                    _transactionOutService.AddTransaction(t_out);
                    //_returnTransactionService.AddReturnTransaction(t_out);

                    debtor.Finapp_Debet -= creditor.Finapp_Balance;
                    _debtorService.ModifyDebtor(debtor);

                    creditor.Finapp_Balance = 0;
                    creditor.Available = false;
                    _creditorService.ModifyCreditor(creditor);

                    if (debtor.Finapp_Debet == 0)
                        break;
                }
            }
            return true;
        }

        private IEnumerable<Debtor> AddDebtorsToQueue()
        {
            var debtorsList = _debtorService.GetAvailableDebtors();

            if (debtorsList == null)
                return null;

            return debtorsList;
        }

        private IEnumerable<Creditor> AddCreditorsToQueue(float eapr)
        {
            var creditorsList = _creditorService.GetAvailableCreditors(eapr);

            if (creditorsList == null)
                return null;

            return creditorsList;
        }
    }
}