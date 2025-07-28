// <copyright file="LessonRepository.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using SadSchool.Contracts;
    using SadSchool.Contracts.Repositories;
    using SadSchool.DbContexts;
    using SadSchool.Models.SqlServer;
    using Serilog;

    /// <inheritdoc/>
    public class LessonRepository : BaseRepository, ILessonRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LessonRepository"/> class.
        /// </summary>
        /// <param name="context">Data context.</param>
        /// <param name="cacheService">Cache service.</param>
        public LessonRepository(SadSchoolContext context, ICacheService cacheService)
            : base(context, cacheService)
        {
        }

        /// <inheritdoc/>
        public async Task<List<Lesson>> GetLessonsByDateAsync(string date)
        {
            return await this.Context.Lessons
                .Where(l => l.Date == date)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Lesson?> GetLessonByScheduledLessonIdAsync(int scheduledLessonId)
        {
            return await this.Context.Lessons
                .FirstOrDefaultAsync(l => l.ScheduledLessonId == scheduledLessonId);
        }
    }
}
