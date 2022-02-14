﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using versión_5_asp.Models;
using versión_5_asp.services;

namespace versión_5_asp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public IFunciones funciones1 {get; }
        public HomeController(IFunciones funciones, ILogger<HomeController> logger)
        {
            funciones1 = funciones;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var posibilidades = funciones1.TodasLasPosibilidades();
            return View(posibilidades);
        }

        public IActionResult Privacy()
        {
           
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
