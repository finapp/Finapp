using Finapp.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Finapp.Controllers
{
    public class TransactionController : Controller
    {
        private readonly IAssociateViewModelService _associateService;
        private readonly ISummaryViewModelService _summaryService;

        public TransactionController(IAssociateViewModelService associateService, ISummaryViewModelService summaryService)
        {
            _associateService = associateService;
            _summaryService = summaryService;
        }

        public ActionResult Index(string username)
        {
            return View("Index", _summaryService.GetAllInformations());
        }

        public ActionResult CreditorTransactions(string username)
        {
            return View("DebtorOrCreditorTransactions", _associateService.GetTransactionsByCreditorUsername(username));
        }

        public ActionResult DebtorTransactions(string username)
        {
            return View("DebtorOrCreditorTransactions", _associateService.GetTransactionsByDebtorUsername(username));
        }
    }
}