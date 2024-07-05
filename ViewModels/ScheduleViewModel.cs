// <copyright file="ScheduleViewModel.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.ViewModels
{
    using SadSchool.Dtos;

    /// <summary>
    /// Represents the schedule view model.
    /// </summary>
    public class ScheduleViewModel
    {
        /// <summary>
        /// Gets the list of days.
        /// </summary>
        public List<string> Days { get; } = new() { "Mon", "Tue", "Wed", "Thu", "Fri" };

        /// <summary>
        /// Gets or sets the list of classes.
        /// </summary>
        public List<string?> Classes { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of SheduleCells.
        /// </summary>
        public ScheduleCellDto[,] Cells { get; set; } = new ScheduleCellDto[,] { };
    }
}
