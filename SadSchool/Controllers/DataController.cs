// <copyright file="DataController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SadSchool.Controllers.Contracts;

    /// <summary>
    /// Processes "Data" main page.
    /// </summary>
    public class DataController(INavigationService navigationService) : Controller
    {
        private readonly INavigationService navigationService = navigationService;

        /// <summary>
        /// Gets Data main page.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for DataIndex view.</returns>
        [HttpGet]
        public IActionResult DataIndex()
        {
            this.navigationService.RefreshBackParams(this.RouteData);
            return this.View(@"~/Views/Data/DataIndex.cshtml");
        }
    }
}
