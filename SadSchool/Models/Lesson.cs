using System;
using System.Collections.Generic;

namespace SadSchool.Models;

public partial class Lesson
{
    public int Id { get; set; }

    public int ScheduledPositionId { get; set; }

    public int SubjectId { get; set; }

    public int ClassId { get; set; }

    public int TeacherId { get; set; }

    public string Date { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual SchedulePosition ScheduledPosition { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;

    public virtual Teacher Teacher { get; set; } = null!;

    public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();
}
