using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            foreach (var c in _context.Classes.Include(cl => cl.Teacher))
            {
                classes.Add(new ClassViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Teacher = c.Teacher,
                    Leader = _context.Students.Find(c.LeaderId)
                }); ;
            }

            return View(@"~/Views/Data/Classes.cshtml", classes);
        }

        [HttpGet]
        public IActionResult AddClass()
        {
            var teachers = _context.Teachers.ToList();

            ClassAddViewModel viewModel = new ClassAddViewModel()
            {
                TeachersForView = teachers
            };

            return View(@"~/Views/Data/ClassAdd.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddClass(ClassAddViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var Class = new Class
                {
                    Name = viewModel.Name,
                    TeacherId = viewModel.TeacherId,
                    Teacher = _context.Teachers.Find(viewModel.TeacherId)
                };

                _context.Classes.Add(Class);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Classes");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClass(int id)
        {
            var Class = await _context.Classes.FindAsync(id);

            if (Class != null)
            {
                _context.Classes.Remove(Class);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Classes");
        }
    }
}
