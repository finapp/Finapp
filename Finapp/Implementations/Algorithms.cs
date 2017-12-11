﻿using Finapp.Interfaces;
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
        private readonly IAssociateService _associateService;
        private readonly ISummaryService _summaryService;
        private SummaryModel _summary;
        private readonly FinapEntities1 _context;

        public Algorithms(ICreditorService creditorService, IDebtorService debtorService, ITransactionOutService transactionOutService, 
            IAssociateService associateService, ISummaryService summaryService, SummaryModel summary, FinapEntities1 context)
        {
            _creditorService = creditorService;
            _debtorService = debtorService;
            _transactionOutService = transactionOutService;
            _associateService = associateService;
            _summaryService = summaryService;
            _summary = summary;
            _context = context;
        }

        public bool DoAssociation(Associate associate)
        {

            var dateCreditor = _creditorService.GetTheOldestQueueDate();
            var dateDebtor = _debtorService.GetTheOldestQueueDate();

            IEnumerable<Creditor> creditors = _context.Creditor.Where(c=>c.Available==true).ToList();
            IEnumerable<Debtor> debtors = _context.Debtor.Where(d=>d.Available==true).ToList();

            debtors = debtors.OrderBy(d => d.Queue_Date);

            foreach (var debtor in debtors)
            {
                var creds = (from c in creditors
                             where c.EROI < debtor.EAPR
                             select c.EROI)
                             .ToList();

                var EROI = creds.Max();

                creditors = creditors.OrderBy(c => c.Queue_Date);

                foreach (var cred in creditors)
                {
                    if (cred.Finapp_Balance > debtor.Finapp_Debet && cred.EROI<debtor.EAPR && cred.Available && debtor.Available)
                    {
                        var debtorBenefitsPerAnnum = (int)((float)(((debtor.APR - debtor.EAPR) / 100) * debtor.Finapp_Debet));
                        var creditorBenefitsPerAnnum = (int)((float)((EROI / 100) * debtor.Finapp_Debet));
                        var days = cred.Expiration_Date.Value.Subtract(DateTime.Now).Days;
                        var realCreditorBenefits = ((int)((float)days / 365 * (debtor.Finapp_Debet * ((float)cred.EROI / 100))));
                        var actualCreditorBenefits = ((int)((float)days / 365 * (debtor.Finapp_Debet * ((float)cred.ROI / 100))));
                        var debtorSavings = ((int)((float)days / 365 * (debtor.Finapp_Debet * ((float)(debtor.APR - debtor.EAPR) / 100))));

                        var transactionOut = new Transaction_Out
                        {
                            Ammount = debtor.Finapp_Debet,
                            Date_Of_Transaction = DateTime.Now,
                            ROI = (float)EROI,
                            Finapp_Debetor = 0,
                            Finapp_Creditor = cred.Finapp_Balance - debtor.Finapp_Debet,
                            Day_Access_To_Funds = cred.Expiration_Date.Value.Subtract(DateTime.Now).Days,
                            Creditor_Benefits_Per_Annum = creditorBenefitsPerAnnum,
                            Debtor_Benefits_Per_Annum = debtorBenefitsPerAnnum,
                            Associate_Id = associate.Associate_Id,
                            Creditor_Id = cred.Creditor_Id,
                            Debtor_Id = debtor.Debtor_Id,
                            ActualCreditorBenefits = actualCreditorBenefits,
                            CreditorBenefits = realCreditorBenefits,
                            DebtorSavings = debtorSavings
                        };

                        _summary.SavingsSum += CountAllSavings(cred, debtorBenefitsPerAnnum);
                        _summary.ProfitsSum += CountAllBalance(cred, creditorBenefitsPerAnnum);
                        _summary.ProfitsAveragePercentage += (int)EROI;
                        _summary.SavingsAveragePercentage += (int)(debtor.APR - debtor.EAPR);
                        _summary.CounterOdCreditors++;
                        _summary.Days += cred.Expiration_Date.Value.Subtract(DateTime.Now).Days * debtor.Finapp_Debet;
                        _summary.Turnover += debtor.Finapp_Debet;

                        dateCreditor = dateCreditor.AddDays(1);

                        if (cred.LastAssociate < associate.Associate_Id)
                        {
                            cred.AssociateCounter += 1;
                            cred.LastAssociate = associate.Associate_Id;
                        }

                        cred.Queue_Date = dateCreditor;
                        //_creditorService.ModifyCreditor(cred);

                        dateDebtor = dateDebtor.AddDays(1);

                        if (debtor.LastAssociate < associate.Associate_Id)
                        {
                            debtor.AssociateCounter += 1;
                            debtor.LastAssociate = associate.Associate_Id;
                        }

                        debtor.Queue_Date = dateDebtor;
                        //_debtorService.ModifyDebtor(debtor);

                        _transactionOutService.AddTransaction(transactionOut);

                        cred.Finapp_Balance -= debtor.Finapp_Debet;
                        //_creditorService.ModifyCreditor(cred);
                        debtor.Finapp_Debet = 0;
                        debtor.Available = false;
                        //_debtorService.ModifyDebtor(debtor);

                        break;
                    }
                    else if (cred.EROI < debtor.EAPR && cred.Available && debtor.Available)
                    {
                        var debtorBenefitsPerAnnum = (int)((float)(((debtor.APR - debtor.EAPR) / 100) * cred.Finapp_Balance));
                        var creditorBenefitsPerAnnum = (int)((float)((EROI / 100) * cred.Finapp_Balance));
                        var days = cred.Expiration_Date.Value.Subtract(DateTime.Now).Days;
                        var realCreditorBenefits = ((int)((float)days / 365 * (cred.Finapp_Balance * ((float)cred.EROI / 100))));
                        var actualCreditorBenefits = ((int)((float)days / 365 * (cred.Finapp_Balance * ((float)cred.ROI / 100))));
                        var debtorSavings = ((int)((float)days / 365 * (cred.Finapp_Balance * ((float)(debtor.APR - debtor.EAPR) / 100))));

                        var transactionOut = new Transaction_Out
                        {
                            Ammount = cred.Finapp_Balance,
                            Date_Of_Transaction = DateTime.Now,
                            ROI = (float)EROI,
                            Finapp_Debetor = debtor.Finapp_Debet - cred.Finapp_Balance,
                            Finapp_Creditor = 0,
                            Day_Access_To_Funds = cred.Expiration_Date.Value.Subtract(DateTime.Now).Days,
                            Creditor_Benefits_Per_Annum = creditorBenefitsPerAnnum,
                            Debtor_Benefits_Per_Annum = debtorBenefitsPerAnnum,
                            Associate_Id = associate.Associate_Id,
                            Creditor_Id = cred.Creditor_Id,
                            Debtor_Id = debtor.Debtor_Id,
                            ActualCreditorBenefits = actualCreditorBenefits,
                            CreditorBenefits = realCreditorBenefits,
                            DebtorSavings = debtorSavings
                        };

                        _summary.SavingsSum += CountAllSavings(cred, debtorBenefitsPerAnnum);
                        _summary.ProfitsSum += CountAllBalance(cred, creditorBenefitsPerAnnum);
                        _summary.ProfitsAveragePercentage += (int)EROI;
                        _summary.SavingsAveragePercentage += (int)(debtor.APR - debtor.EAPR);
                        _summary.CounterOdCreditors++;
                        _summary.Days += cred.Expiration_Date.Value.Subtract(DateTime.Now).Days * cred.Finapp_Balance;
                        _summary.Turnover += cred.Finapp_Balance;

                        dateCreditor = dateCreditor.AddDays(1);

                        if (cred.LastAssociate < associate.Associate_Id)
                        {
                            cred.AssociateCounter += 1;
                            cred.LastAssociate = associate.Associate_Id;
                        }

                        cred.Queue_Date = dateCreditor;
                        //_creditorService.ModifyCreditor(cred);

                        dateDebtor = dateDebtor.AddDays(1);

                        if (debtor.LastAssociate < associate.Associate_Id)
                        {
                            debtor.AssociateCounter += 1;
                            debtor.LastAssociate = associate.Associate_Id;
                        }

                        debtor.Queue_Date = dateDebtor;
                        //_debtorService.ModifyDebtor(debtor);

                        _transactionOutService.AddTransaction(transactionOut);

                        debtor.Finapp_Debet -= cred.Finapp_Balance;
                        //_debtorService.ModifyDebtor(debtor);

                        cred.Finapp_Balance = 0;
                        cred.Available = false;
                       // _creditorService.ModifyCreditor(cred);

                        if (debtor.Finapp_Debet == 0)
                            break;
                    }
                }
            }

            foreach (var debtor in debtors)
            {
                _debtorService.ModifyDebtor(debtor);
            }
            foreach (var cred in creditors)
            {
                _creditorService.ModifyCreditor(cred);
            }


            return true;

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

            //foreach (var cred in creditor)
            //{
            //    _creditorService.AddAssociate(associate, cred);
            //    _associateService.AddCreditor(associate, cred);
            //}

            //foreach (var deb in debtors)
            //{
            //    _debtorService.AddAssociate(associate, deb);
            //    _associateService.AddDebtor(associate, deb);
            //}

            //var sumOfDebets = debtors.Sum(d => d.Finapp_Debet);
            //var sumOfBalance = creditor.Sum(d => d.Finapp_Balance);
            //var avgOfDebet = sumOfDebets / debtors.Count();
            //var avgOfBalance = sumOfBalance / creditor.Count();

            //foreach (var debtor in debtors)
            //{
            //    IEnumerable<Creditor> creditors = AddCreditorsToQueue(debtor.EAPR ?? 0);

            //    if (creditor.Count() > 0)
            //    {
            //        _summary.CounterOfDebtors++;
            //        CreateTransaction(debtor, creditors, associate);
            //    }
            //}


            DoAssociation(associate);

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
                //Debet_Sum = sumOfDebets,
                //Balance_Sum = sumOfBalance,
                //Debet_Average = avgOfDebet,
                //Balance_Average = avgOfBalance,
                Savings_Sum = _summary.SavingsSum,
                Profits_Sum = _summary.ProfitsSum,
                Savings_Average = avgOfSavings,
                Profits_Average = avgOfProfits,
                Savings_Average_Percentage = avgOfSavingsPercentage,
                Profits_Average_Percentage = avgOfProfitsPercentage,
                Days = days / _summary.Turnover,
                DateOfSummary = DateTime.Now,
                Turnover = _summary.Turnover
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
                    var days = creditor.Expiration_Date.Value.Subtract(DateTime.Now).Days;
                    var realCreditorBenefits = ((int)((float)days / 365 * (debtor.Finapp_Debet * ((float)creditor.EROI / 100))));
                    var actualCreditorBenefits = ((int)((float)days / 365 * (debtor.Finapp_Debet * ((float)creditor.ROI / 100))));
                    var debtorSavings = ((int)((float)days / 365 * (debtor.Finapp_Debet * ((float)(debtor.APR - debtor.EAPR) / 100))));

                    var transactionOut = new Transaction_Out
                    {
                        Ammount = debtor.Finapp_Debet,
                        Date_Of_Transaction = DateTime.Now,
                        ROI = (float)EROI,
                        Finapp_Debetor = 0,
                        Finapp_Creditor = creditor.Finapp_Balance - debtor.Finapp_Debet,
                        Day_Access_To_Funds = creditor.Expiration_Date.Value.Subtract(DateTime.Now).Days,
                        Creditor_Benefits_Per_Annum = creditorBenefitsPerAnnum,
                        Debtor_Benefits_Per_Annum = debtorBenefitsPerAnnum,
                        Associate_Id = associate.Associate_Id,
                        Creditor_Id = creditor.Creditor_Id,
                        Debtor_Id = debtor.Debtor_Id,
                        ActualCreditorBenefits = actualCreditorBenefits,
                        CreditorBenefits = realCreditorBenefits,
                        DebtorSavings = debtorSavings
                    };

                    //_summary.SavingsSum += CountAllSavings(creditor, debtorBenefitsPerAnnum);
                    //_summary.ProfitsSum += CountAllBalance(creditor, creditorBenefitsPerAnnum);
                    _summary.ProfitsAveragePercentage += (int)EROI;
                    _summary.SavingsAveragePercentage += (int)(debtor.APR - debtor.EAPR);
                    _summary.CounterOdCreditors++;
                    _summary.Days += creditor.Expiration_Date.Value.Subtract(DateTime.Now).Days*debtor.Finapp_Debet;
                    _summary.Turnover += debtor.Finapp_Debet;

                    var date = _creditorService.GetTheOldestQueueDate().AddDays(1);

                    if (creditor.LastAssociate < associate.Associate_Id)
                    {
                        creditor.AssociateCounter += 1;
                        creditor.LastAssociate = associate.Associate_Id;
                    }

                    creditor.Queue_Date = date;
                    _creditorService.ModifyCreditor(creditor);

                    date = _debtorService.GetTheOldestQueueDate().AddDays(1);

                    if (debtor.LastAssociate < associate.Associate_Id)
                    {
                        debtor.AssociateCounter += 1;
                        debtor.LastAssociate = associate.Associate_Id;
                    }

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
                    var days = creditor.Expiration_Date.Value.Subtract(DateTime.Now).Days;
                    var realCreditorBenefits = ((int)((float)days / 365 * (creditor.Finapp_Balance * ((float)creditor.EROI / 100))));
                    var actualCreditorBenefits = ((int)((float)days / 365 * (creditor.Finapp_Balance * ((float)creditor.ROI / 100))));
                    var debtorSavings = ((int)((float)days / 365 * (creditor.Finapp_Balance * ((float)(debtor.APR - debtor.EAPR) / 100))));

                    var transactionOut = new Transaction_Out
                    {
                        Ammount = creditor.Finapp_Balance,
                        Date_Of_Transaction = DateTime.Now,
                        ROI = (float)EROI,
                        Finapp_Debetor = debtor.Finapp_Debet - creditor.Finapp_Balance,
                        Finapp_Creditor = 0,
                        Day_Access_To_Funds = creditor.Expiration_Date.Value.Subtract(DateTime.Now).Days,
                        Creditor_Benefits_Per_Annum = creditorBenefitsPerAnnum,
                        Debtor_Benefits_Per_Annum = debtorBenefitsPerAnnum,
                        Associate_Id = associate.Associate_Id,
                        Creditor_Id = creditor.Creditor_Id,
                        Debtor_Id = debtor.Debtor_Id,
                        ActualCreditorBenefits = actualCreditorBenefits,
                        CreditorBenefits = realCreditorBenefits,
                        DebtorSavings = debtorSavings
                    };

                    //_summary.SavingsSum += CountAllSavings(creditor, debtorBenefitsPerAnnum);
                    //_summary.ProfitsSum += CountAllBalance(creditor, creditorBenefitsPerAnnum);
                    _summary.ProfitsAveragePercentage += (int)EROI;
                    _summary.SavingsAveragePercentage += (int)(debtor.APR - debtor.EAPR);
                    _summary.CounterOdCreditors++;
                    _summary.Days += creditor.Expiration_Date.Value.Subtract(DateTime.Now).Days * creditor.Finapp_Balance;
                    _summary.Turnover += creditor.Finapp_Balance;

                    var date = _creditorService.GetTheOldestQueueDate().AddDays(1);

                    if (creditor.LastAssociate < associate.Associate_Id)
                    {
                        creditor.AssociateCounter += 1;
                        creditor.LastAssociate = associate.Associate_Id;
                    }

                    creditor.Queue_Date = date;
                    _creditorService.ModifyCreditor(creditor);

                    date = _debtorService.GetTheOldestQueueDate().AddDays(1);

                    if (debtor.LastAssociate < associate.Associate_Id)
                    {
                        debtor.AssociateCounter += 1;
                        debtor.LastAssociate = associate.Associate_Id;
                    }

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