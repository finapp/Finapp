﻿using Finapp.Implementations;
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
        private readonly FinapEntities1 _context;
        private readonly IDebtorService _service;
        private readonly IStatisticsViewModelService _aservice;

        public HomeController(FinapEntities1 context, IDebtorService service, IStatisticsViewModelService aservice)
        {
            _context = context;
            _service = service;
            _aservice = aservice;
        }

        public ActionResult Index()
        {
            var a = _aservice.GetSummary();
            return View();
        }
    }
}