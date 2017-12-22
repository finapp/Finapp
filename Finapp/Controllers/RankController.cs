using Finapp.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Finapp.Controllers
{
    public class RankController : Controller
    {
        private readonly IRankViewModelService _rankService;

        public RankController(IRankViewModelService rankService)
        {
            _rankService = rankService;
        }

        public ActionResult CreditorRank()
        {
            var a = _rankService.GetCreditorsRank();
            return View(_rankService.GetCreditorsRank());
        }

        public ActionResult DebtorRank()
        {
            return View(_rankService.GetDebtorsRank());
        }
    }
}