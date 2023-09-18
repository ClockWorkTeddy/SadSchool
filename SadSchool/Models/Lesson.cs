namespace SadSchool.Models;

public partial class Lesson
{
    public int Id { get; set; }
    public string? Date { get; set; }
    public int? ScheduledLessonId { get; set; }
    public virtual ScheduledLesson? ScheduledLesson { get; set; }
    public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();
}

