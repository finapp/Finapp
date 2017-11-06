using Finapp.CreateDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Finapp.Controllers
{
    public class CreateDBController : Controller
    {
        // GET: CreateDB
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create()
        {
            Creator create = new Creator();
            var amountOfCreditors = Request["amountOfCreditors"];
            var amountOfDebtors = Request["amountOfDebtors"];

            try
            {
                var creditors = int.Parse(amountOfCreditors);
                var debtors = int.Parse(amountOfDebtors);
                create.CreateDB(debtors, creditors);
                return RedirectToAction("Index", "Debtor");
            }
            catch(Exception e)
            {
                ViewBag.ErrorMessage = "Please enter valid data";
                return View("Index");
            }
            


        }
    }
}