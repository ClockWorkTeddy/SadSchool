// <copyright file="ScheduledLesson.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Models;

/// <summary>
/// A scheduled lesson model. Scheduled lesson is the repeated entity of the planned lesson with partucular
///     time, class and teacher.
/// </summary>
public partial class ScheduledLesson : BaseModel
{
    /// <summary>
    /// Gets or sets the start time id.
    /// </summary>
    public int? StartTimeId { get; set; }

    /// <summary>
    /// Gets or sets the subject id.
    /// </summary>
    public int? SubjectId { get; set; }

    /// <summary>
    /// Gets or sets the class id.
    /// </summary>
    public int? ClassId { get; set; }

    /// <summary>
    /// Gets or sets the teacher id.
    /// </summary>
    public int? TeacherId { get; set; }

    /// <summary>
    /// Gets or sets the day of the week.
    /// </summary>
    public string? Day { get; set; }

    /// <summary>
    /// Gets or sets the class object.
    /// </summary>
    public virtual Class? Class { get; set; } = null!;

    /// <summary>
    /// Gets or sets the start time object.
    /// </summary>
    public virtual StartTime? StartTime { get; set; } = null!;

    /// <summary>
    /// Gets or sets the subject object.
    /// </summary>
    public virtual Subject? Subject { get; set; } = null!;

    /// <summary>
    /// Gets or sets the teacher object.
    /// </summary>
    public virtual Teacher? Teacher { get; set; } = null!;

    /// <summary>
    /// Gets or sets the lessons list.
    /// </summary>
    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    /// <summary>
    /// Overriden ToString().
    /// </summary>
    /// <returns>A string representation of scheduled lesson object with the data about a day, a class, a teacher,
    ///     a subject and a start time.</returns>
    public override string ToString() =>
        $"{this.Day} {this.Class?.Name} {this.StartTime?.Value} {this.Subject?.Name} {this.Teacher}";
}
