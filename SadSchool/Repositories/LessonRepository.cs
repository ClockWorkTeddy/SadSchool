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
    public class LessonRepository : ILessonRepository
    {
        private readonly SadSchoolContext context;
        private readonly ICacheService cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LessonRepository"/> class.
        /// </summary>
        /// <param name="context">Data context.</param>
        /// <param name="cacheService">Cache service.</param>
        public LessonRepository(SadSchoolContext context, ICacheService cacheService)
        {
            this.context = context;
            this.cacheService = cacheService;
        }

        /// <inheritdoc/>
        public async Task<List<Lesson>> GetAllLessonsAsync()
        {
            var lessons = this.cacheService.GetObjects<Lesson>();

            if (lessons != null)
            {
                return lessons;
            }

            lessons = await this.context.Lessons.ToListAsync();

            this.cacheService.SetObjects(lessons);

            return lessons;
        }

        /// <inheritdoc/>
        public async Task<Lesson?> GetLessonByIdAsync(int id)
        {
            var lesson = this.cacheService.GetObject<Lesson>(id);

            if (lesson != null)
            {
                return lesson;
            }

            lesson = await this.context.Lessons.FindAsync(id);

            if (lesson != null)
            {
                this.cacheService.SetObject(lesson);
            }

            return lesson;
        }

        /// <inheritdoc/>
        public async Task<Lesson?> AddLessonAsync(Lesson lesson)
        {
            try
            {
                await this.context.Lessons.AddAsync(lesson);
                await this.context.SaveChangesAsync();

                this.cacheService.RemoveObjects<Lesson>();
                this.cacheService.SetObject(lesson);

                return lesson;
            }
            catch (DbUpdateException ex)
            {
                // Handle the exception as needed (e.g., log it, rethrow it, etc.)
                // For now, we just return null to indicate failure.
                Log.Error(ex, "Error adding lesson to the database.");

                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateLessonAsync(Lesson lesson)
        {
            var existing = await this.context.Lessons.FindAsync(lesson.Id);

            if (existing == null)
            {
                return false;
            }

            this.context.Entry(existing).CurrentValues.SetValues(lesson);

            await this.context.SaveChangesAsync();

            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteLessonAsync(int id)
        {
            var lesson = await this.GetLessonByIdAsync(id);

            if (lesson == null)
            {
                return false;
            }

            this.context.Lessons.Remove(lesson);
            await this.context.SaveChangesAsync();

            return true;
        }

        /// <inheritdoc/>
        public async Task<List<Lesson>> GetLessonsByDateAsync(string date)
        {
            return await this.context.Lessons
                .Where(l => l.Date == date)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Lesson?> GetLessonByScheduledLessonIdAsync(int scheduledLessonId)
        {
            return await this.context.Lessons
                .FirstOrDefaultAsync(l => l.ScheduledLessonId == scheduledLessonId);
        }
    }
}
