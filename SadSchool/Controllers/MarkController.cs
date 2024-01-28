using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SadSchool.Models;
using SadSchool.ViewModels;
using SadSchool.Services;
using SadSchool.Services.ApiServices;
using Microsoft.Extensions.Caching.Memory;

namespace SadSchool.Controllers
{
    public class MarkController : Controller
    {
        private readonly SadSchoolContext _context;
        private readonly INavigationService _navigationService;
        private readonly IMarksAnalyticsService _marksAnalyticsService;
        private readonly IMemoryCache _memoryCache;
        public MarkController(
            SadSchoolContext context, 
            INavigationService navigationService, 
            IMarksAnalyticsService marksAnalyticsService,
            IMemoryCache memoryCache)
        {
            _context = context;
            _navigationService = navigationService;
            _marksAnalyticsService = marksAnalyticsService;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public IActionResult Marks()
        {
            var marks = new List<MarkViewModel>();

            foreach (var mark in _context.Marks
                .Include(m => m.Student)
                .Include(m => m.Lesson.ScheduledLesson.StartTime)
                .Include(m => m.Lesson.ScheduledLesson.Class)
                .Include(m => m.Lesson.ScheduledLesson.Subject))
            { 
                marks.Add(new MarkViewModel
                {
                    Id = mark.Id,
                    Value = mark.Value,
                    Student = $"{mark.Student?.FirstName} {mark.Student?.LastName}",
                    Lesson = $"{mark.Lesson}"
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
                .Include(l => l.ScheduledLesson.Class)
                .Include(l => l.ScheduledLesson.StartTime)
                .Include(l => l.ScheduledLesson.Subject)
                .ToList();

            return lessons.Select(lesson => new SelectListItem
            {
                Value = lesson.Id.ToString(),
                Text = $"{lesson.Date} {lesson.ScheduledLesson}",
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

        [HttpGet]
        public IActionResult GetStudentSubject()
        {
            StudentSubjectSelectorViewModel viewModel = new()
            {
                Students = GetStudents(),
                Subjects = GetSubjects()
            };

            _navigationService.RefreshBackParams(RouteData);

            return View(@"~/Views/Data/Representation/StudentSubjectSelector.cshtml", viewModel);
        }

        private List<SelectListItem> GetStudents()
        {
            var students = _context.Students.ToList();
            var studentsItemList = new List<SelectListItem>() 
            { 
                new SelectListItem { Value = 0.ToString(), Text = "", Selected = true } 
            };

            studentsItemList.AddRange(students.Select(student => new SelectListItem
            {
                Value = student.Id.ToString(),
                Text = student.ToString(),
                Selected = false
            }).ToList());

            return studentsItemList;
        }

        private List<SelectListItem> GetSubjects()
        {
            var subjects = _context.Subjects.ToList();
            var subjectsList = new List<SelectListItem>()
            {
                new SelectListItem { Value = 0.ToString(), Text = "", Selected = true }
            };

            subjectsList.AddRange(subjects.Select(subject => new SelectListItem
            {
                Value = subject.Id.ToString(),
                Text = subject.Name,
                Selected = false
            }).ToList());

            return subjectsList;
        }

        [HttpGet]
        public IActionResult GetAverageMarks(StudentSubjectSelectorViewModel viewModel)
        {
            var studentId = viewModel.SelectedStudentId;
            var subjectId = viewModel.SelectedSubjectId;

            var marks = _marksAnalyticsService.GetAverageMarks(studentId, subjectId);
            var students = marks.Select(m => m.StudentName).Distinct().Order().ToList();
            var subjects = marks.Select(m => m.SubjectName).Distinct().Order().ToList();

            var aveMarksTable = new AverageMark[students.Count, subjects.Count];

            for (int i = 0; i < students.Count; i++)
                for (int j = 0; j < subjects.Count; j++)
                    aveMarksTable[i, j] = marks.FirstOrDefault(m => m.StudentName == students[i] && m.SubjectName == subjects[j]);

            _navigationService.RefreshBackParams(RouteData);

            return View(@"~/Views/Data/Representation/AverageMarks.cshtml", new AverageMarksViewModel
            {
                AverageMarksTable = aveMarksTable,
                Subjects = subjects,
                Students = students
            });
        }

    }
}
