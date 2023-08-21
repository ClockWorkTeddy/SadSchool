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
            ClassAddViewModel viewModel = new ClassAddViewModel() { Teachers = GetTeachersList(null) };

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

            ClassAddViewModel viewModel = new ClassAddViewModel()
            {
                Id = editedClass?.Id,
                TeacherId = _context.Teachers.Find(editedClass?.TeacherId)?.Id,
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
        public async Task<IActionResult> EditClass(ClassAddViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var Class = new Class
                {
                    Id = viewModel.Id.Value,
                    Name = viewModel.Name,
                    TeacherId = viewModel.TeacherId,
                    Teacher = _context.Teachers.Find(viewModel.TeacherId)
                };

                _context.Classes.Update(Class);
                await _context.SaveChangesAsync();
                return RedirectToAction("Classes");
            }
            else
            {
                return View(@"~/Views/Data/ClassAdd.cshtml", viewModel);
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
