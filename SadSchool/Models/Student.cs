﻿using System;
using System.Collections.Generic;

namespace SadSchool.Models;

public partial class Student
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? LastName { get; set; }

    public int ClassId { get; set; }

    public string? DateOfBirth { get; set; }

    public bool? Sex { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual ICollection<Mark> Marks { get; set; } = null!;
}
