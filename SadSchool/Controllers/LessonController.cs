using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SadSchool.Models;
using SadSchool.ViewModels;

namespace SadSchool.Controllers
{
    public class LessonController : Controller
    {
        private readonly SadSchoolContext _context;

        public LessonController(SadSchoolContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Lessons()
        {
            var lessons = new List<LessonViewModel>();

            foreach (var lesson in _context.Lessons.Include(l => l.Teacher)
                                                   .Include(l => l.Subject)
                                                   .Include(l => l.Class)
                                                   .Include(l => l.ScheduledPosition).ToList())
            {
                lessons.Add(new LessonViewModel
                {
                    Id = lesson.Id,
                    Starts = lesson?.ScheduledPosition.Value,
                    Subject = lesson?.Subject?.Name,
                    Class = lesson?.Class?.Name,
                    Teacher = $"{lesson.Teacher?.FirstName} {lesson.Teacher?.LastName}",
                    Date = lesson.Date
                });
            }
            return View(@"~/Views/Data/Lessons.cshtml", lessons);
        }

        [HttpGet]
        public IActionResult AddLesson()
        {
            LessonAddViewModel viewModel = new LessonAddViewModel()
            {
                ClassesForView = _context.Classes.ToList(),
                SubjectsForView = _context.Subjects.ToList(),
                TeachersForView = _context.Teachers.ToList(),
                SchedulesForView = _context.SchedulePositions.ToList()
            };

            return View(@"~/Views/Data/LessonAdd.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddLesson(LessonAddViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var lesson = new Lesson
                {
                    ClassId = viewModel.ClassId,
                    Class = _context.Classes.Find(viewModel.ClassId),
                    SubjectId = viewModel.SubjectId,
                    Subject = _context.Subjects.Find(viewModel.SubjectId),
                    TeacherId = viewModel.TeacherId,
                    Teacher = _context.Teachers.Find(viewModel.TeacherId),
                    StartTimeId = viewModel.ScheduleId,
                    ScheduledPosition = _context.SchedulePositions.Find(viewModel.ScheduleId),
                    Date = viewModel.Date
                };

                _context.Lessons.Add(lesson);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Lessons");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);

            if (lesson != null)
            {
                _context.Lessons.Remove(lesson);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Lessons");
        }
    }
}
