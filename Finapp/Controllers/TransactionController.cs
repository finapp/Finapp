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
        private readonly ITransactionOutService _transactionService;
        private readonly IAssociateViewModelService _associateService;

        public TransactionController(ITransactionOutService transactionService, IAssociateViewModelService associateService)
        {
            _transactionService = transactionService;
            _associateService = associateService;
        }

        public ActionResult Index(string username)
        {
            return View("Index", _associateService.GetAllTransactions());
        }

        public ActionResult CreditorTransactions(string username)
        {
            return View("Index", _transactionService.GetTransactionWithCreditorByCreditorUsername(username));
        }

        public ActionResult DebtorTransactions(string username)
        {
            return View("Index", _transactionService.GetTransactionsWithDebtorByDebtorUsername(username));
        }
    }
}