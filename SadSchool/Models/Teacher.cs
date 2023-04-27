using System;
using System.Collections.Generic;

namespace SadSchool.Models;

public partial class Teacher
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public int? Grade { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
