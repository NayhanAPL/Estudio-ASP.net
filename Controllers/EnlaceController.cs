using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace versión_5_asp.Controllers
{
    public class EnlaceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
