using System;
using System.Collections.Generic;

namespace SadSchool.Models;

public partial class Class
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int StudentsQuantity { get; set; }

    public int? TeacherId { get; set; }

    public int? LeaderId { get; set; }

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    public virtual Teacher? Teacher { get; set; }
}
