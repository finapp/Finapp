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

        public TransactionController(ITransactionViewModelService transactionViewModelService)
        {
            _transactionViewModelService = transactionViewModelService;
        }

        // GET: Transaction
        public ActionResult Index()
        {

            return View();
        }
    }
}