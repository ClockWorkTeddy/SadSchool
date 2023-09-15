using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SadSchool.Models;
using SadSchool.ViewModels;
using SadSchool.Services;

namespace SadSchool.Controllers
{
    public class MarkController : Controller
    {
        private readonly SadSchoolContext _context;
        private readonly INavigationService _navigationService;

        public MarkController(SadSchoolContext context, INavigationService navigationService)
        {
            _context = context;
            _navigationService = navigationService;
        }

        [HttpGet]
        public IActionResult Marks()
        {
            var marks = new List<MarkViewModel>();

            foreach (var mark in _context.Marks
                .Include(m => m.Student)
                .Include(m => m.Lesson)
                .Include(m => m.Lesson.Subject)
                .Include(m => m.Lesson.StartTime))
            { 
                marks.Add(new MarkViewModel
                {
                    Id = mark.Id,
                    Value = mark.Value,
                    Student = $"{mark.Student?.FirstName} {mark.Student?.LastName}",
                    Lesson = $"{mark.Lesson?.Subject.Name} {mark.Lesson?.Date} {mark.Lesson?.StartTime?.Value}"
                });
            }

            _navigationService.RefreshBackParams(RouteData);

            return View(@"~/Views/Data/Marks.cshtml", marks);
        }

        [HttpGet]
        public IActionResult Add()
        {
            if (User.Identity.IsAuthenticated && !User.IsInRole("user"))
            {

                MarkViewModel viewModel = new MarkViewModel()
                {
                    Students = GetStudentsList(null),
                    Lessons = GetLessonsList(null),
                };

                _navigationService.RefreshBackParams(RouteData);

                return View(@"~/Views/Data/MarkAdd.cshtml", viewModel);
            }

            return RedirectToAction("Marks");
        }

        [HttpPost]
        public async Task<IActionResult> Add(MarkViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var mark = new Mark
                {
                    Value = viewModel.Value,
                    StudentId = viewModel.StudentId,
                    LessonId = viewModel.LessonId
                };

                _context.Marks.Add(mark);
                await _context.SaveChangesAsync();

                return RedirectToAction("Marks");
            }
            else
            {
                return View(@"~/Views/Data/MarkAdd.cshtml", viewModel);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (User.Identity.IsAuthenticated && !User.IsInRole("user"))
            {

                var editedMark = _context.Marks.Find(id);

                MarkViewModel viewModel = new()
                {
                    Value = editedMark?.Value,
                    Lessons = GetLessonsList(editedMark?.LessonId),
                    Students = GetStudentsList(editedMark?.StudentId)
                };

                _navigationService.RefreshBackParams(RouteData);

                return View(@"~/Views/Data/MarkEdit.cshtml", viewModel);
            }

            return RedirectToAction("Marks");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MarkViewModel viewModel)
        {
            if (ModelState.IsValid && viewModel != null)
            {
                var mark = new Mark
                {
                    Id = viewModel.Id,
                    Value = viewModel.Value,
                    LessonId = viewModel.LessonId,
                    StudentId = viewModel.StudentId,
                };

                _context.Marks.Update(mark);
                await _context.SaveChangesAsync();
                return RedirectToAction("Marks");
            }
            else
            {
                return View(@"~/Views/Data/MarkEdit.cshtml", viewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (User.Identity.IsAuthenticated && !User.IsInRole("user"))
            {
                var mark = await _context.Marks.FindAsync(id);

                if (mark != null)
                {
                    _context.Marks.Remove(mark);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Marks");
        }

        private List<SelectListItem> GetLessonsList(int? lessonId)
        {
            var lessons = _context.Lessons
                .Include(l => l.StartTime)
                .Include(l => l.Subject)
                .Include(l => l.Class).ToList();

            return lessons.Select(lesson => new SelectListItem
            {
                Value = lesson.Id.ToString(),
                Text = $"{lesson.StartTime.Value} {lesson.Subject.Name} {lesson.Class.Name} {lesson.Date}",
                Selected = lesson.Id == lessonId
            }).ToList();
        }

        private List<SelectListItem> GetStudentsList(int? studentId)
        {
            var students = _context.Students.ToList();

            return students.Select(student => new SelectListItem
            {
                Value = student.Id.ToString(),
                Text = $"{student.FirstName} {student.LastName}",
                Selected = student.Id == studentId
            }).ToList();
        }

    }
}
