// <copyright file="ScheduledLessonController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using SadSchool.Controllers.Contracts;
    using SadSchool.Models;
    using SadSchool.Services;
    using SadSchool.ViewModels;

    /// <summary>
    /// Processes requests for scheduled lesson data.
    /// </summary>
    public class ScheduledLessonController : Controller
    {
        private readonly SadSchoolContext context;
        private readonly INavigationService navigationService;
        private readonly IAuthService authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledLessonController"/> class.
        /// </summary>
        /// <param name="context">DB context instance.</param>
        /// <param name="navigationService">Service processes "Back" button.</param>
        /// <param name="authService">Service processes user authorization check.</param>
        public ScheduledLessonController(
            SadSchoolContext context,
            INavigationService navigationService,
            IAuthService authService)
        {
            this.context = context;
            this.navigationService = navigationService;
            this.authService = authService;
        }

        /// <summary>
        /// Gets the scheduled lessons view.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for the Scheduled lessons form.</returns>
        [HttpGet]
        public IActionResult ScheduledLessons()
        {
            var lessons = new List<ScheduledLessonViewModel>();

            foreach (var lesson in this.context.ScheduledLessons.ToList())
            {
                lessons.Add(new ScheduledLessonViewModel
                {
                    Id = lesson.Id,
                    StartTimeValue = lesson?.StartTime?.Value,
                    SubjectName = lesson?.Subject?.Name,
                    ClassName = lesson?.Class?.Name,
                    TeacherName = $"{lesson?.Teacher?.FirstName} {lesson?.Teacher?.LastName}",
                    Day = lesson?.Day,
                });
            }

            this.navigationService.RefreshBackParams(this.RouteData);

            return this.View(@"~/Views/Data/ScheduledLessons.cshtml", lessons);
        }

        /// <summary>
        /// Gets the add scheduled lesson view.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for the Scheduled lesson add form or
        ///     <see cref="RedirectToActionResult"/> for the "ScheduledLessons" actiion.</returns>
        [HttpGet]
        public IActionResult Add()
        {
            if (this.authService.IsAutorized(this.User))
            {
                ScheduledLessonViewModel viewModel = new()
                {
                    Classes = this.GetClassesList(null),
                    Subjects = this.GetSubjectsList(null),
                    Teachers = this.GetTeachersList(null),
                    StartTimes = this.GetStartTimesList(null),
                };

                this.navigationService.RefreshBackParams(this.RouteData);

                return this.View(@"~/Views/Data/ScheduledLessonAdd.cshtml", viewModel);
            }

            return this.RedirectToAction("ScheduledLessons");
        }

        /// <summary>
        /// Adds a new scheduled lesson.
        /// </summary>
        /// <param name="viewModel">View model instance with data.</param>
        /// <returns><see cref="RedirectToActionResult"/> for the "ScheduledLessons" action.</returns>
        [HttpPost]
        public async Task<IActionResult> Add(ScheduledLessonViewModel viewModel)
        {
            if (this.ModelState.IsValid)
            {
                var lesson = new ScheduledLesson
                {
                    ClassId = viewModel.ClassId,
                    SubjectId = viewModel.SubjectId,
                    TeacherId = viewModel.TeacherId,
                    StartTimeId = viewModel.StartTimeId,
                    Day = viewModel.Day,
                };

                this.context.ScheduledLessons.Add(lesson);
                await this.context.SaveChangesAsync();
            }

            return this.RedirectToAction("ScheduledLessons");
        }

        /// <summary>
        /// Gets the edit scheduled lesson view.
        /// </summary>
        /// <param name="id">Desirable <see cref="Lesson"/> id.</param>
        /// <returns><see cref="ViewResult"/> for "ScheduledLessonEdit" view or
        ///     <see cref="RedirectToActionResult"/> for the "ScheduledLessons" action in case of failure.</returns>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (this.authService.IsAutorized(this.User))
            {
                var editedLesson = this.context.ScheduledLessons.Find(id);

                ScheduledLessonViewModel viewModel = new()
                {
                    Day = editedLesson?.Day,
                    StartTimes = this.GetStartTimesList(editedLesson?.StartTimeId),
                    Subjects = this.GetSubjectsList(editedLesson?.SubjectId),
                    Teachers = this.GetTeachersList(editedLesson?.TeacherId),
                    Classes = this.GetClassesList(editedLesson?.ClassId),
                };

                this.navigationService.RefreshBackParams(this.RouteData);

                return this.View(@"~/Views/Data/ScheduledLessonEdit.cshtml", viewModel);
            }

            return this.RedirectToAction("ScheduledLessons");
        }

        /// <summary>
        /// Edits a scheduled lesson.
        /// </summary>
        /// <param name="viewModel">ViewModel instance with data.</param>
        /// <returns><see cref="RedirectToActionResult"/> for the "ScheduledLessons" action or
        ///     <see cref="ViewResult"/> for "ScheduledLessonEdit" view in case of failure.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(ScheduledLessonViewModel viewModel)
        {
            if (this.authService.IsAutorized(this.User))
            {
                var lesson = new ScheduledLesson
                {
                    Id = viewModel.Id,
                    Day = viewModel.Day,
                    ClassId = viewModel.ClassId,
                    SubjectId = viewModel.SubjectId,
                    TeacherId = viewModel.TeacherId,
                    StartTimeId = viewModel.StartTimeId,
                };

                this.context.ScheduledLessons.Update(lesson);
                await this.context.SaveChangesAsync();

                return this.RedirectToAction("ScheduledLessons");
            }

            return this.View(@"~/Views/Data/ScheduledLessonEdit.cshtml", viewModel);
        }

        /// <summary>
        /// Delets a scheduled lesson.
        /// </summary>
        /// <param name="id">Deleted lesson id.</param>
        /// <returns><see cref="RedirectToActionResult"/> for the "ScheduledLessons" action.</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (this.authService.IsAutorized(this.User))
            {
                var lesson = await this.context.ScheduledLessons.FindAsync(id);

                if (lesson != null)
                {
                    this.context.ScheduledLessons.Remove(lesson);
                    await this.context.SaveChangesAsync();
                }
            }

            return this.RedirectToAction("ScheduledLessons");
        }

        private List<SelectListItem> GetClassesList(int? classId)
        {
            var classes = this.context.Classes.ToList();

            return classes.Select(theClass => new SelectListItem
            {
                Value = theClass.Id.ToString(),
                Text = theClass.Name,
                Selected = theClass.Id == classId,
            }).ToList();
        }

        private List<SelectListItem> GetSubjectsList(int? subjectId)
        {
            var subjects = this.context.Subjects.ToList();

            return subjects.Select(subject => new SelectListItem
            {
                Value = subject.Id.ToString(),
                Text = subject.Name,
                Selected = subject.Id == subjectId,
            }).ToList();
        }

        private List<SelectListItem> GetTeachersList(int? teacherId)
        {
            var teachers = this.context.Teachers.ToList();

            return teachers.Select(teacher => new SelectListItem
            {
                Value = teacher.Id.ToString(),
                Text = $"{teacher.FirstName} {teacher.LastName}",
                Selected = teacher.Id == teacherId,
            }).ToList();
        }

        private List<SelectListItem> GetStartTimesList(int? startId)
        {
            var starts = this.context.StartTimes.ToList();

            return starts.Select(start => new SelectListItem
            {
                Value = start.Id.ToString(),
                Text = start.Value,
                Selected = start.Id == startId,
            }).ToList();
        }
    }
}
