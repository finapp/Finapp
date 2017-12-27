using AutoMapper;
using Finapp.Interfaces;
using Finapp.IServices;
using Finapp.Models;
using Finapp.Services;
using Finapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        private FinapEntities1 _context;

        private Stopwatch stopWatch;
        private Stopwatch EROIstopWatch;

        private string getDebtorsTime;
        private string getCreditorsTime;
        private string getROITime;
        private string getAssociationsTime;
        private string getTransactionTime;
        private string getDbDebtorsTime;
        private string getDbCreditorsTime;
        private string getDbTransactionsTime;

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

            stopWatch = new Stopwatch();
            EROIstopWatch = new Stopwatch();
        }

        public bool DoAssociation(Associate associate)
        {
            var dateCreditor = _creditorService.GetTheOldestQueueDate();
            var dateDebtor = _debtorService.GetTheOldestQueueDate();

            stopWatch.Start();
            IEnumerable<Creditor> creditors = _context.Creditor.Where(c => c.Available == true).ToList();
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            getDebtorsTime = String.Format("{0:00}:{1:00}:{2:00}:{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            stopWatch.Reset();
            stopWatch.Start();

            IEnumerable<Debtor> debtors = _context.Debtor.Where(d => d.Available == true).ToList();
            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            getCreditorsTime = String.Format("{0:00}:{1:00}:{2:00}:{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            debtors = debtors.OrderBy(d => d.Queue_Date);

            stopWatch.Reset();
            stopWatch.Start();

            var transactionsList = new List<Transaction_Out>();
            creditors = AddTrialsToCreditors(creditors);
            //  creditors = Mapper.Map<IEnumerable<Creditor>>(creditors);

            // debtors = Mapper.Map<IEnumerable<Debtor>>(debtors);

            foreach (var debtor in debtors)
            {

                debtor.Trials += 1;

                EROIstopWatch.Start();
                //   var creds = (from c in creditors
                //                where c.EROI < debtor.EAPR && c.Available
                //                select c.EROI)
                //                .ToList();
                ////   creditors = Mapper.Map<IEnumerable<Creditor>>(creditors);

                creditors = creditors.OrderBy(c => c.Queue_Date);
                //  creditors = Mapper.Map<IEnumerable<Creditor>>(creditors);
                IEnumerable<Creditor> creds = creditors.Where(c => c.EROI < debtor.EAPR && c.Available).ToList();
                var EROI = creds.Max(c => c.EROI);
                EROIstopWatch.Stop();

                foreach (var cred in creds)
                {
                    if (cred.Finapp_Balance > debtor.Finapp_Debet && debtor.Available && cred.Available)
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
                            DebtorSavings = debtorSavings,
                            AssociateDay = associate.Nr
                        };

                        transactionsList.Add(transactionOut);

                        cred.Profits += realCreditorBenefits;
                        debtor.Savings += debtorSavings;

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

                        dateDebtor = dateDebtor.AddDays(1);

                        if (debtor.LastAssociate < associate.Associate_Id)
                        {
                            debtor.AssociateCounter += 1;
                            debtor.LastAssociate = associate.Associate_Id;
                        }

                        debtor.Queue_Date = dateDebtor;

                        cred.Finapp_Balance -= debtor.Finapp_Debet;

                        debtor.Finapp_Debet = 0;
                        debtor.Available = false;

                        break;
                    }
                    else if (cred.EROI < debtor.EAPR && debtor.Available && cred.Available)
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
                            DebtorSavings = debtorSavings,
                            AssociateDay = associate.Nr
                        };

                        transactionsList.Add(transactionOut);

                        cred.Profits += realCreditorBenefits;
                        debtor.Savings += debtorSavings;

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

                        dateDebtor = dateDebtor.AddDays(1);

                        if (debtor.LastAssociate < associate.Associate_Id)
                        {
                            debtor.AssociateCounter += 1;
                            debtor.LastAssociate = associate.Associate_Id;
                        }

                        debtor.Queue_Date = dateDebtor;

                        debtor.Finapp_Debet -= cred.Finapp_Balance;

                        cred.Finapp_Balance = 0;
                        cred.Available = false;

                        if (debtor.Finapp_Debet == 0)
                            break;
                    }
                }
            }
            stopWatch.Stop();

            TimeSpan tsForTransaction = stopWatch.Elapsed;

            TimeSpan t = new TimeSpan(tsForTransaction.Ticks);
            if (transactionsList.Count() != 0)
            {
                t = new TimeSpan(tsForTransaction.Ticks / transactionsList.Count());
            }
            getTransactionTime = String.Format("{0:00}:{1:00}:{2:00}:{3:00}",
           t.Hours, t.Minutes, t.Seconds, t.Milliseconds / 10);

            var divideCounter = 1;
            if (debtors.Count() != 0)
            {
                divideCounter = debtors.Count();
            }

            TimeSpan tsForEROI = EROIstopWatch.Elapsed;
            t = new TimeSpan(tsForEROI.Ticks / divideCounter);
            getROITime = String.Format("{0:00}:{1:00}:{2:00}:{3:00}",
           t.Hours, t.Minutes, t.Seconds, t.Milliseconds / 10);

            ts = stopWatch.Elapsed;
            getAssociationsTime = String.Format("{0:00}:{1:00}:{2:00}:{3:00}",
           ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            stopWatch.Reset();
            stopWatch.Start();

            _transactionOutService.AddTransactions(transactionsList);
            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            getDbTransactionsTime = String.Format("{0:00}:{1:00}:{2:00}:{3:00}",
           ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            stopWatch.Reset();
            stopWatch.Start();
            IEnumerable<Debtor> updateDebtors = Mapper.Map<IEnumerable<Debtor>>(debtors);

            _debtorService.ModifyDebtors(updateDebtors);

            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            getDbDebtorsTime = String.Format("{0:00}:{1:00}:{2:00}:{3:00}",
           ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            stopWatch.Reset();
            stopWatch.Start();

            IEnumerable<Creditor> updateCreditors = Mapper.Map<IEnumerable<Creditor>>(creditors);

            _creditorService.ModifyCreditors(updateCreditors);

            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            getDbCreditorsTime = String.Format("{0:00}:{1:00}:{2:00}:{3:00}",
           ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            _context = new FinapEntities1();
            _context.Times.Add(new Times
            {
                AssociateTime = getAssociationsTime,
                CountOfCreditors = creditors.Count(),
                CountOfDebtors = debtors.Count(),
                CountOfTransactions = transactionsList.Count(),
                GetCreditorsTime = getCreditorsTime,
                GetDebtorsTime = getDebtorsTime,
                TimeForOneTransaction = getTransactionTime,
                UpdateCreditorsTime = getDbCreditorsTime,
                UpdateDebtorsTime = getDbDebtorsTime,
                UpdateTransactionsTime = getDbTransactionsTime,
                AllCreditors = _context.Creditor.Count(),
                AllDebtors = _context.Debtor.Count(),
                SetROI = getROITime
            });
            _context.SaveChanges();

            return true;
        }

        public bool Associating()
        {
            var associationsCounter = (from a in _context.Associate
                                       select a.Nr)
                                .Count();

            var associate = new Associate
            {
                Date_Of_Associating = DateTime.Now,
                Nr = associationsCounter + 1
            };
            _associateService.AddNewAssociate(associate);

            if (associate.Nr > 1)
            {
                UpdateCreditors(associate.Nr ?? 0);
                UpdateDebtors(associate.Nr ?? 0);
            }
            IEnumerable<Debtor> debtors = AddDebtorsToQueue();
            IEnumerable<Creditor> creditor = AddCreditorsToQueue(34);

            //if (debtors == null)
            //    return false;

            //if (creditor.Count() == 0)
            //    return false;



            DoAssociation(associate);

            var avgOfSavings = 0;
            var avgOfProfits = 0;
            var avgOfSavingsPercentage = 0;
            var avgOfProfitsPercentage = 0;
            var days = 0;

            if (debtors.Count() != 0 && creditor.Count() != 0 && _summary.CounterOdCreditors != 0)
            {
                avgOfSavings = _summary.SavingsSum / debtors.Count();
                avgOfProfits = _summary.ProfitsSum / creditor.Count();
                avgOfSavingsPercentage = _summary.SavingsAveragePercentage / _summary.CounterOdCreditors;
                avgOfProfitsPercentage = _summary.ProfitsAveragePercentage / _summary.CounterOdCreditors;
                days = _summary.Days;
            }

            int avgOfDays = 0;

            if (_summary.Turnover != 0)
            {
                avgOfDays = days / _summary.Turnover;
            }

            var summary = new Summary
            {
                Associate_Id = associate.Associate_Id,
                Debtors = debtors.Count(),
                Creditors = creditor.Count(),
                Savings_Sum = _summary.SavingsSum,
                Profits_Sum = _summary.ProfitsSum,
                Savings_Average = avgOfSavings,
                Profits_Average = avgOfProfits,
                Savings_Average_Percentage = avgOfSavingsPercentage,
                Profits_Average_Percentage = avgOfProfitsPercentage,
                Days = avgOfDays,
                DateOfSummary = DateTime.Now,
                Turnover = _summary.Turnover
            };
            _summaryService.CreateSummary(summary);

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

        private IEnumerable<Creditor> AddTrialsToCreditors(IEnumerable<Creditor> creditors)
        {
            foreach (var creditor in creditors)
            {
                if (creditor.Available == true)
                    creditor.Trials += 1;
            }

            return creditors;
        }

        private void UpdateCreditors(int associateNr)
        {
            var creditors = _context.Creditor.ToList();
            var updateList = new List<Creditor>();
            var canUpdate = false;

            foreach (var creditor in creditors)
            {
                canUpdate = false;
                var transactions = (from t in _context.Transaction_Out
                                    where t.Creditor_Id == creditor.Creditor_Id
                                    select new { t.CreditorBenefits, t.AssociateDay, t.Ammount, t.Day_Access_To_Funds })
                                    .ToList();

                foreach (var transaction in transactions)
                {
                    if (transaction.Day_Access_To_Funds + transaction.AssociateDay == associateNr)
                    {
                        creditor.Finapp_Balance += (transaction.CreditorBenefits + transaction.Ammount ?? 0);
                        creditor.ActualMoney += (transaction.CreditorBenefits + transaction.Ammount ?? 0);
                        creditor.Available = true;
                        canUpdate = true;
                    }
                }
                if (canUpdate)
                    updateList.Add(creditor);

            }
            updateList = Mapper.Map<List<Creditor>>(updateList);

            _creditorService.ModifyCreditors(updateList);
        }

        private void UpdateDebtors(int associateNr)
        {
            var debtors = _context.Debtor.ToList();
            var updateList = new List<Debtor>();
            var toUpdate = false;

            foreach (var debtor in debtors)
            {
                toUpdate = false;

                if (debtor.Finapp_Debet == 0)
                {
                    debtor.HaveMoney++;
                    toUpdate = true;
                }
                var accessDays = debtor.Expiration_Date.Value.Subtract(DateTime.Now).Days;
                var transactions = (from t in _context.Transaction_Out
                                    where t.Debtor_Id == debtor.Debtor_Id
                                    select new { t.DebtorSavings, t.AssociateDay, t.Ammount, t.Day_Access_To_Funds })
                                    .ToList();

                foreach (var transaction in transactions)
                {
                    if (transaction.Day_Access_To_Funds + transaction.AssociateDay == associateNr)
                    {
                        debtor.Finapp_Debet += (transaction.Ammount - transaction.DebtorSavings ?? 0);
                        debtor.ActualMoney -= transaction.DebtorSavings ?? 0;
                        if (accessDays > associateNr)
                        {
                            debtor.Available = true;
                        }
                        toUpdate = true;
                    }
                }
                if (toUpdate)
                    updateList.Add(debtor);

            }
            updateList = Mapper.Map<List<Debtor>>(updateList);

            _debtorService.ModifyDebtors(updateList);
        }
    }
}