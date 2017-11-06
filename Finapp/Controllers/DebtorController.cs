using Finapp.Interfaces;
using Finapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Finapp.Controllers
{
    public class DebtorController : Controller
    {
        // GET: Debtor
        private readonly IAlgorithm _algorithmService;
        private readonly IDebtor _debtorService;
        private readonly ITransactionOut _transactionService;
        private readonly IDebtorAccount _debtorAccountService;


        public DebtorController(IAlgorithm algorithmService, IDebtor debtorService, ITransactionOut transactionService, IDebtorAccount debtorAccountService)
        {
            _algorithmService = algorithmService;
            _debtorService = debtorService;
            _transactionService = transactionService;
            _debtorAccountService = debtorAccountService;
        }

        public ActionResult Index()
        {
            var dwm = new DebtorViewModel
            {
                Debtors = _debtorService.GetAllDeptors()
            };
            return View(dwm);
        }

        public ActionResult TransactionForDeptor(int? id)
        {
            var debtor = _debtorService.GetDeptorById(id ?? 0);

            var debtorAccountId = _debtorAccountService.getDebtorAccountByDebtorId(id ?? 0);

            var transaction = _transactionService.GetTransactionByDebtorId(debtorAccountId.Debtor_Account_Id);

            var dtvm = new DebtorTransactionViewModel
            {
                Debtor = debtor,
                Transactions = transaction
            };

            return View(dtvm);
        }


        public async Task<ActionResult> Algorithm()
        {
            while (await Task.Run(() => _algorithmService.MergeDebtorWithCreditors()) == true) ;

            return RedirectToAction("Index");
        }
    }
}