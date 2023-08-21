using Microsoft.AspNetCore.Mvc.Rendering;
using SadSchool.Models;

namespace SadSchool.ViewModels
{
    public class ClassAddViewModel
    {
        public int? Id { get; set; }
        public int? TeacherId { get; set; }
        public string? Name { get; set; } = null!;

        public List<SelectListItem> Teachers { get; set; } = new List<SelectListItem>();
    }
}
