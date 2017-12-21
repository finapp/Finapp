using Finapp.Implementations;
using Finapp.Interfaces;
using Finapp.IServices;
using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Finapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITests _tests;
        private readonly FinapEntities1 _context;

        public HomeController(ITests tests, FinapEntities1 context)
        {
            _tests = tests;
            _context = context;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Transaction");
        }

        public ActionResult Test()
        {
            return View(_context.Times.ToList());
        }

        public ActionResult Charts()
        {
            var model = _context.Times.FirstOrDefault();
            ViewBag.Message = (_context.Creditor.Count() + _context.Debtor.Count() + " users in seconds");

            return View(GetTimesObj(model));
        }

        private Times GetTimesObj(Times obj)
        {
            Times times = new Times();
            string[] separators = { ":" };

            string[] words = obj.GetDebtorsTime.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            times.GetDebtorsTime = GetTime(words);

            words = obj.GetDebtorsTime.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            times.GetDebtorsTime = GetTime(words);

            words = obj.GetCreditorsTime.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            times.GetCreditorsTime = GetTime(words);

            words = obj.SetROI.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            times.SetROI = GetTime(words);

            words = obj.AssociateTime.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            times.AssociateTime = GetTime(words);

            words = obj.UpdateDebtorsTime.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            times.UpdateDebtorsTime = GetTime(words);

            words = obj.UpdateCreditorsTime.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            times.UpdateCreditorsTime = GetTime(words);

            words = obj.UpdateTransactionsTime.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            times.UpdateTransactionsTime = GetTime(words);

            return times;
        }

        private string GetTime(string[] times)
        {
            int hours = int.Parse(times[0]);
            int minutes = int.Parse(times[1]);
            int seconds = int.Parse(times[2]);
            int ms = int.Parse(times[3]);

            int time = hours * 3600 + minutes * 60 + seconds;

            string timeString = time + "." + ms;

            return timeString;
        }
    }
}