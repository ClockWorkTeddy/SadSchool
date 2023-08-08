using SadSchool.Models;

namespace SadSchool.ViewModels
{
    public class MarkAddViewModel
    {
        public List<Student?> StudentsForView { get; set; } = new List<Student?>();
        public List<Lesson?> LessonsForView { get; set; } = new List<Lesson?>();
        public int StudentId { get; set; }
        public int LessonId { get; set; }
        public string Value { get; set; }
    }
}
