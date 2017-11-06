using Repo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Repo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            FinapEntities context = new FinapEntities();

            Random rand = new Random();
      
            for (int i = 1; i <= 50; i++)
            {
                var roi = rand.Next(0, 3);
                var eroi = rand.Next(3, 8);
                var balance = rand.Next(500, 50000);
                DateTime d = DateTime.Now.AddDays(30);
                var c = new Creditor
                {
                    username = "Ewa" + i,
                    ROI = roi,
                    EROI = eroi,
                    Balance = balance,
                    Available = true,
                    Finapp_Balance = balance,
                    Queue_Date = DateTime.Now.AddMinutes(-i)
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

            for (int i = 1; i <= 50; i++)
            {
                var apr = rand.Next(8, 30);
                var eapr = rand.Next(3, apr - 2);
                if (eapr > 18)
                    eapr = 18;
                var debet = rand.Next(1000, 50000);
                DateTime d = DateTime.Now.AddDays(30);

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

        
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}