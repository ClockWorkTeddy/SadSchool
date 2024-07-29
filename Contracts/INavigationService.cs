// <copyright file="INavigationService.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Contracts
{
    using Microsoft.AspNetCore.Routing;

    /// <summary>
    /// Navigation service interface.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Gets or sets the controller name.
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// Gets or sets the action name.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Gets the class name.
        /// </summary>
        public string ClassName { get; }

        /// <summary>
        /// Refreshes the "Back" button parameters.
        /// </summary>
        /// <param name="routeData">Current route data.</param>
        public void RefreshBackParams(RouteData routeData);

        /// <summary>
        /// Stores the class name.
        /// </summary>
        /// <param name="className">Class name.</param>
        public void StoreClassName(string className);
    }
}
