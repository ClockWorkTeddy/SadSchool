// <copyright file="MarkCellModel.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Services.ClassBook
{
    /// <summary>
    /// Model for mark cell object.
    /// </summary>
    public class MarkCellModel
    {
        /// <summary>
        /// Gets or sets the mark value.
        /// </summary>
        public string? Mark { get; set; }

        /// <summary>
        /// Gets or sets the student name.
        /// </summary>
        public string StudentName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public string Date { get; set; } = string.Empty;
    }
}
