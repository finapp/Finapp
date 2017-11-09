using Finapp.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Finapp.Controllers
{
    public class CreditorController : Controller
    {
        private readonly ICreditorViewModelService _creditorViewModelService;

        public CreditorController(ICreditorViewModelService creditorViewModelService)
        {
            _creditorViewModelService = creditorViewModelService;
        }

        // GET: Creditor
        public ActionResult Index()
        {
            return View(_creditorViewModelService.GetAllCreditorsViewModel());
        }

        public ActionResult CreditorsQueue()
        {
            return View("Index", _creditorViewModelService.GetWithBalanceCreditorsViewModel());
        }
    }
}