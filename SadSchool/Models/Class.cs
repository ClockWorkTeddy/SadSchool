using System;
using System.Collections.Generic;

namespace SadSchool.Models;

public partial class Class : BaseModel
{
    public string? Name { get; set; } = null!;

    public int? TeacherId { get; set; }

    public int? LeaderId { get; set; }

    public virtual ICollection<ScheduledLesson>? Lessons { get; set; } = new List<ScheduledLesson>();

    public virtual ICollection<Student>? Students { get; set; } = new List<Student>();

    public virtual Teacher? Teacher { get; set; }
}
