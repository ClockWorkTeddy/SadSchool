// <copyright file="IAuthService.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers.Contracts
{
    using System.Security.Claims;

    /// <summary>
    /// Define the service that checks if the user is authentificated in an acceptable role.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Check if the user is authentificated in an acceptable role.
        /// </summary>
        /// <param name="user"><see cref="ClaimsPrincipal"/> object of current user.</param>
        /// <returns>Does user have rights or not.</returns>
        bool IsAutorized(ClaimsPrincipal user);

        /// <summary>
        /// Check if the user is authentificated and in admin role.
        /// </summary>
        /// <param name="user"><see cref="ClaimsPrincipal"/> object for user instance.</param>
        /// <returns>Is a user has admin rights.</returns>
        public bool IsAdmin(ClaimsPrincipal user);
    }
}
