// <copyright file="ScheduleCell.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Services.Schedule
{
    /// <summary>
    /// Enum for days of the week.
    /// </summary>
    public enum Days
    {
        /// <summary>
        /// Monday.
        /// </summary>
        Mon,

        /// <summary>
        /// Tuesday.
        /// </summary>
        Tue,

        /// <summary>
        /// Wednesday.
        /// </summary>
        Wed,

        /// <summary>
        /// Thursday.
        /// </summary>
        Thu,

        /// <summary>
        /// Friday.
        /// </summary>
        Fri,
    }

    /// <summary>
    /// Class for schedule cell.
    /// </summary>
    public class ScheduleCell
    {
        /// <summary>
        /// Gets or sets the day.
        /// </summary>
        public string? Day { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the class name.
        /// </summary>
        public string? ClassName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the lesson infos.
        /// </summary>
        public List<LessonInfo>? LessonInfos { get; set; } = new List<LessonInfo>();
    }
}
