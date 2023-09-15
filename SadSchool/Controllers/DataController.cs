using Microsoft.AspNetCore.Mvc;
using SadSchool.Services;

namespace SadSchool.Controllers
{
    public class DataController : Controller
    {
        private INavigationService _navigationService;

        public DataController(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        [HttpGet]
        public IActionResult DataIndex()
        {
            _navigationService.RefreshBackParams(RouteData);
            return View(@"~/Views/Data/DataIndex.cshtml");
        }
    }
}
