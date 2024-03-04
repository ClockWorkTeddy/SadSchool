// <copyright file="ScheduleController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SadSchool.Models;
    using SadSchool.Services;
    using SadSchool.Services.Schedule;
    using SadSchool.ViewModels;

    /// <summary>
    /// Processes requests for schedule data.
    /// </summary>
    public class ScheduleController : Controller
    {
        private readonly SadSchoolContext context;
        private readonly INavigationService navigationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleController"/> class.
        /// </summary>
        /// <param name="context">DB context instance.</param>
        /// <param name="navigationService"><see cref="INavigationService"/> instance.</param>
        public ScheduleController(SadSchoolContext context, INavigationService navigationService)
        {
            this.context = context;
            this.navigationService = navigationService;
        }

        /// <summary>
        /// Returns the schedule view.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for Schedule view.</returns>
        [HttpGet]
        public IActionResult GetSchedule()
        {
            var scheduledLessons = this.context.ScheduledLessons.ToList();

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
