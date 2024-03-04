using Microsoft.AspNetCore.Mvc.Rendering;
using SadSchool.Models;
namespace SadSchool.ViewModels
{
    public class ClassViewModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public int? TeacherId { get; set; }
        public int? LeaderId { get; set; }
        public string? TeacherName { get; set; }
        public string? LeaderName { get; set; }
        public List<SelectListItem> Teachers { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Leaders { get; set; } = new List<SelectListItem>();
    }
}
