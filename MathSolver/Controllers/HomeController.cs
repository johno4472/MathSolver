using System.Diagnostics;
using MathSolver.Models;
using Microsoft.AspNetCore.Mvc;
using MathSolver.Helpers;

namespace MathSolver.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ParsedLineResponse parsedLineResponse = new ParsedLineResponse();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Calculate(string equationFile)
        {
            parsedLineResponse.ReadAndCalculate(equationFile);
            //solve file
            return View();
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
