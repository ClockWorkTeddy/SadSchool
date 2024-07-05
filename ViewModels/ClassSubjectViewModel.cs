// <copyright file="ClassSubjectViewModel.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.ViewModels
{
    /// <summary>
    /// Represents the class subject view model.
    /// </summary>
    public class ClassSubjectViewModel
    {
        /// <summary>
        /// Gets or sets the list of subjects.
        /// </summary>
        public List<string?>? Subjects { get; set; } = new();

        /// <summary>
        /// Gets or sets the class name.
        /// </summary>
        public string ClassName { get; set; } = string.Empty;
    }
}
