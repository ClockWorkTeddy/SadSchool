using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                    FirstName = student.FirstName,
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
            StudentAddViewModel viewModel = new StudentAddViewModel() { Classes = GetClassesList(null) };

            return View(@"~/Views/Data/StudentAdd.cshtml", viewModel);
        }

        private List<SelectListItem> GetClassesList(int? classId)
        {
            var classes = _context.Classes.ToList();

            return classes.Select(Class => new SelectListItem
            {
                Value = Class.Id.ToString(),
                Text = $"{Class.Name}",
                Selected = Class.Id == classId
            }).ToList();
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent(StudentAddViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var student = new Student
                {
                    FirstName = viewModel.FirstName,
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

        [HttpGet]
        public IActionResult EditStudent(int id)
        {
            var editedStudent = _context.Students.Find(id);

            StudentAddViewModel viewModel = new()
            {
                 Id = editedStudent?.Id,
                 ClassId = _context.Classes.Find(editedStudent?.ClassId)?.Id,
                 FirstName = editedStudent?.FirstName,
                 LastName = editedStudent?.LastName,
                 Sex = editedStudent?.Sex == null 
                    ? "null" 
                    : editedStudent.Sex.Value ? "Male" : "Female",
                 DateOfBirth = editedStudent?.DateOfBirth
            };

            return View(@"~/Views/Data/StudentEdit.cshtml", viewModel);
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
