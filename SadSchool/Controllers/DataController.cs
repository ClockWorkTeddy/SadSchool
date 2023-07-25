using Microsoft.AspNetCore.Mvc;

namespace SadSchool.Controllers
{
    public class DataController : Controller
    {
        public IActionResult DataIndex()
        {
            return View(@"~/Views/Data/DataIndex.cshtml");
        }

        [HttpGet]
        public IActionResult Data()
        {
            return View(@"~/Views/Data/Data.cshtml");
        }
    }
}
