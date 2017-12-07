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
        }

        public void UpdateCreditors()
        {
            var creditors = _context.Creditor.ToList();

            foreach (var creditor in creditors)
            {
                creditor.Available = true;
                creditor.Finapp_Balance = creditor.Balance;

                _context.Creditor.Attach(creditor);
                _context.Entry(creditor).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public void UpdateDebtors()
        {
            var debtors = _context.Debtor.ToList();

            foreach (var debtor in debtors)
            {
                debtor.Available = true;
                debtor.Finapp_Debet = debtor.Debet;

                _context.Debtor.Attach(debtor);
                _context.Entry(debtor).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }


        public void CreateDB(int amountOfDebtors, int amountOfCreditors)
        {
            Random rand = new Random();

            var creditors = _context.Creditor.ToList();
            var creditorsCounter = creditors.Count();
            DateTime creditorQueueDate;

            if (creditorsCounter == 0)
                creditorQueueDate = DateTime.Now;
            else
            {
                var date = creditors.Max(c => c.Queue_Date);
                creditorQueueDate = date ?? DateTime.Now;
            }

            var debtors = _context.Debtor.ToList();
            var debtorsCounter = debtors.Count();
            DateTime debtorQueueDate;

            if (debtorsCounter == 0)
                debtorQueueDate = DateTime.Now;
            else
            {
                var date = debtors.Max(c => c.Queue_Date);
                debtorQueueDate = date ?? DateTime.Now;
            }


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
                var number = _creditorService.GetAllCreditors().Count();

                var c = new Creditor
                {
                    username = "Jan" + number,
                    ROI = roi,
                    EROI = eroi,
                    Delta_ROI = droi,
                    Balance = balance,
                    Available = true,
                    Finapp_Balance = balance,
                    Queue_Date = creditorQueueDate.AddDays(rand.Next(1, 30)),
                    Expiration_Date = d,
                    Trials = 0,
                    AssociateCounter = 0,
                    LastAssociate = 0,
                    ActualCreditorBenefits = actualBenefits,
                };

                _creditorService.AddNewCreditor(c);
            }

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

                var number = _debtorService.GetAllDebtors().Count();

                var deb = new Debtor
                {
                    username = "Ewa" + number,
                    APR = apr,
                    EAPR = eapr,
                    Delta_APR = dapr,
                    Debet = debet,
                    Available = true,
                    Finapp_Debet = debet,
                    Queue_Date = debtorQueueDate.AddDays(rand.Next(1, 30)),
                    Expiration_Date = d,
                    Trials = 0,
                    AssociateCounter = 0,
                    LastAssociate = 0
                };
                _debtorService.AddNewDebtor(deb);
            }
        }
    }
}