using Microsoft.AspNetCore.Mvc.Rendering;

namespace SadSchool.ViewModels
{
    public class LessonViewModel
    {
        public int? Id { get; set; }
        public string? Date { get; set; }
        public int? ScheduledLessonId { get; set; }
        public string? LessonData { get; set; }
        public List<SelectListItem> ScheduledLessons { get; set; } = new List<SelectListItem>();
    }
}
