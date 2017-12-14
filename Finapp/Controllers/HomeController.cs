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
        private readonly FinapEntities1 _context;

        public HomeController(ITests tests, FinapEntities1 context)
        {
            _tests = tests;
            _context = context;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Transaction");
        }

        public ActionResult Test()
        {
            return View(_context.Times.ToList());
        }
    }
}