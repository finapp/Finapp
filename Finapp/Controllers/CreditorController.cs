using Finapp.Interfaces;
using Finapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Finapp.Controllers
{
    public class CreditorController : Controller
    {
        // GET: Creditor
        private readonly ICreditor _creditorService;
        private readonly ICreditorAccount _creditorAccountService;
        private readonly ITransactionOut _transactionService;

        public CreditorController(ICreditor creditorService, ICreditorAccount credtiorAccountService, ITransactionOut transactionService)
        {
            _creditorService = creditorService;
            _creditorAccountService = credtiorAccountService;
            _transactionService = transactionService;
        }
        public ActionResult Index()
        {

            var cwm = new CreditorViewModel
            {
                Creditors = _creditorService.GetAllCreditors()
            };
            return View(cwm);
        }

        public ActionResult TransactionForCreditor(int? id)
        {
            var creditor = _creditorService.GetCreditorById(id ?? 0);

            var creditorAccountId = _creditorAccountService.getCreditorAccountByCreditorId(id ?? 0);

            var transaction = _transactionService.GetTransactionByCreditorId(creditorAccountId.Creditor_Account_Id);

            var ctvm = new CreditorTransactionViewModel
            {
                Creditor = creditor,
                Transactions = transaction
            };

            return View(ctvm);
        }

    }
}