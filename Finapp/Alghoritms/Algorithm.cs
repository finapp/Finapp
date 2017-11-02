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
            FinapEntities context = new FinapEntities();

            Queue<Debtor> debtors = new Queue<Debtor>();
            var deb = context.Debtor.ToList();

            foreach (var item in deb)
            {
                debtors.Enqueue(item);
            }

            Queue<Creditor> creditors  = new Queue<Creditor>();
            var cred = context.Creditor.ToList();

            foreach (var item in cred)
            {
                creditors.Enqueue(item);
            }

            
        }
    }
}