using Finapp.Implementations;
using Finapp.Interfaces;
using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Finapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAlgorithms _algorithm;

        public HomeController(IAlgorithms algorithm)
        {
            _algorithm = algorithm;
        }

        public ActionResult Index()
        {
            _algorithm.Associating();
           
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