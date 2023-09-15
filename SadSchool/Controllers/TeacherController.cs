using Microsoft.AspNetCore.Mvc;
using SadSchool.Models;
using SadSchool.ViewModels;
using SadSchool.Services;

namespace SadSchool.Controllers
{
    public class TeacherController : Controller
    {
        private readonly SadSchoolContext _context;
        private readonly INavigationService _navigationService;

        public TeacherController(SadSchoolContext context, INavigationService navigationService)
        {
            _context = context;
            _navigationService = navigationService;
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
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    DateOfBirth = t.DateOfBirth?.ToString(),
                    Grade = t.Grade
                });
            }

            _navigationService.RefreshBackParams(RouteData);

            return View(@"~/Views/Data/Teachers.cshtml", teachers);
        }

        [HttpGet]
        public IActionResult Add()
        {
            _navigationService.RefreshBackParams(RouteData);

            return View(@"~/Views/Data/TeacherAdd.cshtml");
        }

        [HttpPost]
        public IActionResult Add(TeacherViewModel model)
        {
            if (ModelState.IsValid)
            {
                var teacher = new Teacher
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = model.DateOfBirth,
                    Grade = model.Grade
                };

                _context.Teachers.Add(teacher);
                _context.SaveChanges();
            }

            return RedirectToAction("Teachers");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var editedTeacher = _context.Teachers.FirstOrDefault(_ => _.Id == id);

            if (editedTeacher != null)
            {
                var model = new TeacherViewModel
                {
                    FirstName = editedTeacher.FirstName,
                    LastName = editedTeacher.LastName,
                    DateOfBirth = editedTeacher.DateOfBirth?.ToString(),
                    Grade = editedTeacher.Grade
                };

                _navigationService.RefreshBackParams(RouteData);

                return View(@"~/Views/Data/TeacherEdit.cshtml", model);
            }

            return RedirectToAction("Teachers");
        }

        [HttpPost]
        public async Task<IActionResult> Edit( TeacherViewModel viewModel )
        {
            if (ModelState.IsValid && viewModel != null)
            {
                var teacher = new Teacher
                {
                    Id = viewModel.Id,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    DateOfBirth = viewModel.DateOfBirth,
                    Grade = viewModel.Grade
                };

                _context.Teachers.Update(teacher);
                await _context.SaveChangesAsync();

                return RedirectToAction("Teachers");
            }
            else
            {
                return View(@"~/Views/Data/TeacherEdit.cshtml", viewModel);
            }
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
