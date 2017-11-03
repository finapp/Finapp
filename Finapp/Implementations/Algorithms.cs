using Finapp.Interfaces;
using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.Implementations
{
    public class Algorithms : IAlgorithms
    {
        public Queue<Debtor> AddDebtorsToQueue()
        {
            FinapEntities context = new FinapEntities();
            var debtorsList = context.Debtor.ToList();
            Queue<Debtor> debtors = new Queue<Debtor>();

            foreach (var debtor in debtorsList)
            {
                debtors.Enqueue(debtor);
            }

            return debtors;
        }

        public Queue<Creditor> AddCreditorsToQueue()
        {
            FinapEntities context = new FinapEntities();
            var creditorList = context.Creditor.ToList();
            Queue<Creditor> creditors = new Queue<Creditor>();

            foreach (var creditor in creditorList)
            {
                creditors.Enqueue(creditor);
            }

            return creditors;
        }

        public bool Associating()
        {
            FinapEntities context = new FinapEntities();
            Queue<Debtor> debtors = AddDebtorsToQueue();
            Queue<Creditor> creditors = AddCreditorsToQueue();

            if (debtors.Count == 0)
                return false;
            debtors.Dequeue();
            debtors.Dequeue();
            Debtor debtor = debtors.Dequeue();
            int collectedMoney = 0;
            int moneyFromLastCreditor;

            Queue<Creditor> selectedCreditors = new Queue<Creditor>();

            foreach (var creditor in creditors)
            {
                if (creditor.EROI < debtor.EAPR)
                {
                    selectedCreditors.Enqueue(creditor);
                    collectedMoney += creditor.Finapp_Balance;

                    if (collectedMoney >= debtor.Debet)
                    {
                        moneyFromLastCreditor = debtor.Debet - Math.Abs(collectedMoney - creditor.Finapp_Balance);

                        context.Transaction_Out.Add(new Transaction_Out
                        {
                            Ammount = moneyFromLastCreditor,
                            Date_Of_Transaction = DateTime.Now,
                            Creditor_Account_Id = creditor.Creditor_Id,
                            Debtor_Account_Id = debtor.Debtor_Id
                        });
                        context.SaveChanges();

                        break;
                    }

                    context.Transaction_Out.Add(new Transaction_Out
                    {
                        Ammount = creditor.Finapp_Balance,
                        Date_Of_Transaction = DateTime.Now,
                        Creditor_Account_Id = creditor.Creditor_Id,
                        Debtor_Account_Id = debtor.Debtor_Id
                    });
                    context.SaveChanges();
                }
            }



            return true;
        }


    }
}