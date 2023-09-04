using System;
using System.Collections.Generic;

namespace SadSchool.Models;

public partial class StartTime
{
    public int Id { get; set; }

    public string? Value { get; set; }

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
