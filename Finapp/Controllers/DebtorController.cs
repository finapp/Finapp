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
        public DebtorController(IAlgorithm algorithmService)
        {
            _algorithmService = algorithmService;
        }

        public ActionResult Index()
        {
            _algorithmService.MergeDebtorWithCreditors();
            return View();
        }
    }
}