using System;
using System.Collections.Generic;

namespace SadSchool.Models;

public partial class Subject
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
