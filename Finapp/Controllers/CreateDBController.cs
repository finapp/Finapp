using Finapp.CreateDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Finapp.Controllers
{
    public class CreateDBController : Controller
    {
        // GET: CreateDB
        public ActionResult Index()
        {
            Creator create = new Creator();

            create.CreateDB();

            return RedirectToAction("Index", "Debtor");
        }
    }
}