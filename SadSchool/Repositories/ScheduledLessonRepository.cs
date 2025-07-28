// <copyright file="ScheduledLessonRepository.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using SadSchool.Contracts;
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
    /// <param name="cacheService">Cache service instance.</param>
    public class ScheduledLessonRepository(SadSchoolContext context, ICacheService cacheService)
        : BaseRepository(context, cacheService), IScheduledLessonRepository
    {
        /// <inheritdoc/>
        public async Task<List<ScheduledLesson>> GetScheduledLessonsByStartTimeIdAsync(int startTimeId)
        {
            return await this.Context.ScheduledLessons
                .Where(x => x.StartTimeId == startTimeId)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<ScheduledLesson>> GetScheduledLessonsBySubjectIdAsync(int subjectId)
        {
            return await this.Context.ScheduledLessons
                .Where(x => x.SubjectId == subjectId)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<ScheduledLesson>> GetScheduledLessonsByClassIdAsync(int classId)
        {
            return await this.Context.ScheduledLessons
                .Where(x => x.ClassId == classId)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<ScheduledLesson>> GetScheduledLessonsByTeacherIdAsync(int teacherId)
        {
            return await this.Context.ScheduledLessons
                .Where(x => x.TeacherId == teacherId)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<ScheduledLesson>> GetScheduledLessonsByDayAsync(Days day)
        {
            return await this.Context.ScheduledLessons
                .Where(x => x.Day == day.ToString())
                .ToListAsync();
        }
    }
}
