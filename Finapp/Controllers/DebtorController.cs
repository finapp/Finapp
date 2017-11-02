using Finapp.Alghoritms;
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
        public ActionResult Index()
        {
            Algorithm.Debtor();
            return View();
        }
    }
}