// <copyright file="StudentViewModel.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.ViewModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    /// <summary>
    /// Represents the student view model.
    /// </summary>
    public class StudentViewModel
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        public string? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the sex of the student.
        /// </summary>
        public bool? Sex { get; set; }

        /// <summary>
        /// Gets or sets the class id.
        /// </summary>
        public int? ClassId { get; set; }

        /// <summary>
        /// Gets or sets the class name.
        /// </summary>
        public string? ClassName { get; set; }

        /// <summary>
        /// Gets or sets the list of classes.
        /// </summary>
        public List<SelectListItem> Classes { get; set; } = new List<SelectListItem>();

        /// <summary>
        /// Gets or sets the list of students' sexes.
        /// </summary>
        public List<SelectListItem> Sexes { get; set; } = new List<SelectListItem>();
    }
}
