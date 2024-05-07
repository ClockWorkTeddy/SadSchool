// <copyright file="Teacher.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Models;

/// <summary>
/// Teacher model.
/// </summary>
public partial class Teacher : BaseModel
{
    /// <summary>
    /// Gets or sets the first name of the teacher.
    /// </summary>
    public string? FirstName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the last name of the teacher.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Gets or sets the date of birth of the teacher.
    /// </summary>
    public DateOnly? DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the grade of the teacher.
    /// </summary>
    public int? Grade { get; set; }

    /// <summary>
    /// Gets or sets the class id.
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    /// <summary>
    /// Gets or sets the scheduled lessons list.
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public virtual ICollection<ScheduledLesson> Lessons { get; set; } = new List<ScheduledLesson>();

    /// <summary>
    /// Overridden ToString().
    /// </summary>
    /// <returns>The string representation of the theacher object with the full name.</returns>
    public override string ToString() =>
         $"{this.FirstName} {this.LastName}";
}
