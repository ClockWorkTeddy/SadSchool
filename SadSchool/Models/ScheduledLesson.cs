namespace SadSchool.Models;

public partial class ScheduledLesson
{
    public int Id { get; set; }

    public int? StartTimeId { get; set; }

    public int? SubjectId { get; set; }

    public int? ClassId { get; set; }

    public int? TeacherId { get; set; }

    public string? Day { get; set; }

    public virtual Class? Class { get; set; } = null!;

    public virtual StartTime? StartTime { get; set; } = null!;

    public virtual Subject? Subject { get; set; } = null!;

    public virtual Teacher? Teacher { get; set; } = null!;

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    public override string ToString() =>
        $"{Day} {Class?.Name} {StartTime?.Value} {Subject?.Name} {Teacher}";
            
}
