// <copyright file="Lesson.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Models;

/// <summary>
/// Lesson (like a real event that is being held some time) model.
/// </summary>
public partial class Lesson : BaseModel
{
    /// <summary>
    /// Gets or sets lesson date.
    /// </summary>
    public string? Date { get; set; }

    /// <summary>
    /// Gets or sets lesson's scheduled lesson id.
    /// </summary>
    public int? ScheduledLessonId { get; set; }

    /// <summary>
    /// Gets or sets lesson's scheduled lesson object.
    /// </summary>
    public virtual ScheduledLesson? ScheduledLesson { get; set; } = new ScheduledLesson();

    /// <summary>
    /// Gets or sets lesson's marks list.
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();

    /// <summary>
    /// Orerriden ToString().
    /// </summary>
    /// <returns>Returns a string representation of the lesson object.</returns>
    public override string ToString() => $"{this.Date} {this.ScheduledLesson}";
}