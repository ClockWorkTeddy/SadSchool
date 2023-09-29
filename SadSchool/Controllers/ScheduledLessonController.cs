using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SadSchool.Models;
using SadSchool.ViewModels;
using SadSchool.Services;

namespace SadSchool.Controllers
{
    public class ScheduledLessonController : Controller
    {
        private readonly SadSchoolContext _context;
        private readonly INavigationService _navigationService;

        public ScheduledLessonController(SadSchoolContext context, INavigationService navigationService)
        {
            _context = context;
            _navigationService = navigationService;
        }

        [HttpGet]
        public IActionResult ScheduledLessons()
        {
            var lessons = new List<ScheduledLessonViewModel>();

            foreach (var lesson in _context.ScheduledLessons.Include(l => l.Teacher)
                                                   .Include(l => l.Subject)
                                                   .Include(l => l.Class)
                                                   .Include(l => l.StartTime).ToList())
            {
                lessons.Add(new ScheduledLessonViewModel
                {
                    Id = lesson.Id,
                    StartTimeValue = lesson?.StartTime?.Value,
                    SubjectName = lesson?.Subject?.Name,
                    ClassName = lesson?.Class?.Name,
                    TeacherName = $"{lesson?.Teacher?.FirstName} {lesson?.Teacher?.LastName}",
                    Day = lesson?.Day
                });
            }

            _navigationService.RefreshBackParams(RouteData);

            return View(@"~/Views/Data/ScheduledLessons.cshtml", lessons);
        }

        [HttpGet]
        public IActionResult Add()
        {
            if (User.Identity.IsAuthenticated && !User.IsInRole("user"))
            {
                ScheduledLessonViewModel viewModel = new()
                {
                    Classes = GetClassesList(null),
                    Subjects = GetSubjectsList(null),
                    Teachers = GetTeachersList(null),
                    StartTimes = GetStartTimesList(null)
                };

                _navigationService.RefreshBackParams(RouteData);

                return View(@"~/Views/Data/ScheduledLessonAdd.cshtml", viewModel);
            }

            return RedirectToAction("ScheduledLessons");
        }

        private List<SelectListItem> GetClassesList(int? classId)
        {
            var classes = _context.Classes.ToList();

            return classes.Select(theClass => new SelectListItem
            {
                Value = theClass.Id.ToString(),
                Text = theClass.Name,
                Selected = theClass.Id == classId
            }).ToList();
        }

        private List<SelectListItem> GetSubjectsList(int? subjectId)
        {
            var subjects = _context.Subjects.ToList();

            return subjects.Select(subject => new SelectListItem
            {
                Value = subject.Id.ToString(),
                Text = subject.Name,
                Selected = subject.Id == subjectId
            }).ToList();
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

        private List<SelectListItem> GetStartTimesList(int? startId)
        {
            var starts = _context.StartTimes.ToList();

            return starts.Select(start => new SelectListItem
            {
                Value = start.Id.ToString(),
                Text = start.Value,
                Selected = start.Id == startId
            }).ToList();
        }

        [HttpPost]
        public async Task<IActionResult> Add(ScheduledLessonViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var lesson = new ScheduledLesson
                {
                    ClassId = viewModel.ClassId,
                    SubjectId = viewModel.SubjectId,
                    TeacherId = viewModel.TeacherId,
                    StartTimeId = viewModel.StartTimeId,
                    Day = viewModel.Day
                };

                _context.ScheduledLessons.Add(lesson);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ScheduledLessons");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (User.Identity.IsAuthenticated && !User.IsInRole("user"))
            {
                var editedLesson = _context.ScheduledLessons.Find(id);

                ScheduledLessonViewModel viewModel = new()
                {
                    Day = editedLesson?.Day,
                    StartTimes = GetStartTimesList(editedLesson.StartTimeId),
                    Subjects = GetSubjectsList(editedLesson.SubjectId),
                    Teachers = GetTeachersList(editedLesson.TeacherId),
                    Classes = GetClassesList(editedLesson.ClassId)
                };

                _navigationService.RefreshBackParams(RouteData);

                return View(@"~/Views/Data/ScheduledLessonEdit.cshtml", viewModel);
            }

            return RedirectToAction("ScheduledLessons");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ScheduledLessonViewModel viewModel)
        {
            if (ModelState.IsValid && viewModel != null)
            {
                var Lesson = new ScheduledLesson
                {
                    Id = viewModel.Id,
                    Day = viewModel.Day,
                    ClassId = viewModel.ClassId,
                    SubjectId = viewModel.SubjectId,
                    TeacherId = viewModel.TeacherId,
                    StartTimeId = viewModel.StartTimeId
                };

                _context.ScheduledLessons.Update(Lesson);
                await _context.SaveChangesAsync();

                return RedirectToAction("Lessons");
            }

            return View(@"~/Views/Data/ScheduledLessonEdit.cshtml", viewModel);
            
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (User.Identity.IsAuthenticated && !User.IsInRole("user"))
            {
                var lesson = await _context.ScheduledLessons.FindAsync(id);

                if (lesson != null)
                {
                    _context.ScheduledLessons.Remove(lesson);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("ScheduledLessons");
        }
    }
}
