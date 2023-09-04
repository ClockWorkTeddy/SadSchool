using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SadSchool.ViewModels
{
    public class LessonViewModel
    {
        public int Id { get; set; }
        public string? Date { get; set; }

        [Required(ErrorMessage = "You have to choose a start time!")]
        public int? StartTimeId { get; set; }
        public string? StartTimeValue { get; set; }
        public List<SelectListItem> StartTimes { get; set; } = new List<SelectListItem>();

        [Required(ErrorMessage = "You have to choose a subject!")]
        public int? SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public List<SelectListItem> Subjects { get; set; } = new List<SelectListItem>();

        [Required(ErrorMessage = "You have to choose a class!")]
        public int? ClassId { get; set; }
        public string? ClassName { get; set; }
        public List<SelectListItem> Classes { get; set; } = new List<SelectListItem>();

        [Required(ErrorMessage = "You have to choose a teacher!")]
        public int? TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public List<SelectListItem> Teachers { get; set; } = new List<SelectListItem>();
        

    }
}
