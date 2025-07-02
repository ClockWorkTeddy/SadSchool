// <copyright file="StartTimeController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SadSchool.Contracts;
    using SadSchool.DbContexts;
    using SadSchool.Models.SqlServer;
    using SadSchool.ViewModels;

    /// <summary>
    /// Processes requests for start time data.
    /// </summary>
    public class StartTimeController : Controller
    {
        private readonly SadSchoolContext context;
        private readonly INavigationService navigationService;
        private readonly IAuthService authService;
        private readonly ICacheService cacheService;
        private readonly ICommonMapper commonMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartTimeController"/> class.
        /// </summary>
        /// <param name="context">DB context.</param>
        /// <param name="navigationService">Service processes "Back" button.</param>
        /// <param name="authService">Service processes user authorization check.</param>
        /// <param name="cacheService">Service processes cache operations.</param>
        /// <param name="commonMapper">Service processes mapping operations.</param>
        public StartTimeController(
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
        /// Gets the start times view.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for the "StartTimes" view.</returns>
        [HttpGet]
        public IActionResult StartTimes()
        {
            List<StartTimeViewModel> startTimes = this.context.StartTimes
                .Select(startTime => this.commonMapper.StartTimeToVm(startTime))
                .ToList();

            this.navigationService.RefreshBackParams(this.RouteData);

            return this.View(@"~/Views/Data/StartTimes.cshtml", startTimes);
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
                var schedule = this.commonMapper.StartTimeToModel(model);

                this.context.StartTimes.Add(schedule);
                this.context.SaveChanges();

                this.cacheService.GetObject<StartTime>(schedule.Id!.Value);
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

                if (editedStartTime != null)
                {
                    var viewModel = this.commonMapper.StartTimeToVm(editedStartTime);

                    this.navigationService.RefreshBackParams(this.RouteData);

                    return this.View(@"~/Views/Data/StartTimeEdit.cshtml", viewModel);
                }
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
                var startTime = this.commonMapper.StartTimeToModel(viewModel);

                this.context.StartTimes.Update(startTime);
                await this.context.SaveChangesAsync();

                this.cacheService.SetObject(startTime);

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
                var startTime = this.context.StartTimes.Find(id);

                if (startTime != null)
                {
                    this.context.StartTimes.Remove(startTime);
                    this.context.SaveChanges();

                    this.cacheService.RemoveObject(startTime);
                }
            }

            return this.RedirectToAction("StartTimes");
        }
    }
}
