// <copyright file="MarkViewModel.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.ViewModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using MongoDB.Bson;

    /// <summary>
    /// Represents the mark view model.
    /// </summary>
    public class MarkViewModel
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// Gets or sets the mark's value.
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Gets or sets the student's id.
        /// </summary>
        public int? StudentId { get; set; }

        /// <summary>
        /// Gets or sets the student's name.
        /// </summary>
        public string? Student { get; set; }

        /// <summary>
        /// Gets or sets the list of students.
        /// </summary>
        public List<SelectListItem> Students { get; set; } = new List<SelectListItem>();

        /// <summary>
        /// Gets or sets the lesson's id.
        /// </summary>
        public int? LessonId { get; set; }

        /// <summary>
        /// Gets or sets the lesson's name.
        /// </summary>
        public string? Lesson { get; set; }

        /// <summary>
        /// Gets or sets the list of lessons.
        /// </summary>
        public List<SelectListItem> Lessons { get; set; } = new List<SelectListItem>();
    }
}
