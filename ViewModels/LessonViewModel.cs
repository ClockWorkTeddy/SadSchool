// <copyright file="LessonViewModel.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.ViewModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    /// <summary>
    /// Represents the lesson view model.
    /// </summary>
    public class LessonViewModel
    {
        /// <summary>
        /// Gets or sets the lesson id.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets the lesson name.
        /// </summary>
        public string? Date { get; set; }

        /// <summary>
        /// Gets or sets the lesson data.
        /// </summary>
        public int? ScheduledLessonId { get; set; }

        /// <summary>
        /// Gets or sets the lesson data.
        /// </summary>
        public string? LessonData { get; set; }

        /// <summary>
        /// Gets or sets the list of scheduled lessons.
        /// </summary>
        public List<SelectListItem> ScheduledLessons { get; set; } = new List<SelectListItem>();
    }
}
