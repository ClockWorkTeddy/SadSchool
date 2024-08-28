// <copyright file="Subject.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Models.SqlServer;

using System.Text.Json.Serialization;
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
    [JsonIgnore]
    public virtual ICollection<ScheduledLesson> Lessons { get; set; } = new List<ScheduledLesson>();
}
