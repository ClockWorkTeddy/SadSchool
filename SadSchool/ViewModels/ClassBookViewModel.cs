// <copyright file="ClassBookViewModel.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.ViewModels
{
    using SadSchool.Services.ClassBook;

    /// <summary>
    /// Represents the class book view model.
    /// </summary>
    public class ClassBookViewModel
    {
        /// <summary>
        /// Gets or sets the class name.
        /// </summary>
        public string ClassName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the subject name.
        /// </summary>
        public string SubjectName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the dates.
        /// </summary>
        public List<string> Dates { get; set; } = new();

        /// <summary>
        /// Gets or sets the students.
        /// </summary>
        public List<string> Students { get; set; } = new();

        /// <summary>
        /// Gets or sets the mark cells.
        /// </summary>
        public MarkCellModel[,] MarkCells { get; set; } = new MarkCellModel[0, 0];
    }
}