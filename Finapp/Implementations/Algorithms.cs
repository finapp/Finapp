using Finapp.Interfaces;
using Finapp.IServices;
using Finapp.Models;
using Finapp.Services;
using Finapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.Implementations
{
    public class Algorithms : IAlgorithms
    {
        private readonly ICreditorService _creditorService;
        private readonly IDebtorService _debtorService;
        private readonly ITransactionOutService _transactionOutService;
        private readonly ICreditorAccountService _creditorAccountService;
        private readonly IDebtorAccountService _debtorAccountService;
        private readonly IAssociateService _associateService;
        private readonly ISummaryService _summaryService;
        private SummaryModel _summary;

        public Algorithms(ICreditorService creditorService, IDebtorService debtorService,
            ITransactionOutService transactionOutService, ICreditorAccountService creditorAccountService,
            IDebtorAccountService debtorAccountService, IAssociateService associateService,
            ISummaryService summaryService)
        {
            _creditorService = creditorService;
            _debtorService = debtorService;
            _transactionOutService = transactionOutService;
            _creditorAccountService = creditorAccountService;
            _debtorAccountService = debtorAccountService;
            _associateService = associateService;
            _summaryService = summaryService;
            _summary = new SummaryModel();
        }

        public bool Associating()
        {
            IEnumerable<Debtor> debtors = AddDebtorsToQueue();
            IEnumerable<Creditor> creditor = AddCreditorsToQueue(34);

            if (debtors == null)
                return false;

            if (creditor.Count() == 0)
                return false;

            var associate = new Associate
            {
                Date_Of_Associating = DateTime.Now
            };
            _associateService.AddNewAssociate(associate);

            foreach (var cred in creditor)
            {
                _creditorService.AddAssociate(associate, cred);
                _associateService.AddCreditor(associate, cred);
            }

            foreach (var deb in debtors)
            {
                _debtorService.AddAssociate(associate, deb);
                _associateService.AddDebtor(associate, deb);
            }

            var sumOfDebets = debtors.Sum(d => d.Finapp_Debet);
            var sumOfBalance = creditor.Sum(d => d.Finapp_Balance);
            var avgOfDebet = sumOfDebets / debtors.Count();
            var avgOfBalance = sumOfBalance / creditor.Count();

            foreach (var debtor in debtors)
            {
                IEnumerable<Creditor> creditors = AddCreditorsToQueue(debtor.EAPR ?? 0);

                if (creditor.Count() > 0)
                {
                    _summary.CounterOfDebtors++;
                    CreateTransaction(debtor, creditors, associate);
                }
            }
            var avgOfSavings = _summary.SavingsSum / debtors.Count();
            var avgOfProfits = _summary.ProfitsSum / creditor.Count();
            var avgOfSavingsPercentage = _summary.SavingsAveragePercentage / _summary.CounterOdCreditors;
            var avgOfProfitsPercentage = _summary.ProfitsAveragePercentage / _summary.CounterOdCreditors;
            var days = _summary.Days;

            var summary = new Summary
            {
                Associate_Id = associate.Associate_Id,
                Debtors = debtors.Count(),
                Creditors = creditor.Count(),
                Debet_Sum = sumOfDebets,
                Balance_Sum = sumOfBalance,
                Debet_Average = avgOfDebet,
                Balance_Average = avgOfBalance,
                Savings_Sum = _summary.SavingsSum,
                Profits_Sum = _summary.ProfitsSum,
                Savings_Average = avgOfSavings,
                Profits_Average = avgOfProfits,
                Savings_Average_Percentage = avgOfSavingsPercentage,
                Profits_Average_Percentage = avgOfProfitsPercentage,
                Days = days / _summary.CounterOdCreditors,
                DateOfSummary = DateTime.Now
            };
            _summaryService.CreateSummary(summary);

            return true;
        }

        private bool CreateTransaction(Debtor debtor, IEnumerable<Creditor> creditors, Associate associate)
        {
            var EROI = creditors.Max(e => e.EROI);

            foreach (var creditor in creditors)
            {
                if (creditor.Finapp_Balance > debtor.Finapp_Debet)
                {
                    var debtorBenefitsPerAnnum = (int)((float)(((debtor.APR - debtor.EAPR) / 100) * debtor.Finapp_Debet));
                    var creditorBenefitsPerAnnum = (int)((float)((EROI / 100) * debtor.Finapp_Debet));
                    var transactionOut = new Transaction_Out
                    {
                        Ammount = debtor.Finapp_Debet,
                        Date_Of_Transaction = DateTime.Now,
                        Creditor_Account_Id = _creditorAccountService.GetAccountIdByCreditorId(creditor.Creditor_Id),
                        Debtor_Account_Id = _debtorAccountService.GetAccountIdByDebtorId(debtor.Debtor_Id),
                        ROI = (float)EROI,
                        Finapp_Debetor = 0,
                        Finapp_Creditor = creditor.Finapp_Balance - debtor.Finapp_Debet,
                        Day_Access_To_Funds = creditor.Expiration_Date.Value.Subtract(DateTime.Now).Days,
                        Creditor_Benefits_Per_Annum = creditorBenefitsPerAnnum,
                        Debtor_Benefits_Per_Annum = debtorBenefitsPerAnnum,
                        Associate_Id = associate.Associate_Id,
                        Creditor_Id = creditor.Creditor_Id,
                        Debtor_Id = debtor.Debtor_Id
                    };

                    _summary.SavingsSum += CountAllSavings(creditor, debtorBenefitsPerAnnum);
                    _summary.ProfitsSum += CountAllBalance(creditor, creditorBenefitsPerAnnum);
                    _summary.ProfitsAveragePercentage += (int)EROI;
                    _summary.SavingsAveragePercentage += (int)(debtor.APR - debtor.EAPR);
                    _summary.CounterOdCreditors++;
                    _summary.Days += creditor.Expiration_Date.Value.Subtract(DateTime.Now).Days;

                    var date = _creditorService.GetTheOldestQueueDate().AddDays(1);
                    creditor.Queue_Date = date;
                    _creditorService.ModifyCreditor(creditor);

                    date = _debtorService.GetTheOldestQueueDate().AddDays(1);
                    debtor.Queue_Date = date;
                    _debtorService.ModifyDebtor(debtor);

                    _transactionOutService.AddTransaction(transactionOut);

                    creditor.Finapp_Balance -= debtor.Finapp_Debet;
                    _creditorService.ModifyCreditor(creditor);
                    debtor.Finapp_Debet = 0;
                    debtor.Available = false;
                    _debtorService.ModifyDebtor(debtor);

                    break;
                }
                else
                {
                    var debtorBenefitsPerAnnum = (int)((float)(((debtor.APR - debtor.EAPR) / 100) * creditor.Finapp_Balance));
                    var creditorBenefitsPerAnnum = (int)((float)((EROI / 100) * creditor.Finapp_Balance));
                    var transactionOut = new Transaction_Out
                    {
                        Ammount = creditor.Finapp_Balance,
                        Date_Of_Transaction = DateTime.Now,
                        Creditor_Account_Id = _creditorAccountService.GetAccountIdByCreditorId(creditor.Creditor_Id),
                        Debtor_Account_Id = _debtorAccountService.GetAccountIdByDebtorId(debtor.Debtor_Id),
                        ROI = (float)EROI,
                        Finapp_Debetor = debtor.Finapp_Debet - creditor.Finapp_Balance,
                        Finapp_Creditor = 0,
                        Day_Access_To_Funds = creditor.Expiration_Date.Value.Subtract(DateTime.Now).Days,
                        Creditor_Benefits_Per_Annum = creditorBenefitsPerAnnum,
                        Debtor_Benefits_Per_Annum = debtorBenefitsPerAnnum,
                        Associate_Id = associate.Associate_Id,
                        Creditor_Id = creditor.Creditor_Id,
                        Debtor_Id=debtor.Debtor_Id
                    };

                    _summary.SavingsSum += CountAllSavings(creditor, debtorBenefitsPerAnnum);
                    _summary.ProfitsSum += CountAllBalance(creditor, creditorBenefitsPerAnnum);
                    _summary.ProfitsAveragePercentage += (int)EROI;
                    _summary.SavingsAveragePercentage += (int)(debtor.APR - debtor.EAPR);
                    _summary.CounterOdCreditors++;
                    _summary.Days += creditor.Expiration_Date.Value.Subtract(DateTime.Now).Days;

                    var date = _creditorService.GetTheOldestQueueDate().AddDays(1);
                    creditor.Queue_Date = date;
                    _creditorService.ModifyCreditor(creditor);

                    date = _debtorService.GetTheOldestQueueDate().AddDays(1);
                    debtor.Queue_Date = date;
                    _debtorService.ModifyDebtor(debtor);

                    _transactionOutService.AddTransaction(transactionOut);

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

        private int CountAllSavings(Creditor creditor, int sum)
        {
            return (int)(sum * ((float)creditor.Expiration_Date.Value.Subtract(DateTime.Now).Days / 365));
        }

        private int CountAllBalance(Creditor creditor, int sum)
        {
            return (int)(sum * ((float)creditor.Expiration_Date.Value.Subtract(DateTime.Now).Days) / 365);
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