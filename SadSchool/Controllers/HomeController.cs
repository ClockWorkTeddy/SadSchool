// <copyright file="HomeController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers
{
    using System.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using SadSchool.Contracts;
    using SadSchool.ViewModels;

    /// <summary>
    /// Main controller for site layout.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly INavigationService navigationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="navigationService">Service for "Back" button operating.</param>
        public HomeController(
            INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        /// <summary>
        /// Processes index view.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for index view.</returns>
        public IActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// Gets About view.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for about view.</returns>
        public IActionResult About()
        {
            try
            {
                this.navigationService.RefreshBackParams(this.RouteData);
                return this.View();
            }
            catch (Exception ex)
            {
                return this.View(ex.Message);
            }
        }

        /// <summary>
        /// Gets Chat view.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for About view.</returns>
        public IActionResult Chat()
        {
            try
            {
                this.navigationService.RefreshBackParams(this.RouteData);
                return this.View();
            }
            catch (Exception ex)
            {
                return this.View(ex.Message);
            }
        }

        /// <summary>
        /// Gets the Blackboard view.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for the BlackBoard view.</returns>
        public IActionResult Blackboard()
        {
            try
            {
                this.navigationService.RefreshBackParams(this.RouteData);
                return this.View();
            }
            catch (Exception ex)
            {
                return this.View(ex.Message);
            }
        }

        /// <summary>
        /// Processes error cases.
        /// </summary>
        /// <returns><see cref="ViewResult"/>View for errors.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}