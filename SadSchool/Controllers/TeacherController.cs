using Microsoft.AspNetCore.Mvc;
using SadSchool.Models;
using SadSchool.ViewModels;

namespace SadSchool.Controllers
{
    public class TeacherController : Controller
    {
        private readonly SadSchoolContext _context;

        public TeacherController(SadSchoolContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Teachers()
        {
            List<TeacherViewModel> teachers = new List<TeacherViewModel>();

            foreach (var t in _context.Teachers)
            {
                teachers.Add(new TeacherViewModel
                {
                    Id = t.Id,
                    Name = t.FirstName,
                    LastName = t.LastName,
                    DateOfBirth = t.DateOfBirth?.ToString(),
                    Grade = t.Grade
                });
            }

            return View(@"~/Views/Data/Teachers.cshtml", teachers);
        }

        [HttpGet]
        public IActionResult AddTeacher()
        {
            return View(@"~/Views/Data/TeacherAdd.cshtml");
        }

        [HttpPost]
        public IActionResult AddTeacher(TeacherViewModel model)
        {
            if (ModelState.IsValid)
            {
                var teacher = new Teacher
                {
                    FirstName = model.Name,
                    LastName = model.LastName,
                    DateOfBirth = model.DateOfBirth,
                    Grade = model.Grade
                };

                _context.Teachers.Add(teacher);
                _context.SaveChanges();
            }

            return RedirectToAction("Teachers");
        }

        [HttpPost]
        public IActionResult DeleteTeacher(int id)
        {
            var teacher = _context.Teachers.FirstOrDefault(_ => _.Id == id);

            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
                _context.SaveChanges();
            }

            return RedirectToAction("Teachers");
        }
    }
}
