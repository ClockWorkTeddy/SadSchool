// <copyright file="ScheduledLessonController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using SadSchool.Contracts;
    using SadSchool.Contracts.Repositories;
    using SadSchool.Models.SqlServer;
    using SadSchool.ViewModels;

    /// <summary>
    /// Processes requests for scheduled lesson data.
    /// </summary>
    public class ScheduledLessonController : Controller
    {
        private readonly IScheduledLessonRepository scheduledLessonRepository;
        private readonly IClassRepository classRepository;
        private readonly INavigationService navigationService;
        private readonly IAuthService authService;
        private readonly ICacheService cacheService;
        private readonly IScheduledLessonMapper scheduledLessonMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledLessonController"/> class.
        /// </summary>
        /// <param name="scheduledLessonRepository">DB context repository instance.</param>
        /// <param name="navigationService">Service processes "Back" button.</param>
        /// <param name="authService">Service processes user authorization check.</param>
        /// <param name="cacheService">Service processes cache operations.</param>
        /// <param name="scheduledLessonMapper">Service processes mapping operations.</param>
        public ScheduledLessonController(
            IScheduledLessonRepository scheduledLessonRepository,
            INavigationService navigationService,
            IAuthService authService,
            ICacheService cacheService,
            IScheduledLessonMapper scheduledLessonMapper)
        {
            this.scheduledLessonRepository = scheduledLessonRepository;
            this.navigationService = navigationService;
            this.authService = authService;
            this.cacheService = cacheService;
            this.scheduledLessonMapper = scheduledLessonMapper;
        }

        /// <summary>
        /// Gets the scheduled lessons view.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for the Scheduled lessons form.</returns>
        [HttpGet]
        public async Task<IActionResult> ScheduledLessons()
        {
            var scheduledLessons = await this.scheduledLessonRepository.GetAllEntitiesAsync<ScheduledLesson>();
            var scheduledLessonsViewModels = scheduledLessons
                .Select(this.scheduledLessonMapper.ScheduledLessonToVm)
                .ToList();

            this.navigationService.RefreshBackParams(this.RouteData);

            return this.View(@"~/Views/Data/ScheduledLessons.cshtml", scheduledLessonsViewModels);
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
                var scheduledLesson = this.scheduledLessonMapper.ScheduledLessonToModel(viewModel);

                await this.scheduledLessonRepository.AddEntityAsync(scheduledLesson);
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
        public async Task<IActionResult> Edit(int id)
        {
            if (this.authService.IsAutorized(this.User))
            {
                var editedLesson = await this.scheduledLessonRepository.GetEntityByIdAsync<ScheduledLesson>(id);

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
                var scheduledLesson = this.scheduledLessonMapper.ScheduledLessonToModel(viewModel);

                await this.scheduledLessonRepository.UpdateEntityAsync(scheduledLesson);

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
                await this.scheduledLessonRepository.DeleteEntityAsync<ScheduledLesson>(id);
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
