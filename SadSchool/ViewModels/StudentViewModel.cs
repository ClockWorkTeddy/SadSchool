using Microsoft.AspNetCore.Mvc.Rendering;

namespace SadSchool.ViewModels
{
    public class StudentViewModel
    {
        public int? Id { get; set; }
        public string? FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public string? DateOfBirth { get; set; }
        public bool? Sex { get; set; }
        public int? ClassId { get; set; }
        public string? ClassName { get; set; } = null!;
        public List<SelectListItem> Classes { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Sexes { get; set; } = new List<SelectListItem>();
    }
}
