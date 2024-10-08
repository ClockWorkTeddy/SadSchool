﻿// <copyright file="LessonController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using SadSchool.Contracts;
    using SadSchool.DbContexts;
    using SadSchool.Models.SqlServer;
    using SadSchool.Services;
    using SadSchool.ViewModels;

    /// <summary>
    /// Processes operations with <see cref="Lesson"/> entities.
    /// </summary>
    public class LessonController : Controller
    {
        private readonly SadSchoolContext context;
        private readonly INavigationService navigationService;
        private readonly IAuthService authService;
        private readonly ICacheService cacheService;
        private readonly ICommonMapper commonMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="LessonController"/> class.
        /// </summary>
        /// <param name="context">DB context.</param>
        /// <param name="navigationService">Service that operates "Back" button.</param>
        /// <param name="authService">Service that checks user authentication.</param>
        /// <param name="cacheService">Service that operates cache.</param>
        /// <param name="commonMapper">Service that maps entities.</param>
        public LessonController(
            SadSchoolContext context,
            INavigationService navigationService,
            IAuthService authService,
            ICacheService cacheService,
            ICommonMapper commonMapper)
        {
            this.context = context;
            this.navigationService = navigationService;
            this.authService = authService;
            this.cacheService = cacheService;
            this.commonMapper = commonMapper;
        }

        /// <summary>
        /// Gets lessons list view.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for lessons main page.</returns>
        [HttpGet]
        public IActionResult Lessons()
        {
            var lessons = new List<LessonViewModel>();

            foreach (var lesson in this.context.Lessons.ToList())
            {
                lessons.Add(new LessonViewModel
                {
                    Id = lesson.Id,
                    Date = lesson?.Date,
                    LessonData = $"{lesson?.ScheduledLesson}",
                });
            }

            this.navigationService.RefreshBackParams(this.RouteData);

            return this.View(@"~/Views/Data/Lessons.cshtml", lessons);
        }

        /// <summary>
        /// Gets <see cref="Lesson"/> entity add-form.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for <see cref="Lesson"/> add form ot redirects to Lessons.</returns>
        [HttpGet]
        public IActionResult Add()
        {
            if (this.authService.IsAutorized(this.User))
            {
                LessonViewModel viewModel = new()
                {
                    ScheduledLessons = this.GetScheduledLessonsList(null),
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
        public IActionResult Add(LessonViewModel viewModel)
        {
            if (this.ModelState.IsValid)
            {
                var lesson = this.commonMapper.LessonToModel(viewModel);

                this.context.Lessons.Add(lesson);
                this.context.SaveChanges();

                this.cacheService.GetObject<Lesson>(lesson.Id!.Value);
            }

            return this.RedirectToAction("Lessons");
        }

        /// <summary>
        /// Gets <see cref="Lesson"/> entity edit-form.
        /// </summary>
        /// <param name="id">Edited lesson id.</param>
        /// <returns><see cref="ViewResult"/> for the entity-edit form or <see cref="RedirectToActionResult"/> for Lessons view.</returns>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (this.authService.IsAutorized(this.User))
            {
                var editedLesson = this.context.Lessons.Find(id);

                if (editedLesson != null)
                {
                    LessonViewModel viewModel = this.commonMapper.LessonToVm(editedLesson);
                    viewModel.ScheduledLessons = this.GetScheduledLessonsList(viewModel.ScheduledLessonId);

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
        public IActionResult Edit(LessonViewModel viewModel)
        {
            if (this.ModelState.IsValid && viewModel != null)
            {
                var lesson = this.commonMapper.LessonToModel(viewModel);

                this.context.Lessons.Update(lesson);
                this.context.SaveChanges();

                this.cacheService.RefreshObject(lesson);

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
        public IActionResult Delete(int id)
        {
            if (this.authService.IsAutorized(this.User))
            {
                var lesson = this.context.Lessons.Find(id);

                if (lesson != null)
                {
                    this.context.Lessons.Remove(lesson);
                    this.context.SaveChanges();

                    this.cacheService.RemoveObject(lesson);
                }
            }

            return this.RedirectToAction("Lessons");
        }

        private List<SelectListItem> GetScheduledLessonsList(int? lessonId)
        {
            var scheduledLessons = this.context.ScheduledLessons.ToList();

            return scheduledLessons
                .OrderBy(scheduledLesson => scheduledLesson?.Class?.Name, new MixedNumericStringComparer())
                .OrderBy(scheduledLesson => this.GetDayOfWeekNumber(scheduledLesson.Day))
                .Select(scheduledLesson => new SelectListItem
                {
                    Value = scheduledLesson.Id.ToString(),
                    Text = $"{scheduledLesson}",
                    Selected = lessonId?.ToString() == scheduledLesson.Id.ToString(),
                }).ToList();
        }

        private int GetDayOfWeekNumber(string? day)
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
    }
}
