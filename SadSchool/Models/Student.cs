// <copyright file="Student.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Models;

/// <summary>
/// Student model.
/// </summary>
public partial class Student : BaseModel
{
    /// <summary>
    /// Gets or sets the first name.
    /// </summary>
    public string? FirstName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the last name.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Gets or sets the class id.
    /// </summary>
    public int? ClassId { get; set; }

    /// <summary>
    /// Gets or sets the date of birth.
    /// </summary>
    public DateOnly? DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets sex (male/female).
    /// </summary>
    public bool? Sex { get; set; }

    /// <summary>
    /// Gets or sets the class object.
    /// </summary>
    public virtual Class? Class { get; set; } = null!;

    /// <summary>
    /// Gets or sets the marks list.
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public virtual ICollection<Mark> Marks { get; set; } = null!;

    /// <summary>
    /// Overridden ToString().
    /// </summary>
    /// <returns>The string representation of the student object with the firstname and the last name.</returns>
    public override string ToString()
    {
        return $"{this.FirstName} {this.LastName}";
    }
}
