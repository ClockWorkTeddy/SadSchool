// <copyright file="ClassViewModel.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.ViewModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    /// <summary>
    /// Represents the class view model.
    /// </summary>
    public class ClassViewModel
    {
        /// <summary>
        /// Gets or sets the class id.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets the class name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the teacher id.
        /// </summary>
        public int? TeacherId { get; set; }

        /// <summary>
        /// Gets or sets the leader id.
        /// </summary>
        public int? LeaderId { get; set; }

        /// <summary>
        /// Gets or sets the teacher name.
        /// </summary>
        public string? TeacherName { get; set; }

        /// <summary>
        /// Gets or sets the list of teachers.
        /// </summary>
        public List<SelectListItem> Teachers { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of leaders.
        /// </summary>
        public List<SelectListItem> Leaders { get; set; } = new();
    }
}
