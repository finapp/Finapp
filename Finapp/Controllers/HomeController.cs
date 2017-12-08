using Finapp.Implementations;
using Finapp.Interfaces;
using Finapp.IServices;
using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Finapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITests _tests;

        public HomeController(ITests tests)
        {
            _tests = tests;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Transaction");
        }

        public ActionResult Test()
        {
            return View(_tests.TestFor1000());
        }
    }
}