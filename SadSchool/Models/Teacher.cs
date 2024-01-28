﻿using System;
using System.Collections.Generic;

namespace SadSchool.Models;

public partial class Teacher : BaseModel
{
    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string? DateOfBirth { get; set; }

    public int? Grade { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<ScheduledLesson> Lessons { get; set; } = new List<ScheduledLesson>();

    public override string ToString() =>
         $"{FirstName} {LastName}";
}
