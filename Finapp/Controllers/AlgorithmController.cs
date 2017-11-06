using Finapp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Finapp.Controllers
{
    public class AlgorithmController : Controller
    {
        private readonly IAlgorithms _algorithm;

        public AlgorithmController(IAlgorithms algorithm)
        {
            _algorithm = algorithm;
        }

        // GET: Algorithm
        public async Task<ActionResult> Index()
        {
            await Task.Run(() => _algorithm.Associating());
            return RedirectToAction("Index", "Debtor");
        }
    }
}