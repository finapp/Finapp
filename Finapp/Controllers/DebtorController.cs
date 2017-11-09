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

        public DebtorController(IDebtorViewModelService debtorViewModelService)
        {
            _debtorViewModelService = debtorViewModelService;
        }

        // GET: Debtor


        public ActionResult Index()
        {
            return View(_debtorViewModelService.GetAllDebtorsViewModel());
        }

        public ActionResult DebtorsQueue()
        {
            return View("Index",_debtorViewModelService.GetWithDebetDebtorsViewModel());

        }
    }
}