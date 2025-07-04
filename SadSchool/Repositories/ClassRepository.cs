// <copyright file="ClassRepository.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Repositories
{
    using SadSchool.Contracts;
    using SadSchool.Contracts.Repositories;
    using SadSchool.DbContexts;

    /// <inheritdoc/>
    public class ClassRepository : BaseRepository, IClassRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClassRepository"/> class.
        /// </summary>
        /// <param name="context">DB context.</param>
        /// <param name="cacheService">Cache service instance.</param>
        public ClassRepository(SadSchoolContext context, ICacheService cacheService)
            : base(context, cacheService)
        {
        }
    }
}
