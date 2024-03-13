using System.ComponentModel.DataAnnotations;
namespace SadSchool.ViewModels
{
    public enum Roles
    {
        admin,
        moder,
        user
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords are different!")]
        [DataType(DataType.Password)]
        [Display(Name = "Password confirm")]
        public string PasswordConfirm { get; set; }

        public List<string?>? RolesForDisplay { get; set; }
    }
}
