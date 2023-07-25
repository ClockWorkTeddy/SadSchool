using Microsoft.AspNetCore.Mvc;
using SadSchool.Models;
using SadSchool.ViewModels;

namespace SadSchool.Controllers
{
    public class ClassController : Controller
    {
        private readonly SadSchoolContext _context;

        public ClassController(SadSchoolContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Classes()
        {
            List<ClassViewModel> classes = new List<ClassViewModel>();

            foreach (var c in _context.Classes)
            {
                classes.Add(new ClassViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    TeacherId = c.TeacherId,
                    LeaderId = c.LeaderId
                });
            }

            return View(@"~/Views/Data/Classes.cshtml", classes);
        }

        [HttpGet]
        public IActionResult AddClass()
        {
            var teachers = _context.Teachers.ToList();
            ClassAddViewModel viewModel = new ClassAddViewModel()
            {

            };
            return View(@"~/Views/Data/AddClass.cshtml" );
        }
    }
}
