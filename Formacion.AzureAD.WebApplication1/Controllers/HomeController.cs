using Formacion.AzureAD.WebApplication1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Formacion.AzureAD.WebApplication1.Controllers
{    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]                            //Acceso sin autenticación
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]                                 //Usuarios autenticados
        [Authorize(Roles = "Rol-App-Admin")]        //Usuarios autenticados con el Rol de Aplicación definido en el portal de Azure
        [Authorize(Policy = "Grupo-BackOffice")]    //Usuarios autenticados con la Política definida en la clases Startup
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
