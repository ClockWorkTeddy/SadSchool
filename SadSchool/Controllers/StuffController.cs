using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using SadSchool.Models;
using Microsoft.AspNetCore.Authorization;

namespace SadSchool.Controllers
{
    public class StuffController : Controller
    {
        private readonly SadSchoolContext _context;

        public StuffController(SadSchoolContext context)
        {
            _context = context; 
        }

        public IActionResult Stuff()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("admin"))
                return View("~/Views/Stuff/Stuff.cshtml");
            else
                return View("Index");
        }
    }
}
