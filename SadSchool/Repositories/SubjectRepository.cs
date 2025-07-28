// <copyright file="SubjectRepository.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using SadSchool.Contracts;
    using SadSchool.Contracts.Repositories;
    using SadSchool.DbContexts;
    using SadSchool.Models.SqlServer;

    /// <summary>
    /// Provides methods for accessing and managing subjects within the data store.
    /// </summary>
    /// <remarks>This repository is responsible for retrieving and manipulating subject data. It utilizes
    /// caching to improve performance and reduce database load.</remarks>
    /// <param name="context">DB context instance.</param>
    /// <param name="cacheService">Cache service instance.</param>
    public class SubjectRepository(SadSchoolContext context, ICacheService cacheService)
        : BaseRepository(context, cacheService), ISubjectRepository
    {
        /// <inheritdoc/>
        public async Task<Subject?> GetSubjectByNameAsync(string name)
        {
            return await this.Context.Subjects.FirstOrDefaultAsync(s => s.Name == name);
        }
    }
}
