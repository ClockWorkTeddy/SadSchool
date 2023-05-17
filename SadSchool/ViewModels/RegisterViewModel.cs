using System.ComponentModel.DataAnnotations;
namespace SadSchool.ViewModels
{
    public class RegisterViewModel
    {
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
    }
}
