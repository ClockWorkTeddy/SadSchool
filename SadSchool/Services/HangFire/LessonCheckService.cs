// <copyright file="LessonCheckService.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Services.HangFire
{
    using SadSchool.Contracts.Repositories;
    using SadSchool.Models.SqlServer;
    using Serilog;

    /// <summary>
    /// Service that checks for lessons without a date and deletes them.
    /// </summary>
    public class LessonCheckService
    {
        private readonly ILessonRepository lessonRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="LessonCheckService"/> class.
        /// </summary>
        /// <param name="lessonRepository">Lessons repo instance.</param>
        public LessonCheckService(ILessonRepository lessonRepository)
        {
            this.lessonRepository = lessonRepository;
        }

        /// <summary>
        /// Checks for lessons without a date and deletes them.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task DeleteLessonWithoutDate()
        {
            var lessons = await this.lessonRepository.GetAllEntitiesAsync<Lesson>();
            var lessonsWithoutDate = lessons.Where(x => x.Date == null).ToList();

            if (lessonsWithoutDate.Count > 0)
            {
                var firstLessonWithoutDate = lessonsWithoutDate[0];
                await this.lessonRepository.DeleteEntityAsync<Lesson>(firstLessonWithoutDate.Id!.Value);
                Log.Information("LessonCheckService.DeleteLessonWithoutDate(): Deleted lesson without date, Id: {FirstLessonWithoutDateId}", firstLessonWithoutDate.Id);
            }
        }
    }
}
