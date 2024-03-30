// <copyright file="NewRoleViewModel.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents the new role view model.
    /// </summary>
    public class NewRoleViewModel
    {
        /// <summary>
        /// Gets or sets the role name.
        /// </summary>
        [Required]
        public string? RoleName { get; set; }
    }
}
