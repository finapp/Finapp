using Finapp.Implementations;
using Finapp.Interfaces;
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
        private readonly IAlgorithms _algorithm;
        private readonly FinapEntities _context;

        public HomeController(IAlgorithms algorithm, FinapEntities context)
        {
            _algorithm = algorithm;
            _context = context;
        }

        public async Task<ActionResult> Index()
        {
            await Task.Run(() => _algorithm.Associating());



            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}