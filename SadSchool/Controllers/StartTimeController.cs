// <copyright file="StartTimeController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models.SqlServer;
    using SadSchool.Controllers.Contracts;
    using SadSchool.DbContexts;
    using SadSchool.ViewModels;

    /// <summary>
    /// Processes requests for start time data.
    /// </summary>
    public class StartTimeController : Controller
    {
        private readonly SadSchoolContext context;
        private readonly INavigationService navigationService;
        private readonly IAuthService authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartTimeController"/> class.
        /// </summary>
        /// <param name="context">DB context.</param>
        /// <param name="navigationService">Service processes "Back" button.</param>
        /// <param name="authService">Service processes user authorization check.</param>
        public StartTimeController(
            SadSchoolContext context,
            INavigationService navigationService,
            IAuthService authService)
        {
            this.context = context;
            this.navigationService = navigationService;
            this.authService = authService;
        }

        /// <summary>
        /// Gets the start times view.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for the "StartTimes" view.</returns>
        [HttpGet]
        public IActionResult StartTimes()
        {
            List<StartTimeViewModel> schedules = new List<StartTimeViewModel>();

            foreach (var schedule in this.context.StartTimes)
            {
                schedules.Add(new StartTimeViewModel
                {
                    Id = schedule.Id,
                    StartTime = schedule.Value,
                });
            }

            this.navigationService.RefreshBackParams(this.RouteData);

            return this.View(@"~/Views/Data/StartTimes.cshtml", schedules);
        }

        /// <summary>
        /// Gets the form for add a new start time.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for the "StartTimeAdd" view
        ///     or <see cref="RedirectToActionResult"/> for the "StartTimes" action.</returns>
        [HttpGet]
        public IActionResult Add()
        {
            if (this.authService.IsAutorized(this.User))
            {
                this.navigationService.RefreshBackParams(this.RouteData);

                return this.View(@"~/Views/Data/StartTimeAdd.cshtml");
            }

            return this.RedirectToAction("StartTimes");
        }

        /// <summary>
        /// Adds a new start time.
        /// </summary>
        /// <param name="model">View model with data.</param>
        /// <returns><see cref="RedirectToActionResult"/> for the action "StartTimes".</returns>
        [HttpPost]
        public IActionResult Add(StartTimeViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var schedule = new StartTime
                {
                    Value = model.StartTime,
                };

                this.context.StartTimes.Add(schedule);
                this.context.SaveChanges();
            }

            return this.RedirectToAction("StartTimes");
        }

        /// <summary>
        /// Gets the form for edit a start time.
        /// </summary>
        /// <param name="id">The edited start time.</param>
        /// <returns><see cref="ViewResult"/> for the "StartTimeEdit" view or
        ///     <see cref="RedirectToActionResult"/> for the "StartTimes" action.</returns>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (this.authService.IsAutorized(this.User))
            {
                var editedStartTime = this.context.StartTimes.Find(id);

                StartTimeViewModel viewModel = new()
                {
                    StartTime = editedStartTime?.Value,
                };

                this.navigationService.RefreshBackParams(this.RouteData);

                return this.View(@"~/Views/Data/StartTimeEdit.cshtml", viewModel);
            }

            return this.RedirectToAction("StartTimes");
        }

        /// <summary>
        /// Edits a start time.
        /// </summary>
        /// <param name="viewModel">View model with data.</param>
        /// <returns><see cref="RedirectToActionResult"/> to the "StartTimes" action of
        ///     <see cref="ViewResult"/> for StartTimeEdit view.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(StartTimeViewModel viewModel)
        {
            if (this.authService.IsAutorized(this.User))
            {
                var startTime = new StartTime
                {
                    Id = viewModel.Id,
                    Value = viewModel.StartTime,
                };

                this.context.StartTimes.Update(startTime);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("StartTimes");
            }
            else
            {
                return this.View(@"~/Views/Data/StartTimeEdit.cshtml", viewModel);
            }
        }

        /// <summary>
        /// Deletes a start time.
        /// </summary>
        /// <param name="id">Deleted item id.</param>
        /// <returns><see cref="RedirectToActionResult"/> for the "StartTimes" action.</returns>
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (this.authService.IsAutorized(this.User))
            {
                var schedule = this.context.StartTimes.Find(id);

                if (schedule != null)
                {
                    this.context.StartTimes.Remove(schedule);
                    this.context.SaveChanges();
                }
            }

            return this.RedirectToAction("StartTimes");
        }
    }
}
