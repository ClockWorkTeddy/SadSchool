using Microsoft.AspNetCore.Mvc;
using SadSchool.Models;
using SadSchool.Services;

namespace SadSchool.Controllers
{
    public class StuffController : Controller
    {
        private readonly SadSchoolContext _context;
        private readonly INavigationService _navigationService;

        public StuffController(SadSchoolContext context, INavigationService navigationService)
        {
            _context = context; 
            _navigationService = navigationService;
        }

        public IActionResult Stuff()
        {
            _navigationService.RefreshBackParams(RouteData);

            if (User.Identity.IsAuthenticated && User.IsInRole("admin"))
                return View("~/Views/Stuff/Stuff.cshtml");
            else
                return View("Index");
        }
    }
}
