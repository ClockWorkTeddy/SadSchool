namespace SadSchool.Models;

public partial class Lesson : BaseModel
{
    public string? Date { get; set; }
    public int? ScheduledLessonId { get; set; }
    public virtual ScheduledLesson? ScheduledLesson { get; set; } = new ScheduledLesson();
    public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();

    public override string ToString() =>
         $"{Date} {ScheduledLesson}";
}

