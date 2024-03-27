// <copyright file="LessonInfo.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Services.Schedule
{
    /// <summary>
    /// The class represents the lesson info.
    /// </summary>
    public class LessonInfo
    {
        /// <summary>
        /// Gets or sets the lesson start time.
        /// </summary>
        public string? StartTime { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the lesson's teacher.
        /// </summary>
        public string? Teacher { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the lesson's subject name.
        /// </summary>
        public string? Name { get; set; } = string.Empty;
    }
}
