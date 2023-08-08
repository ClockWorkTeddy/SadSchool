namespace SadSchool.Models
{
    public class Mark
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int? LessonId { get; set; }
        public virtual Lesson? Lesson { get; set; }
        public int? StudentId { get; set; }
        public virtual Student? Student { get; set; }
    }
}
