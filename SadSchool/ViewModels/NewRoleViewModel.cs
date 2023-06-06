using System.ComponentModel.DataAnnotations;

namespace SadSchool.ViewModels
{
    public class NewRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
