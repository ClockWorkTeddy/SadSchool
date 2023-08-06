using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SadSchool.Models;
using SadSchool.ViewModels;

namespace SadSchool.Controllers
{
    public class StudentController : Controller
    {
        private readonly SadSchoolContext _context;

        public StudentController(SadSchoolContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Students()
        {
            var students = new List<StudentViewModel>();

            foreach (var student in _context.Students.Include(s => s.Class).ToList())
            {
                students.Add(new StudentViewModel
                {
                    Id = student.Id,
                    FirstName = student.Name,
                    LastName = student.LastName,
                    DateOfBirth = student.DateOfBirth,
                    Sex = student.Sex.Value ? "Female" : "Male",
                    ClassName = student.Class?.Name
                });
            }
            return View(@"~/Views/Data/Students.cshtml", students);
        }

        [HttpGet]
        public IActionResult AddStudent()
        {
            StudentAddViewModel viewModel = new StudentAddViewModel()
            {
                ClassesForView = _context.Classes.ToList()
            };
            return View(@"~/Views/Data/StudentAdd.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent(StudentAddViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var student = new Student
                {
                    Name = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    ClassId = viewModel.ClassId,
                    Class = _context.Classes.Find(viewModel.ClassId),
                    DateOfBirth = viewModel.DateOfBirth,
                    Sex = viewModel.Sex == "Male" ? false : true
                };

                _context.Students.Add(student);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Students");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Students");
        }
    }
}
