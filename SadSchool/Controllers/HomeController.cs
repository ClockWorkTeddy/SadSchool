using Microsoft.AspNetCore.Mvc;
using SadSchool.Models;
using SadSchool.Services;
using SadSchool.ViewModels;
using System.Diagnostics;

namespace SadSchool.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SadSchoolContext _context;
        private readonly INavigationService _navigationService;

        public HomeController(
            ILogger<HomeController> logger, 
            SadSchoolContext context, 
            INavigationService navigationService )
        {
            _logger = logger;
            _context = context;
            _navigationService = navigationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            try
            {
                _navigationService.RefreshBackParams(RouteData);
                return View();
            }
            catch(Exception ex)
            {
                return View(ex.Message);
            }
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