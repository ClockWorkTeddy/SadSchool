using System;
using System.Collections.Generic;

namespace SadSchool.Models;

public partial class Subject : BaseModel
{
    public string Name { get; set; } = null!;

    public virtual ICollection<ScheduledLesson> Lessons { get; set; } = new List<ScheduledLesson>();
}
