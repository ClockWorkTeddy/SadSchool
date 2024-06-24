// <copyright file="AuthService.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Services
{
    using System.Security.Claims;
    using SadSchool.Controllers.Contracts;
    using Serilog;

    /// <summary>
    /// Service that checks if the user is authentificated in an acceptable role.
    /// </summary>
    public class AuthService : IAuthService
    {
        /// <summary>
        /// Check if the user is authentificated and not in user role (admin/moder).
        /// </summary>
        /// <param name="user"><see cref="ClaimsPrincipal"/> object for user instance.</param>
        /// <returns>Is user has rights.</returns>
        public bool IsAutorized(ClaimsPrincipal user)
        {
            Log.Information("AuthService.IsAutorized(): method called with parameters: user = {user?.Identity?.Name}");

            if (user?.Identity?.IsAuthenticated != null)
            {
                var isAuthenticated = user.Identity.IsAuthenticated;

                return isAuthenticated && !user.IsInRole("User");
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check if the user is authentificated and in admin role.
        /// </summary>
        /// <param name="user"><see cref="ClaimsPrincipal"/> object for user instance.</param>
        /// <returns>Is a user has admin rights.</returns>
        public bool IsAdmin(ClaimsPrincipal user)
        {
            Log.Information($"AuthService.IsAdmin(): method called with parameters: user = {user?.Identity?.Name}");

            if (user?.Identity?.IsAuthenticated != null)
            {
                var isAuthenticated = user.Identity.IsAuthenticated;

                return isAuthenticated && user.IsInRole("admin");
            }

            return false;
        }
    }
}
