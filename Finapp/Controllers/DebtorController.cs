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
        IDebtor debtorService;
        public DebtorController(IDebtor _debtorService)
        {
            debtorService = _debtorService;
        }

        public ActionResult Index()
        {
            var debtor = debtorService.GetAvaialbleDebtor();
            return View();
        }
    }
}