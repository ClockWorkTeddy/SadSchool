// <copyright file="Subject.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Models;

/// <summary>
/// Subject model.
/// </summary>
public partial class Subject : BaseModel
{
    /// <summary>
    /// Gets or sets the name of the subject.
    /// </summary>
    public string? Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the scheduled lessons list.
    /// </summary>
    public virtual ICollection<ScheduledLesson> Lessons { get; set; } = new List<ScheduledLesson>();
}
