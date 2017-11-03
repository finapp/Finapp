using Finapp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Finapp.Controllers
{
    public class DebtorController : Controller
    {
        // GET: Debtor
        private readonly IAlgorithm _algorithmService;
        private readonly IDebtor _debtorService;
        public DebtorController(IAlgorithm algorithmService, IDebtor debtorService)
        {
            _algorithmService = algorithmService;
            _debtorService = debtorService;
        }

        public ActionResult Index()
        {
            //while (_algorithmService.MergeDebtorWithCreditors() == true) ;
            var debtors = _debtorService.GetAllDeptors();

            return View(debtors);
        }
    }
}