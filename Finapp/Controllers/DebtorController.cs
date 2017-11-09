using Finapp.IServices;
using Finapp.Models;
using Finapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Finapp.Controllers
{
    public class DebtorController : Controller
    {
        private readonly IDebtorViewModelService _debtorViewModelService;
        private readonly ITransactionOutService _transactionOutService;

        public DebtorController(IDebtorViewModelService debtorViewModelService, ITransactionOutService transactionOutService)
        {
            _debtorViewModelService = debtorViewModelService;
            _transactionOutService = transactionOutService;
        }

        // GET: Debtor


        public ActionResult Index()
        {
           // var a = _transactionOutService.GetTransactionsWithDebtorByDebtorId(1996);
            return View(_transactionOutService.GetTransactionsWithDebtorByDebtorId(1919));
        }
    }
}