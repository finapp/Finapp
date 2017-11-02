using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Finapp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            FinapEntities context = new FinapEntities();
            Random rand = new Random();
            for (int i = 1; i <= 50; i++)
            {
                DateTime d = DateTime.Now.AddDays(30);
                Debtor c = context.Debtor.Where(x => x.Debtor_Id == i).FirstOrDefault();
                context.Debtor_Account.Add(new Debtor_Account
                {
                    Debtor_Id = i,
                    Debet = c.debet,
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