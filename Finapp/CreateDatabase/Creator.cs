using Finapp.ICreateDatabase;
using Finapp.IServices;
using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Finapp.CreateDatabase
{
    public class Creator : ICreator
    {
        private readonly FinapEntities1 _context;
        private readonly IDebtorService _debtorService;
        private readonly ICreditorService _creditorService;

        public Creator(FinapEntities1 context, IDebtorService debtorService, ICreditorService creditorService)
        {
            _context = context;
            _debtorService = debtorService;
            _creditorService = creditorService;
        }

        public void ClearDB()
        {
            _context.Database.ExecuteSqlCommand("Delete from [Summary]");
            _context.Database.ExecuteSqlCommand("Delete from [Creditor_Associate]");
            _context.Database.ExecuteSqlCommand("Delete from [Debtor_Associate]");
            _context.Database.ExecuteSqlCommand("Delete from [Transaction_Out]");
            _context.Database.ExecuteSqlCommand("Delete from [Associate]");
            _context.Database.ExecuteSqlCommand("Delete from [Creditor]");
            _context.Database.ExecuteSqlCommand("Delete from [Debtor]");
            _context.Database.ExecuteSqlCommand("Delete from [Times]");
        }

        public void UpdateCreditors()
        {
            var creditors = _context.Creditor.ToList();

            _creditorService.ModifyCreditors(creditors);
        }

        public void UpdateDebtors()
        {
            var debtors = _context.Debtor.ToList();

            _debtorService.ModifyDebtors(debtors);
        }


        public void CreateDB(int amountOfDebtors, int amountOfCreditors)
        {
            Random rand = new Random();

            var creditors = _context.Creditor.ToList();
            var creditorsCounter = creditors.Count();
            var idCreditor = 0;
            var idDebtor = 0;

            DateTime creditorQueueDate;

            if (creditorsCounter == 0)
            {
                creditorQueueDate = DateTime.Now;
            }
            else
            {
                var date = creditors.Max(c => c.Queue_Date);
                creditorQueueDate = date ?? DateTime.Now;
                idCreditor = creditorsCounter;
            }

            var debtors = _context.Debtor.ToList();
            var debtorsCounter = debtors.Count();
            DateTime debtorQueueDate;

            if (debtorsCounter == 0)
            {
                debtorQueueDate = DateTime.Now;
            }
            else
            {
                var date = debtors.Max(c => c.Queue_Date);
                debtorQueueDate = date ?? DateTime.Now;
                idDebtor = debtorsCounter;
            }

            var creditorsToCreate = new List<Creditor>(amountOfCreditors);

            for (int i = 1; i <= amountOfCreditors; i++)
            {
                int balance = 0;
                var propability = rand.Next(1, 20);
                if (propability > 1 && propability < 20)
                    balance = rand.Next(10, 100) * 100;
                else if (propability == 0)
                {
                    balance = rand.Next(1, 10) * 100;
                }
                else
                {
                    balance = rand.Next(100, 500) * 100;
                }
                var roi = rand.Next(0, 3);
                var eroi = rand.Next(4, 8);
                var droi = eroi - roi;
                DateTime d = DateTime.Now.AddDays(rand.Next(1, 12) * 7);
                var days = d.Subtract(DateTime.Now).Days; 
                var actualBenefits = (int)(balance * ((float)roi / 100)*(float)days/365);
                var number = i+idCreditor;

                var c = new Creditor
                {
                    Creditor_Id = i + idCreditor,
                    username = "Jan" + number,
                    ROI = roi,
                    EROI = eroi,
                    Delta_ROI = droi,
                    Balance = balance,
                    ActualMoney = balance,
                    Available = true,
                    Finapp_Balance = balance,
                    Queue_Date = creditorQueueDate.AddDays(rand.Next(1, 30)),
                    Expiration_Date = d,
                    Trials = 0,
                    AssociateCounter = 0,
                    LastAssociate = 0,
                    ActualCreditorBenefits = actualBenefits,
                    Profits = 0
                };

                creditorsToCreate.Add(c);

            }

            _creditorService.AddNewCreditors(creditorsToCreate);

            var debtorsToCreate = new List<Debtor>(amountOfDebtors);

            for (int i = 1; i <= amountOfDebtors; i++)
            {
                var propability = rand.Next(1, 20);
                float apr = 0, eapr = 0;
                int debet = 0;
                if (propability > 1 && propability < 20)
                {
                    apr = rand.Next(10, 16);
                    eapr = rand.Next(6, (int)apr - 2);
                    debet = rand.Next(50, 300) * 100;
                }
                else if (propability == 1)
                {
                    apr = rand.Next(5, 10);
                    eapr = rand.Next(3, (int)apr - 1);
                    debet = rand.Next(1, 50) * 100;
                }
                else
                {
                    apr = rand.Next(17, 40);
                    eapr = rand.Next(14, (int)apr - 3);
                    debet = rand.Next(300, 500) * 100;
                }

                var dapr = apr - eapr;
                DateTime d = DateTime.Now.AddDays(rand.Next(15, 50) * 7);

                var number = i + idDebtor;

                var deb = new Debtor
                {
                    Debtor_Id = i + idDebtor,
                    username = "Ewa" + number,
                    APR = apr,
                    EAPR = eapr,
                    Delta_APR = dapr,
                    Debet = debet,
                    ActualMoney = debet,
                    Available = true,
                    Finapp_Debet = debet,
                    Queue_Date = debtorQueueDate.AddDays(rand.Next(1, 30)),
                    Expiration_Date = d,
                    Trials = 0,
                    AssociateCounter = 0,
                    LastAssociate = 0,
                    Savings = 0,
                    HaveMoney = 0
                };
                debtorsToCreate.Add(deb);
            
                
            }

            _debtorService.AddNewDebtors(debtorsToCreate);
        }
    }
}