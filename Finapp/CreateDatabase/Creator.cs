﻿using Finapp.ICreateDatabase;
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
        private readonly ICreditorAccountService _creditorAccountService;
        private readonly IDebtorAccountService _debtorAccountService;

        public Creator(FinapEntities1 context, IDebtorService debtorService, ICreditorService creditorService, 
            ICreditorAccountService creditorAccountService, IDebtorAccountService debtorAccountService)
        {
            _context = context;
            _debtorService = debtorService;
            _creditorService = creditorService;
            _creditorAccountService = creditorAccountService;
            _debtorAccountService = debtorAccountService;
        }

        public void ClearDB()
        {
            _context.Database.ExecuteSqlCommand("Delete from [Summary]");
            _context.Database.ExecuteSqlCommand("Delete from [Creditor_Associate]");
            _context.Database.ExecuteSqlCommand("Delete from [Debtor_Associate]");
            _context.Database.ExecuteSqlCommand("Delete from [Return_Transaction]");
            _context.Database.ExecuteSqlCommand("Delete from [Transaction_Out]");
            _context.Database.ExecuteSqlCommand("Delete from [Associate]");
            _context.Database.ExecuteSqlCommand("Delete from [Creditor_Account]");
            _context.Database.ExecuteSqlCommand("Delete from [Debtor_Account]");
            _context.Database.ExecuteSqlCommand("Delete from [Creditor]");
            _context.Database.ExecuteSqlCommand("Delete from [Debtor]");
        }

        public void UpdateDB()
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
                var number = _creditorService.GetAllCreditors().Count();
                var c = new Creditor
                {
                    username = "Jan" + number ,
                    ROI = roi,
                    EROI = eroi,
                    Delta_ROI = droi,
                    Balance = balance,
                    Available = true,
                    Finapp_Balance = balance,
                    Queue_Date = DateTime.Now.AddDays(-rand.Next(1, 30)),
                    Expiration_Date = d,
                };

                _creditorService.AddNewCreditor(c);
                _creditorAccountService.AddCreditorAccount(new Creditor_Account
                {
                    Creditor_Id = c.Creditor_Id,
                    Balance = c.Balance,
                    Min_Balance = 0
                });

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
                    username = "Ewa" + number ,
                    APR = apr,
                    EAPR = eapr,
                    Delta_APR = dapr,
                    Debet = debet,
                    Available = true,
                    Finapp_Debet = debet,
                    Queue_Date = DateTime.Now.AddDays(-rand.Next(1, 30)),
                    Expiration_Date = d
                };
                _debtorService.AddNewDebtor(deb);

                _debtorAccountService.AddDebtorAccount(new Debtor_Account
                {
                    Debtor_Id = deb.Debtor_Id,
                    Debet = deb.Debet,
                    Credit_Line_Date = d
                });
            }
        }
    }
}