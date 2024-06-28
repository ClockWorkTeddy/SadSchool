// <copyright file="AuthDbContext.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.DbContexts
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The database context for the authentication.
    /// </summary>
    public class AuthDbContext : IdentityDbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthDbContext"/> class.
        /// </summary>
        /// <param name="options">Object with options data.</param>
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }
    }
}
