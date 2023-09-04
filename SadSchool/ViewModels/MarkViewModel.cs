using Microsoft.AspNetCore.Mvc.Rendering;

namespace SadSchool.ViewModels
{
    public class MarkViewModel
    {
        public int Id { get; set; }
        public string Value { get; set; }

        public int? StudentId { get; set; }
        public string? Student { get; set; }
        public List<SelectListItem> Students { get; set; } = new List<SelectListItem>();

        public int? LessonId { get; set; }
        public string? Lesson { get; set; }
        public List<SelectListItem> Lessons { get; set; } = new List<SelectListItem>();
    }
}
