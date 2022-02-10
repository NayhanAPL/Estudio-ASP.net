using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using versión_5_asp.Models;

namespace versión_5_asp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<string> posibilidades = new List<string>() {
        "Crear tus propios trueques.",
        "Recibir las sugerencias que le ofrecemos para sus trueques.",
        "Iterar por todos los trueques que se encuentren activos para hacer su selección.",
        "Enviar ofertas de trueques a otros usuarios.",
        "Gestionar y mantenerce informado sobre sus ofertas enviadas.",
        "Recibir ofertas y gestionarlas hasta su finalización.",
        "Finalizar un TRUEQUE y comenzar la comunicación con otros usuarios.",
        "Recibir ayuda y leer la guía sobre TRUEQUE."
            };
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
