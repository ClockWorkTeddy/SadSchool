// <copyright file="MarkController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;
    using SadSchool.Controllers.Contracts;
    using SadSchool.Models;
    using SadSchool.Services;
    using SadSchool.Services.ApiServices;
    using SadSchool.ViewModels;

    /// <summary>
    /// Processes <see cref="Mark"/> entities.
    /// </summary>
    public class MarkController : Controller
    {
        private readonly SadSchoolContext context;
        private readonly INavigationService navigationService;
        private readonly IMarksAnalyticsService marksAnalyticsService;
        private readonly IAuthService authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkController"/> class.
        /// </summary>
        /// <param name="context">DB context.</param>
        /// <param name="navigationService">Service instance processing Back button logic.</param>
        /// <param name="marksAnalyticsService">Service operates marks analytics.</param>
        /// <param name="authService">Authentication check service instance.</param>
        public MarkController(
            SadSchoolContext context,
            INavigationService navigationService,
            IMarksAnalyticsService marksAnalyticsService,
            IAuthService authService)
        {
            this.context = context;
            this.navigationService = navigationService;
            this.marksAnalyticsService = marksAnalyticsService;
            this.authService = authService;
        }

        /// <summary>
        /// Gets marks view.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for marks view.</returns>
        [HttpGet]
        public IActionResult Marks()
        {
            var marks = new List<MarkViewModel>();

            foreach (var mark in this.context.Marks)
            {
                marks.Add(new MarkViewModel
                {
                    Id = mark.Id,
                    Value = mark.Value,
                    Student = $"{mark.Student?.FirstName} {mark.Student?.LastName}",
                    Lesson = $"{mark.Lesson}",
                });
            }

            this.navigationService.RefreshBackParams(this.RouteData);

            return this.View(@"~/Views/Data/Marks.cshtml", marks);
        }

        /// <summary>
        /// Gets add mark view.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for "Add" form or
        ///     <see cref="RedirectToActionResult"/> for action "Marks" in case of failure.</returns>
        [HttpGet]
        public IActionResult Add()
        {
            if (this.authService.IsAutorized(this.User))
            {
                MarkViewModel viewModel = new MarkViewModel()
                {
                    Students = this.GetStudentsList(null),
                    Lessons = this.GetLessonsList(null),
                };

                this.navigationService.RefreshBackParams(this.RouteData);

                return this.View(@"~/Views/Data/MarkAdd.cshtml", viewModel);
            }

            return this.RedirectToAction("Marks");
        }

        /// <summary>
        /// Adds mark insstance to DB.
        /// </summary>
        /// <param name="viewModel"><see cref="MarkViewModel"/> instance with data.</param>
        /// <returns><see cref="RedirectToActionResult"/> to action "Marks" or
        /// <see cref="ViewResult"/> for "Add" form in case of failure.</returns>
        [HttpPost]
        public async Task<IActionResult> Add(MarkViewModel viewModel)
        {
            if (this.ModelState.IsValid)
            {
                var mark = new Mark
                {
                    Value = viewModel.Value,
                    StudentId = viewModel.StudentId,
                    LessonId = viewModel.LessonId,
                };

                this.context.Marks.Add(mark);
                await this.context.SaveChangesAsync();

                return this.RedirectToAction("Marks");
            }
            else
            {
                return this.View(@"~/Views/Data/MarkAdd.cshtml", viewModel);
            }
        }

        /// <summary>
        /// Gets edit mark view.
        /// </summary>
        /// <param name="id">Desiarable <see cref="Mark"/> id.</param>
        /// <returns><see cref="ViewResult"/> for "Add" form or
        ///     <see cref="RedirectToActionResult"/> for action "Marks" in case of failure.</returns>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (this.authService.IsAutorized(this.User))
            {
                var editedMark = this.context.Marks.Find(id);

                MarkViewModel viewModel = new()
                {
                    Value = editedMark!.Value,
                    Lessons = this.GetLessonsList(editedMark?.LessonId),
                    Students = this.GetStudentsList(editedMark?.StudentId),
                };

                this.navigationService.RefreshBackParams(this.RouteData);

                return this.View(@"~/Views/Data/MarkEdit.cshtml", viewModel);
            }

            return this.RedirectToAction("Marks");
        }

        /// <summary>
        /// Edits mark insstance in DB.
        /// </summary>
        /// <param name="viewModel"><see cref="MarkViewModel"/> istance with data.</param>
        /// <returns><see cref="RedirectToActionResult"/> to action "Marks" or
        ///     <see cref="ViewResult"/> for "Edit" form in case of failure.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(MarkViewModel viewModel)
        {
            if (this.authService.IsAutorized(this.User))
            {
                var mark = new Mark
                {
                    Id = viewModel.Id,
                    Value = viewModel.Value,
                    LessonId = viewModel.LessonId,
                    StudentId = viewModel.StudentId,
                };

                this.context.Marks.Update(mark);
                await this.context.SaveChangesAsync();

                return this.RedirectToAction("Marks");
            }
            else
            {
                return this.View(@"~/Views/Data/MarkEdit.cshtml", viewModel);
            }
        }

        /// <summary>
        /// Deletes mark instance from DB.
        /// </summary>
        /// <param name="id">Desiarable <see cref="Mark"/> id.</param>
        /// <returns><see cref="RedirectToActionResult"/> to action "Marks".</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (this.authService.IsAutorized(this.User))
            {
                var mark = await this.context.Marks.FindAsync(id);

                if (mark != null)
                {
                    this.context.Marks.Remove(mark);
                    await this.context.SaveChangesAsync();
                }
            }

            return this.RedirectToAction("Marks");
        }

        /// <summary>
        /// Gets student-subject selector view.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for "StudentSubjectSelector" view.</returns>
        [HttpGet]
        public IActionResult GetStudentSubject()
        {
            StudentSubjectSelectorViewModel viewModel = new()
            {
                Students = this.GetStudents(),
                Subjects = this.GetSubjects(),
            };

            this.navigationService.RefreshBackParams(this.RouteData);

            return this.View(@"~/Views/Data/Representation/StudentSubjectSelector.cshtml", viewModel);
        }

        /// <summary>
        /// Gets average marks view.
        /// </summary>
        /// <param name="viewModel"><see cref="StudentSubjectSelectorViewModel"/> instance with data.</param>
        /// <returns><see cref="ViewResult"/> for "AverageMarks" view.</returns>
        [HttpGet]
        public IActionResult GetAverageMarks(StudentSubjectSelectorViewModel viewModel)
        {
            var studentId = viewModel.SelectedStudentId;
            var subjectId = viewModel.SelectedSubjectId;

            var marks = this.marksAnalyticsService.GetAverageMarks(studentId, subjectId);
            var students = marks.Select(m => m.StudentName).Distinct().Order().ToList();
            var subjects = marks.Select(m => m.SubjectName).Distinct().Order().ToList();

            var aveMarksTable = new AverageMark?[students.Count, subjects.Count];

            for (int i = 0; i < students.Count; i++)
            {
                for (int j = 0; j < subjects.Count; j++)
                {
                    aveMarksTable[i, j] = marks.FirstOrDefault(m => m!.StudentName == students![i] && m!.SubjectName == subjects![j]);
                }
            }

            this.navigationService.RefreshBackParams(this.RouteData);

            return this.View(@"~/Views/Data/Representation/AverageMarks.cshtml", new AverageMarksViewModel
            {
                AverageMarksTable = aveMarksTable,
                Subjects = subjects,
                Students = students,
            });
        }

        private List<SelectListItem> GetLessonsList(int? lessonId)
        {
            var lessons = this.context.Lessons.ToList();

            return lessons.Select(lesson => new SelectListItem
            {
                Value = lesson.Id.ToString(),
                Text = $"{lesson.Date} {lesson.ScheduledLesson}",
                Selected = lesson.Id == lessonId,
            }).ToList();
        }

        private List<SelectListItem> GetStudentsList(int? studentId)
        {
            var students = this.context.Students.ToList();

            return students.Select(student => new SelectListItem
            {
                Value = student.Id.ToString(),
                Text = $"{student.FirstName} {student.LastName}",
                Selected = student.Id == studentId,
            }).ToList();
        }

        private List<SelectListItem> GetStudents()
        {
            var students = this.context.Students.ToList();
            var studentsItemList = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Value = 0.ToString(),
                    Text = string.Empty,
                    Selected = true,
                },
            };

            studentsItemList.AddRange(students.Select(student => new SelectListItem
            {
                Value = student.Id.ToString(),
                Text = student.ToString(),
                Selected = false,
            }).ToList());

            return studentsItemList;
        }

        private List<SelectListItem> GetSubjects()
        {
            var subjects = this.context.Subjects.ToList();
            var subjectsList = new List<SelectListItem>()
            {
                new SelectListItem { Value = 0.ToString(), Text = string.Empty, Selected = true },
            };

            subjectsList.AddRange(subjects.Select(subject => new SelectListItem
            {
                Value = subject.Id.ToString(),
                Text = subject.Name,
                Selected = false,
            }).ToList());

            return subjectsList;
        }
    }
}
