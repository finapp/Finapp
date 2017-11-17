using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Finapp.Controllers
{
    public class RankController : Controller
    {

        public ActionResult CreditorRank()
        {
            return View();
        }

        public ActionResult DebtorRank()
        {
            return View();
        }
    }
}