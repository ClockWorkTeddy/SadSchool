// <copyright file="StartTime.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace Models.SqlServer;

/// <summary>
/// Start time (schedule position) model.
/// </summary>
public partial class StartTime : BaseModel
{
    /// <summary>
    /// Gets or sets the value of the start time.
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// Gets or sets the scheduled lessons list.
    /// </summary>
    public virtual ICollection<ScheduledLesson> Lessons { get; set; } = new List<ScheduledLesson>();
}
