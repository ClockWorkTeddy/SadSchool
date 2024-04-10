// <copyright file="ISecretService.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Services
{
    /// <summary>
    /// Manages secrets.
    /// </summary>
    public interface ISecretService
    {
        /// <summary>
        /// Gets the secret data.
        /// </summary>
        /// <returns>Secret data string.</returns>
        string? GetSecret();
    }
}
