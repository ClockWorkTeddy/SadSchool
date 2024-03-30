// <copyright file="StudentSubjectSelectorViewModel.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.ViewModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    /// <summary>
    /// Represents the student subject selector view model.
    /// </summary>
    public class StudentSubjectSelectorViewModel
    {
        /// <summary>
        /// Gets or sets the selected student id.
        /// </summary>
        public int SelectedStudentId { get; set; }

        /// <summary>
        /// Gets or sets the selected subject id.
        /// </summary>
        public int SelectedSubjectId { get; set; }

        /// <summary>
        /// Gets or sets the list of students.
        /// </summary>
        public List<SelectListItem>? Students { get; set; }

        /// <summary>
        /// Gets or sets the list of subjects.
        /// </summary>
        public List<SelectListItem>? Subjects { get; set; }
    }
}
