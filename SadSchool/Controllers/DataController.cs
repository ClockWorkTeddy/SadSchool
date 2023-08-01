using Microsoft.AspNetCore.Mvc;

namespace SadSchool.Controllers
{
    public class DataController : Controller
    {
        [HttpGet]
        public IActionResult DataIndex()
        {
            return View(@"~/Views/Data/DataIndex.cshtml");
        }
    }
}
