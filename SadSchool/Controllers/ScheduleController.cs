// <copyright file="ScheduleController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SadSchool.Contracts;
    using SadSchool.Contracts.Repositories;
    using SadSchool.Models.SqlServer;
    using SadSchool.Services.Schedule;
    using SadSchool.ViewModels;

    /// <summary>
    /// Processes requests for schedule data.
    /// </summary>
    public class ScheduleController : Controller
    {
        private readonly IScheduledLessonRepository scheduledLessonRepository;
        private readonly INavigationService navigationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleController"/> class.
        /// </summary>
        /// <param name="scheduledLessonRepository">Scheduled lesson repository instance.</param>
        /// <param name="navigationService"><see cref="INavigationService"/> instance.</param>
        public ScheduleController(IScheduledLessonRepository scheduledLessonRepository, INavigationService navigationService)
        {
            this.scheduledLessonRepository = scheduledLessonRepository;
            this.navigationService = navigationService;
        }

        /// <summary>
        /// Returns the schedule view.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for Schedule view.</returns>
        [HttpGet]
        public async Task<IActionResult> GetSchedule()
        {
            var scheduledLessons = await this.scheduledLessonRepository.GetAllEntitiesAsync<ScheduledLesson>();

            ScheduleService service = new ScheduleService(scheduledLessons);
            var scheduleCells = service.GetScheduleCells();

            this.navigationService.RefreshBackParams(this.RouteData);

            return this.View(@"~/Views/Data/Representation/Schedule.cshtml", new ScheduleViewModel()
            {
                Classes = service.Classes,
                Cells = scheduleCells,
            });
        }
    }
}
