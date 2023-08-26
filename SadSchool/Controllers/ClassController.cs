using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

            foreach (var theClass in _context.Classes.Include(c => c.Teacher).ToList())
                classes.Add(new ClassViewModel
                {
                    Id = theClass.Id,
                    Name = theClass.Name,
                    TeacherId = theClass.TeacherId,
                    TeacherName = GetTeacherName(theClass.TeacherId),
                    LeaderName = GetLeaderName(theClass.LeaderId)
                });

            return View(@"~/Views/Data/Classes.cshtml", classes);
        }

        private string GetLeaderName(int? leaderId)
        {
            var leader = _context.Students.Find(leaderId);

            return $"{leader?.FirstName} {leader?.LastName}";
        }

        private string GetTeacherName(int? teacherId)
        {
            var teacher = _context.Teachers.Find(teacherId);

            return $"{teacher?.FirstName} {teacher?.LastName}";
        }

        [HttpGet]
        public IActionResult AddClass()
        {
            ClassViewModel viewModel = new ClassViewModel() { Teachers = GetTeachersList(null) };

            return View(@"~/Views/Data/ClassAdd.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddClass(ClassViewModel viewModel)                                         
        {
            if (ModelState.IsValid)
            {
                var Class = new Class
                {
                    Name = viewModel.Name,
                    TeacherId = viewModel.TeacherId,
                };

                _context.Classes.Add(Class);
                await _context.SaveChangesAsync();

                return RedirectToAction("Classes");
            }
            else
            {
                return View(@"~/Views/Data/ClassAdd.cshtml", viewModel);
            }
        }

        [HttpGet]
        public IActionResult EditClass(int id)
        {
            var editedClass = _context.Classes.Find(id);

            ClassViewModel viewModel = new()
            {
                Name = editedClass?.Name,
                Teachers = GetTeachersList(editedClass?.TeacherId)
            };

            return View(@"~/Views/Data/ClassEdit.cshtml", viewModel);
        }

        private List<SelectListItem> GetTeachersList(int? teacherId)
        {
            var teachers = _context.Teachers.ToList();

            return teachers.Select(teacher => new SelectListItem
            {
                Value = teacher.Id.ToString(),
                Text = $"{teacher.FirstName} {teacher.LastName}",
                Selected = teacher.Id == teacherId
            }).ToList();
        }

        [HttpPost]
        public async Task<IActionResult> EditClass(ClassViewModel viewModel)
        {
            if (ModelState.IsValid && viewModel != null)
            {
                var Class = new Class
                {
                    Id = viewModel.Id.Value,
                    Name = viewModel.Name,
                    TeacherId = viewModel.TeacherId,
                };

                _context.Classes.Update(Class);
                await _context.SaveChangesAsync();
                return RedirectToAction("Classes");
            }
            else
            {
                return View(@"~/Views/Data/ClassEdit.cshtml", viewModel);
            }
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
