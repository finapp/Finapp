﻿using Finapp.ICreateDatabase;
using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.CreateDatabase
{
    public class Creator : ICreator
    {
        public void CreateDB(int amountOfDebtors, int amountOfCreditors)
        {
            FinapEntities1 context = new FinapEntities1();

            Random rand = new Random();

            context.Database.ExecuteSqlCommand("Delete from [Return_Transaction]");
            context.Database.ExecuteSqlCommand("Delete from [Transaction_Out]");
            context.Database.ExecuteSqlCommand("Delete from [Creditor_Account]");
            context.Database.ExecuteSqlCommand("Delete from [Debtor_Account]");
            context.Database.ExecuteSqlCommand("Delete from [Creditor]");
            context.Database.ExecuteSqlCommand("Delete from [Debtor]");

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
                DateTime d = DateTime.Now.AddDays(rand.Next(1, 12) * 7);
                var c = new Creditor
                {
                    username = "Ewa" + i,
                    ROI = roi,
                    EROI = eroi,
                    Balance = balance,
                    Available = true,
                    Finapp_Balance = balance,
                    Queue_Date = DateTime.Now.AddHours(-rand.Next(1,30))
                };

                context.Creditor.Add(c);
                context.SaveChanges();

                context.Creditor_Account.Add(new Creditor_Account
                {
                    Creditor_Id = c.Creditor_Id,
                    Balance = c.Balance,
                    Expiration_Date = d,
                    Min_Balance = 0
                });
                context.SaveChanges();
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
                    debet = rand.Next(0, 50) * 100;
                }
                else
                {
                    apr = rand.Next(17, 40);
                    eapr = rand.Next(14, (int)apr - 3);
                    debet = rand.Next(300, 500) * 100;
                }

                DateTime d = DateTime.Now.AddDays(365);

                var deb = new Debtor
                {
                    username = "Adam" + i,
                    APR = apr,
                    EAPR = eapr,
                    Debet = debet,
                    Available = true,
                    Finapp_Debet = debet,
                    Queue_Date = DateTime.Now.AddMinutes(-i)
                };
                context.Debtor.Add(deb);
                context.SaveChanges();

                context.Debtor_Account.Add(new Debtor_Account
                {
                    Debtor_Id = deb.Debtor_Id,
                    Debet = deb.Debet,
                    Expiration_Date = d,
                    Credit_Line_Date = d
                });
                context.SaveChanges();
            }

        }
    }
}