// <copyright file="ISecretService.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers.Contracts
{
    /// <summary>
    /// Manages secrets.
    /// </summary>
    public interface ISecretService
    {
        /// <summary>
        /// Gets the secret data.
        /// </summary>
        /// <param name="keyName">Key name.</param>
        /// <returns>Secret data string.</returns>
        string? GetSecret(string keyName);
    }
}
