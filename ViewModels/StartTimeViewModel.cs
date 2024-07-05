// <copyright file="StartTimeViewModel.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.ViewModels
{
    /// <summary>
    /// Represents the start time view model.
    /// </summary>
    public class StartTimeViewModel
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        public string? StartTime { get; set; } = null!;
    }
}
