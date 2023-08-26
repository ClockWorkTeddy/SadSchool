using Microsoft.AspNetCore.Mvc.Rendering;
using SadSchool.Models;

namespace SadSchool.ViewModels
{
    public class StudentAddViewModel
    {
        public int? Id { get; set; }
        public int? ClassId { get; set; }
        public string? FirstName { get; set; } = null!;
        public string? LastName { get; set; } = null!;
        public string? DateOfBirth { get; set; }
        public string Sex { get; set; }
        public List<SelectListItem> Classes { get; set; } = new List<SelectListItem>();
    }
}
