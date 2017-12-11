using Finapp.CreateDatabase;
using Finapp.ICreateDatabase;
using Finapp.IServices;
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
        private readonly IDBSummaryViewModelService _dbSummaryService;

        public CreateDBController(ICreator creator, IDBSummaryViewModelService dbSummaryService)
        {
            _creator = creator;
            _dbSummaryService = dbSummaryService;
        }

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
                _creator.ClearDB();
                _creator.CreateDB(debtors, creditors);

                return RedirectToAction("Index", "Debtor");
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError(e.ToString());
                ViewBag.ErrorMessage = "Please enter valid data";
                return View("Index");
            }
        }

        public ActionResult NewCustomers()
        {
            return View(_dbSummaryService.GetViewModel());
        }

        [HttpPost]
        public ActionResult AddNewCustomers()
        {
            var amountOfCreditors = Request["amountOfCreditors"];
            var amountOfDebtors = Request["amountOfDebtors"];
            var updateCreditors = Request["updateCreditors"];
            var updateDebtors = Request["updateDebtors"];

            try
            {
                var creditors = int.Parse(amountOfCreditors);
                var debtors = int.Parse(amountOfDebtors);
                if (updateCreditors == "on")
                    _creator.UpdateCreditors();

                if (updateDebtors == "on")
                    _creator.UpdateDebtors();

                _creator.CreateDB(debtors, creditors);
                return RedirectToAction("Index", "Debtor");
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Please enter valid data";

                if (updateCreditors == null)
                    return View("NewCustomers");

                return View("Index");
            }
        }
    }
}