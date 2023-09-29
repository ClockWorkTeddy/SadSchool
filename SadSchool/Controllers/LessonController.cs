using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SadSchool.Models;
using SadSchool.Services;
using SadSchool.ViewModels;

namespace SadSchool.Controllers
{
    public class LessonController : Controller
    {
        private readonly SadSchoolContext _context;
        private readonly INavigationService _navigationService;

        public LessonController(SadSchoolContext context, INavigationService navigationService)
        {
            _context = context;
            _navigationService = navigationService;
        }

        [HttpGet]
        public IActionResult Lessons()
        {
            var lessons = new List<LessonViewModel>();

            foreach (var lesson in _context.Lessons
                .Include(l => l.ScheduledLesson.Class)
                .Include(l => l.ScheduledLesson.Teacher)
                .Include(l => l.ScheduledLesson.StartTime)
                .Include(l => l.ScheduledLesson.Subject))
            {
                lessons.Add(new LessonViewModel
                {
                    Id = lesson.Id,
                    Date = lesson?.Date,
                    LessonData = $"{lesson?.ScheduledLesson?.Day} " +
                                 $"{lesson?.ScheduledLesson?.StartTime?.Value} " + 
                                 $"{lesson?.ScheduledLesson?.Subject?.Name} " +
                                 $"{lesson?.ScheduledLesson?.Class?.Name} " +
                                 $"{lesson?.ScheduledLesson?.Teacher?.FirstName} " +
                                 $"{lesson?.ScheduledLesson?.Teacher?.LastName} "
                });
            }

            _navigationService.RefreshBackParams(RouteData);

            return View(@"~/Views/Data/Lessons.cshtml", lessons);
        }

        [HttpGet]
        public IActionResult Add()
        {
            if (User.Identity.IsAuthenticated && !User.IsInRole("user"))
            {
                LessonViewModel viewModel = new()
                {
                    ScheduledLessons = GetScheduledLessonsList(null)
                };

                _navigationService.RefreshBackParams(RouteData);

                return View(@"~/Views/Data/LessonAdd.cshtml", viewModel);
            }

            return RedirectToAction("Lessons");
        }

        private List<SelectListItem> GetScheduledLessonsList(int? lessonId)
        {
            var scheduledLessons = _context.ScheduledLessons
                .Include(sl => sl.Subject)
                .Include(sl => sl.Class)
                .Include(sl => sl.Teacher)
                .Include(sl => sl.StartTime).ToList();

            return scheduledLessons.Select(scheduledLesson => new SelectListItem
            {
                Value = scheduledLesson.Id.ToString(),
                Text = $"{scheduledLesson.Subject?.Name} " +
                       $"{scheduledLesson.Class?.Name} " +
                       $"{scheduledLesson.Teacher?.FirstName} " +
                       $"{scheduledLesson.Teacher?.LastName} " +
                       $"{scheduledLesson.StartTime?.Value}",
                Selected = lessonId?.ToString() == scheduledLesson.Id.ToString()
            }).ToList();
        }

        [HttpPost]
        public IActionResult Add(LessonViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var lesson = new Lesson
                {
                    Date = viewModel.Date,
                    ScheduledLessonId = viewModel.ScheduledLessonId,
                };

                _context.Lessons.Add(lesson);
                _context.SaveChanges();
            }

            return RedirectToAction("Lessons");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (User.Identity.IsAuthenticated && !User.IsInRole("user"))
            {
                var editedLesson = _context.Lessons.Find(id);

                LessonViewModel viewModel = new()
                {
                    Id = editedLesson.Id,
                    Date = editedLesson.Date,
                    ScheduledLessons = GetScheduledLessonsList(editedLesson.ScheduledLessonId)
                };

                _navigationService.RefreshBackParams(RouteData);

                return View(@"~/Views/Data/LessonEdit.cshtml", viewModel);
            }

            return RedirectToAction("Lessons");
        }

        [HttpPost]
        public IActionResult Edit(LessonViewModel viewModel)
        {
            if (ModelState.IsValid && viewModel != null)
            {
                var lesson = new Lesson
                {
                    Id = viewModel.Id,
                    Date = viewModel.Date,
                    ScheduledLessonId = viewModel.ScheduledLessonId
                };

                _context.Lessons.Update(lesson);
                _context.SaveChanges();

                return RedirectToAction("Lessons");
            }

            return View(@"~/Views/Data/LessonEdit.cshtml", viewModel);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (User.Identity.IsAuthenticated && !User.IsInRole("user"))
            {
                var lesson = _context.Lessons.Find(id);

                if (lesson != null)
                {
                    _context.Lessons.Remove(lesson);
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Lessons");
        }
    }
}
