// <copyright file="LessonCheckService.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Services.HangFire
{
    using SadSchool.DbContexts;
    using Serilog;

    /// <summary>
    /// Service that checks for lessons without a date and deletes them.
    /// </summary>
    public class LessonCheckService
    {
        private readonly SadSchoolContext sadSchoolContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="LessonCheckService"/> class.
        /// </summary>
        /// <param name="sadSchoolContext">DB context.</param>
        public LessonCheckService(SadSchoolContext sadSchoolContext)
        {
            this.sadSchoolContext = sadSchoolContext;
        }

        /// <summary>
        /// Checks for lessons without a date and deletes them.
        /// </summary>
        public void DeleteLessonWithoutDate()
        {
            var lessonsWithoutDate = this.sadSchoolContext.Lessons
                .Where(x => x.Date == null)
                .ToList();

            if (lessonsWithoutDate.Count > 0)
            {
                var firstLessonWithoutDate = lessonsWithoutDate.First();
                this.sadSchoolContext.Remove(firstLessonWithoutDate);
                this.sadSchoolContext.SaveChanges();
                Log.Information($"LessonCheckService.DeleteLessonWithoutDate(): Deleted lesson without date, Id: {firstLessonWithoutDate.Id}");
            }
        }
    }
}
