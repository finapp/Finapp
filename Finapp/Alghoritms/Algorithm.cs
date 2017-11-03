using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.Alghoritms
{
    public static class Algorithm
    {

        public static void Debtor()
        {

            Queue<Creditor> creditors = getCreditors();

            Queue<Debtor> debtors = getDebtors();

            if (creditors == null || debtors == null) return;

            var selectedDebtor = debtors.Dequeue();

            Queue<Creditor> selectedCreditors = selectCreditors(selectedDebtor, creditors);
        }


        public static Queue<Creditor> getCreditors()
        {
            FinapEntities context = new FinapEntities();

            Queue<Creditor> creditors = new Queue<Creditor>();
            var cred = context.Creditor.ToList();

            foreach (var item in cred)
            {
                creditors.Enqueue(item);
            }
            return creditors;
        }

        public static Queue<Debtor> getDebtors()
        {
            FinapEntities context = new FinapEntities();

            Queue<Debtor> debtors = new Queue<Debtor>();
            var deb = context.Debtor.ToList();

            foreach (var item in deb)
            {
                if (item.Debet > 0)
                    debtors.Enqueue(item);
            }
            return debtors;
        }

        public static Queue<Creditor> selectCreditors(Debtor selectedDebtor, Queue<Creditor> creditors)
        {
            Queue<Creditor> selectedCreditors = new Queue<Creditor>();
            var suma = 0;

            foreach (var item in creditors)
            {
                if (item.EROI < selectedDebtor.EAPR && item.Balance > 0)
                {
                    if (suma + item.Balance < selectedDebtor.Debet)
                    {
                        suma += item.Balance ;
                        selectedCreditors.Enqueue(item);
                    }
                }
            }

            return selectedCreditors;
        }
    }
}