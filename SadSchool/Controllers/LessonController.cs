// <copyright file="LessonController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using SadSchool.Contracts;
    using SadSchool.Contracts.Repositories;
    using SadSchool.Models.SqlServer;
    using SadSchool.Services;
    using SadSchool.ViewModels;

    /// <summary>
    /// Processes operations with <see cref="Lesson"/> entities.
    /// </summary>
    public class LessonController : Controller
    {
        private readonly ILessonRepository lessonRepository;
        private readonly IScheduledLessonRepository scheduledLessonRepository;
        private readonly INavigationService navigationService;
        private readonly IAuthService authService;
        private readonly ICommonMapper commonMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="LessonController"/> class.
        /// </summary>
        /// <param name="lessonRepository">DB context repository instance.</param>
        /// <param name="scheduledLessonRepository">Scheduled lesson repository instance.</param>
        /// <param name="navigationService">Service that operates "Back" button.</param>
        /// <param name="authService">Service that checks user authentication.</param>
        /// <param name="commonMapper">Service that maps entities.</param>
        public LessonController(
            ILessonRepository lessonRepository,
            IScheduledLessonRepository scheduledLessonRepository,
            INavigationService navigationService,
            IAuthService authService,
            ICommonMapper commonMapper)
        {
            this.lessonRepository = lessonRepository;
            this.scheduledLessonRepository = scheduledLessonRepository;
            this.navigationService = navigationService;
            this.authService = authService;
            this.commonMapper = commonMapper;
        }

        /// <summary>
        /// Gets lessons list view.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for lessons main page.</returns>
        [HttpGet]
        public async Task<IActionResult> Lessons()
        {
            var lessons = await this.lessonRepository.GetAllEntitiesAsync<Lesson>();

            var lessonViewModels = lessons.Select(lesson =>
            {
                var lessonVm = this.commonMapper.LessonToVm(lesson);
                lessonVm.LessonData = $"{lesson.ScheduledLesson}";

                return lessonVm;
            }).ToList();

            this.navigationService.RefreshBackParams(this.RouteData);

            return this.View(@"~/Views/Data/Lessons.cshtml", lessonViewModels);
        }

        /// <summary>
        /// Gets <see cref="Lesson"/> entity add-form.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for <see cref="Lesson"/> add form ot redirects to Lessons.</returns>
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            if (this.authService.IsAutorized(this.User))
            {
                LessonViewModel viewModel = new()
                {
                    ScheduledLessons = await this.GetScheduledLessonsList(null),
                };

                this.navigationService.RefreshBackParams(this.RouteData);

                return this.View(@"~/Views/Data/LessonAdd.cshtml", viewModel);
            }

            return this.RedirectToAction("Lessons");
        }

        /// <summary>
        /// Adds new <see cref="Lesson"/> entity to DB.
        /// </summary>
        /// <param name="viewModel"><see cref="LessonViewModel"/> with data about the lesson.</param>
        /// <returns><see cref="RedirectToActionResult"/> for Lessons view.</returns>
        [HttpPost]
        public async Task<IActionResult> Add(LessonViewModel viewModel)
        {
            if (this.ModelState.IsValid)
            {
                var lesson = this.commonMapper.LessonToModel(viewModel);

                await this.lessonRepository.AddEntityAsync(lesson);
            }

            return this.RedirectToAction("Lessons");
        }

        /// <summary>
        /// Gets <see cref="Lesson"/> entity edit-form.
        /// </summary>
        /// <param name="id">Edited lesson id.</param>
        /// <returns><see cref="ViewResult"/> for the entity-edit form or <see cref="RedirectToActionResult"/> for Lessons view.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (this.authService.IsAutorized(this.User) && this.ModelState.IsValid)
            {
                var editedLesson = await this.lessonRepository.GetEntityByIdAsync<Lesson>(id);

                if (editedLesson != null)
                {
                    LessonViewModel viewModel = this.commonMapper.LessonToVm(editedLesson);
                    viewModel.ScheduledLessons = await this.GetScheduledLessonsList(viewModel.ScheduledLessonId);

                    this.navigationService.RefreshBackParams(this.RouteData);

                    return this.View(@"~/Views/Data/LessonEdit.cshtml", viewModel);
                }
            }

            return this.RedirectToAction("Lessons");
        }

        /// <summary>
        /// Edits <see cref="Lesson"/> entity in DB.
        /// </summary>
        /// <param name="viewModel"><see cref="LessonViewModel"/> with new data.</param>
        /// <returns><see cref="RedirectToActionResult"/> for "Lessons" action or <see cref="ViewResult"/> for LessonEdit view.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(LessonViewModel viewModel)
        {
            if (this.ModelState.IsValid && viewModel != null)
            {
                var lesson = this.commonMapper.LessonToModel(viewModel);

                await this.lessonRepository.UpdateEntityAsync(lesson);

                return this.RedirectToAction("Lessons");
            }

            return this.View(@"~/Views/Data/LessonEdit.cshtml", viewModel);
        }

        /// <summary>
        /// Deletes <see cref="Lesson"/> entity from DB.
        /// </summary>
        /// <param name="id">Desirable instance id.</param>
        /// <returns><see cref="RedirectToActionResult"/> for action "Lessons".</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (this.authService.IsAutorized(this.User) && this.ModelState.IsValid)
            {
                await this.lessonRepository.DeleteEntityAsync<Lesson>(id);
            }

            return this.RedirectToAction("Lessons");
        }

        private static int GetDayOfWeekNumber(string? day)
        {
            return day switch
            {
                "Mon" => 1,
                "Tue" => 2,
                "Wed" => 3,
                "Thu" => 4,
                "Fri" => 5,
                _ => int.MaxValue, // Place unknown days at the end
            };
        }

        private async Task<List<SelectListItem>> GetScheduledLessonsList(int? lessonId)
        {
            var scheduledLessons = await this.scheduledLessonRepository.GetAllEntitiesAsync<ScheduledLesson>();

            return scheduledLessons
                .OrderBy(scheduledLesson => scheduledLesson?.Class?.Name, new MixedNumericStringComparer())
                .ThenBy(scheduledLesson => GetDayOfWeekNumber(scheduledLesson.Day))
                .Select(scheduledLesson => new SelectListItem
                {
                    Value = scheduledLesson.Id.ToString(),
                    Text = $"{scheduledLesson}",
                    Selected = lessonId?.ToString() == scheduledLesson.Id.ToString(),
                }).ToList();
        }
    }
}
