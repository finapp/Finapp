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
        private readonly IAssociateViewModelService _associateService;

        public TransactionController(ITransactionViewModelService transactionViewModelService, ITransactionOutService transactionService, IAssociateViewModelService associateService)
        {
            _transactionViewModelService = transactionViewModelService;
            _transactionService = transactionService;
            _associateService = associateService;
        }

        public ActionResult Index(string username)
        {
            //return View("Index", _transactionService.GetTransactions());
            return View("Index", _associateService.GetTransactions());
        
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