
namespace SadSchool.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using SadSchool.Contracts.Data;
    using SadSchool.Contracts.Repositories;
    using SadSchool.DbContexts;
    using SadSchool.Models.SqlServer;

    /// <summary>
    /// Provides methods for managing scheduled lessons in the database.
    /// </summary>
    /// <remarks>This repository offers functionality to retrieve, add, update, and delete scheduled lessons.
    /// It interacts with the underlying database context to perform these operations asynchronously.</remarks>
    /// <param name="context">Db context instance.</param>
    public class ScheduledLessonRepository(SadSchoolContext context) : IScheduledLessonRepository
    {
        private readonly SadSchoolContext context = context;

        /// <inheritdoc/>
        public async Task<List<ScheduledLesson>> GetAllScheduledLessonsAsync()
        {
            return await this.context.ScheduledLessons.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<ScheduledLesson?> GetScheduledLessonByIdAsync(int id)
        {
            return await this.context.ScheduledLessons.FindAsync(id);
        }

        /// <inheritdoc/>
        public async Task<List<ScheduledLesson>> GetScheduledLessonsByStartTimeIdAsync(int startTimeId)
        {
            return await this.context.ScheduledLessons.Where(x => x.StartTimeId == startTimeId).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<ScheduledLesson>> GetScheduledLessonsBySubjectIdAsync(int subjectId)
        {
            return await this.context.ScheduledLessons.Where(x => x.SubjectId == subjectId).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<ScheduledLesson>> GetScheduledLessonsByClassIdAsync(int classId)
        {
            return await this.context.ScheduledLessons.Where(x => x.ClassId == classId).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<ScheduledLesson>> GetScheduledLessonsByTeacherIdAsync(int teacherId)
        {
            return await this.context.ScheduledLessons.Where(x => x.TeacherId == teacherId).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<ScheduledLesson>> GetScheduledLessonsByDayAsync(Days day)
        {
            return await this.context.ScheduledLessons.Where(x => x.Day == day.ToString()).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<ScheduledLesson?> AddScheduledLessonAsync(ScheduledLesson scheduledLesson)
        {
            try
            {
                await this.context.ScheduledLessons.AddAsync(scheduledLesson);
                await this.context.SaveChangesAsync();

                return scheduledLesson;
            }
            catch (DbUpdateException)
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateScheduledLessonAsync(ScheduledLesson scheduledLesson)
        {
            var existing = await this.context.ScheduledLessons.FindAsync(scheduledLesson.Id);

            if (existing == null)
            {
                return false;
            }

            existing.StartTimeId = scheduledLesson.StartTimeId;
            existing.SubjectId = scheduledLesson.SubjectId;
            existing.ClassId = scheduledLesson.ClassId;
            existing.TeacherId = scheduledLesson.TeacherId;
            existing.Day = scheduledLesson.Day;

            await this.context.SaveChangesAsync();

            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteScheduledLessonAsync(int id)
        {
            var scheduledLesson = await this.context.ScheduledLessons.FindAsync(id);

            if (scheduledLesson == null)
            {
                return false;
            }

            this.context.ScheduledLessons.Remove(scheduledLesson);

            await this.context.SaveChangesAsync();

            return true;
        }
    }
}
