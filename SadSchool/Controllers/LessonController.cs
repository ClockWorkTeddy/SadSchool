using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SadSchool.Models;
using SadSchool.ViewModels;
using SadSchool.Services;
using Microsoft.AspNetCore.Authorization;

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

            foreach (var lesson in _context.Lessons.Include(l => l.Teacher)
                                                   .Include(l => l.Subject)
                                                   .Include(l => l.Class)
                                                   .Include(l => l.StartTime).ToList())
            {
                lessons.Add(new LessonViewModel
                {
                    Id = lesson.Id,
                    StartTimeValue = lesson?.StartTime?.Value,
                    SubjectName = lesson?.Subject?.Name,
                    ClassName = lesson?.Class?.Name,
                    TeacherName = $"{lesson?.Teacher?.FirstName} {lesson?.Teacher?.LastName}",
                    Date = lesson?.Date
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
                    Classes = GetClassesList(null),
                    Subjects = GetSubjectsList(null),
                    Teachers = GetTeachersList(null),
                    StartTimes = GetStartTimesList(null)
                };

                _navigationService.RefreshBackParams(RouteData);

                return View(@"~/Views/Data/LessonAdd.cshtml", viewModel);
            }

            return RedirectToAction("Lessons");
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
        public async Task<IActionResult> Add(LessonViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var lesson = new Lesson
                {
                    ClassId = viewModel.ClassId,
                    SubjectId = viewModel.SubjectId,
                    TeacherId = viewModel.TeacherId,
                    StartTimeId = viewModel.StartTimeId,
                    Date = viewModel.Date
                };

                _context.Lessons.Add(lesson);
                await _context.SaveChangesAsync();
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
                    Date = editedLesson?.Date,
                    StartTimes = GetStartTimesList(editedLesson.StartTimeId),
                    Subjects = GetSubjectsList(editedLesson.SubjectId),
                    Teachers = GetTeachersList(editedLesson.TeacherId),
                    Classes = GetClassesList(editedLesson.ClassId)
                };

                _navigationService.RefreshBackParams(RouteData);

                return View(@"~/Views/Data/LessonEdit.cshtml", viewModel);
            }

            return RedirectToAction("Lessons");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(LessonViewModel viewModel)
        {
            if (ModelState.IsValid && viewModel != null)
            {
                var Lesson = new Lesson
                {
                    Id = viewModel.Id,
                    Date = viewModel.Date,
                    ClassId = viewModel.ClassId,
                    SubjectId = viewModel.SubjectId,
                    TeacherId = viewModel.TeacherId,
                    StartTimeId = viewModel.StartTimeId
                };

                _context.Lessons.Update(Lesson);
                await _context.SaveChangesAsync();

                return RedirectToAction("Lessons");
            }
            else
            {
                return View(@"~/Views/Data/LessonEdit.cshtml", viewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (User.Identity.IsAuthenticated && !User.IsInRole("user"))
            {
                var lesson = await _context.Lessons.FindAsync(id);

                if (lesson != null)
                {
                    _context.Lessons.Remove(lesson);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Lessons");
        }
    }
}
