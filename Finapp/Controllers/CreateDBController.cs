using Finapp.CreateDatabase;
using Finapp.ICreateDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Finapp.Controllers
{
    public class CreateDBController : Controller
    {
        private readonly ICreator _creator;

        public CreateDBController(ICreator creator)
        {
            _creator = creator;
        }

        // GET: CreateDB
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create()
        {
            var amountOfCreditors = Request["amountOfCreditors"];
            var amountOfDebtors = Request["amountOfDebtors"];

            try
            {
                var creditors = int.Parse(amountOfCreditors);
                var debtors = int.Parse(amountOfDebtors);
                _creator.CreateDB(debtors, creditors);

                return RedirectToAction("Index", "Debtor");
            }
            catch(Exception e)
            {
                ViewBag.ErrorMessage = "Please enter valid data";
                return View("Index");
            }
        }

        public ActionResult AddNewCustomers()
        {
            return View("Index");
        }
    }
}