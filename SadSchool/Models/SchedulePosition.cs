using System;
using System.Collections.Generic;

namespace SadSchool.Models;

public partial class SchedulePosition
{
    public int Id { get; set; }

    public TimeSpan StartTime { get; set; }

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
