using SadSchool.Models;
using System.ComponentModel.DataAnnotations;

namespace SadSchool.ViewModels
{
    public class MarkAddViewModel
    {
        public List<Student?> StudentsForView { get; set; } = new List<Student?>();
        public List<Lesson?> LessonsForView { get; set; } = new List<Lesson?>();
        [Required(ErrorMessage = "You have to choose a student!")]
        public int? StudentId { get; set; }
        [Required(ErrorMessage = "You have to choose a lesson!")]
        public int? LessonId { get; set; }
        public string Value { get; set; }
    }
}
