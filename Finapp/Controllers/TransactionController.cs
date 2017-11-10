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
        private readonly ITransactionViewModelService _transactionViewModelService;
        private readonly ITransactionOutService _transactionService;

        public TransactionController(ITransactionViewModelService transactionViewModelService, ITransactionOutService transactionService)
        {
            _transactionViewModelService = transactionViewModelService;
            _transactionService = transactionService;
        }

        public ActionResult Index(string username)
        {
            return View("Index", _transactionService.GetTransactions());
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