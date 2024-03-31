// <copyright file="AverageMarksViewModel.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.ViewModels
{
    using SadSchool.Services.ApiServices;

    /// <summary>
    /// Represents the average marks view model.
    /// </summary>
    public class AverageMarksViewModel
    {
        /// <summary>
        /// Gets or sets table for average marks.
        /// </summary>
        public AverageMarkModel?[,] AverageMarksTable { get; set; } = new AverageMarkModel[0, 0];

        /// <summary>
        /// Gets or sets the list of students.
        /// </summary>
        public List<string> Students { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of subjects.
        /// </summary>
        public List<string?>? Subjects { get; set; } = new();
    }
}
