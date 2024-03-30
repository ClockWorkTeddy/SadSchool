// <copyright file="RegisterViewModel.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents the registration view model.
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// Gets or sets the role name.
        /// </summary>
        [Required]
        [Display(Name = "Role")]
        public string? RoleName { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        [Required]
        [Display(Name = "Username")]
        public string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets the password confirmation.
        /// </summary>
        [Required]
        [Compare("Password", ErrorMessage = "Passwords are different!")]
        [DataType(DataType.Password)]
        [Display(Name = "Password confirm")]
        public string? PasswordConfirm { get; set; }

        /// <summary>
        /// Gets or sets the list of roles for display.
        /// </summary>
        public List<string?>? RolesForDisplay { get; set; }
    }
}
