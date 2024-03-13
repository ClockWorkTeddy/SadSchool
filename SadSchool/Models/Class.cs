// <copyright file="Class.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Models;

/// <summary>
/// Class model.
/// </summary>
public partial class Class : BaseModel
{
    /// <summary>
    /// Gets or sets class name.
    /// </summary>
    public string? Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets id of the class teacher.
    /// </summary>
    public int? TeacherId { get; set; }

    /// <summary>
    /// Gets or sets id of the class leader.
    /// </summary>
    public int? LeaderId { get; set; }

    /// <summary>
    /// Gets or sets class's lessons list.
    /// </summary>
    public virtual ICollection<ScheduledLesson>? Lessons { get; set; } = new List<ScheduledLesson>();

    /// <summary>
    /// Gets or sets class's students list.
    /// </summary>
    public virtual ICollection<Student>? Students { get; set; } = new List<Student>();

    /// <summary>
    /// Gets or sets class's teacher model object.
    /// </summary>
    public virtual Teacher? Teacher { get; set; }
}
