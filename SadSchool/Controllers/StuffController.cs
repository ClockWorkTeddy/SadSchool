// <copyright file="StuffController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SadSchool.Controllers.Contracts;
    using SadSchool.Services;

    /// <summary>
    /// Processes requests for stuff data.
    /// </summary>
    public class StuffController : Controller
    {
        private readonly INavigationService navigationService;
        private readonly IAuthService authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StuffController"/> class.
        /// </summary>
        /// <param name="navigationService">Service processes the "Back" button.</param>
        /// <param name="authService">Service processes user authorization check.</param>
        public StuffController(INavigationService navigationService, IAuthService authService)
        {
            this.navigationService = navigationService;
            this.authService = authService;
        }

        /// <summary>
        /// Gets the index view.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for the "Stuff" view or
        ///     for the "Index" view in case of luck of rights.</returns>
        public IActionResult Stuff()
        {
            this.navigationService.RefreshBackParams(this.RouteData);

            if (this.authService.IsAutorized(this.User))
            {
                return this.View("~/Views/Stuff/Stuff.cshtml");
            }

            return this.View("Index");
        }
    }
}
