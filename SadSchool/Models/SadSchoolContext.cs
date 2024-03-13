﻿// <copyright file="SadSchoolContext.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Models;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// SadSchool database context.
/// </summary>
public partial class SadSchoolContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SadSchoolContext"/> class. The default constructor.
    /// </summary>
    public SadSchoolContext()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SadSchoolContext"/> class.
    /// </summary>
    /// <param name="options">DB context options.</param>
    public SadSchoolContext(DbContextOptions<SadSchoolContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the classes table.
    /// </summary>
    public virtual DbSet<Class> Classes { get; set; }

    /// <summary>
    /// Gets or sets the lessons table.
    /// </summary>
    public virtual DbSet<Lesson> Lessons { get; set; }

    /// <summary>
    /// Gets or sets the scheduled lessons table.
    /// </summary>
    public virtual DbSet<ScheduledLesson> ScheduledLessons { get; set; }

    /// <summary>
    /// Gets or sets the start times (schedule positions) table.
    /// </summary>
    public virtual DbSet<StartTime> StartTimes { get; set; }

    /// <summary>
    /// Gets or sets the students table.
    /// </summary>
    public virtual DbSet<Student> Students { get; set; }

    /// <summary>
    /// Gets or sets the subjects table.
    /// </summary>
    public virtual DbSet<Subject> Subjects { get; set; }

    /// <summary>
    /// Gets or sets the teachers table.
    /// </summary>
    public virtual DbSet<Teacher> Teachers { get; set; }

    /// <summary>
    /// Gets or sets the marks table.
    /// </summary>
    public virtual DbSet<Mark> Marks { get; set; }

    /// <summary>
    /// Fluent API configuration.
    /// </summary>
    /// <param name="modelBuilder">Model builder object.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Mark>(entity =>
        {
            entity.ToTable("mark");

            entity.HasKey(_ => _.Id);

            entity.Property(_ => _.Id)
                  .ValueGeneratedOnAdd()
                  .HasColumnName("id");

            entity.Property(_ => _.Value)
                  .HasColumnName("value");

            entity.Property(_ => _.LessonId)
                  .HasColumnName("lesson_id");

            entity.HasOne(d => d.Lesson).WithMany(p => p.Marks)
                .HasForeignKey(d => d.LessonId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_mark_lesson");

            entity.Property(_ => _.StudentId)
                  .HasColumnName("student_id");

            entity.HasOne(d => d.Student).WithMany(p => p.Marks)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_mark_student");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.ToTable("class");

            entity.HasKey(c => c.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LeaderId).HasColumnName("leader_id");
            entity.Property(e => e.Name).HasMaxLength(10).HasColumnName("name");
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

            entity.HasOne(d => d.Teacher)
                .WithMany(p => p.Classes)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_class_teacher");
        });

        modelBuilder.Entity<ScheduledLesson>(entity =>
        {
            entity.ToTable("scheduled_lessons");

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.Day).HasColumnName("day");
            entity.Property(e => e.ClassId).HasColumnName("class_id");
            entity.Property(e => e.StartTimeId).HasColumnName("start_time_id");
            entity.Property(e => e.SubjectId).HasColumnName("subject_id");
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

            entity.HasOne(d => d.Class)
                .WithMany(p => p.Lessons)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_lesson_class");

            entity.HasOne(d => d.StartTime).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.StartTimeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_lesson_schedule");

            entity.HasOne(d => d.Subject).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_lesson_subject");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_lesson_teacher");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.ToTable("lesson");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ScheduledLessonId).HasColumnName("scheduled_lesson_id");

            entity.HasOne(d => d.ScheduledLesson)
                .WithMany(p => p.Lessons)
                .HasForeignKey(d => d.ScheduledLessonId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_mark_scheduled_lessons");
        });

        modelBuilder.Entity<StartTime>(entity =>
        {
            entity.ToTable("start_time");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("student");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClassId).HasColumnName("class_id");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.LastName).HasMaxLength(30).HasColumnName("last_name");
            entity.Property(e => e.FirstName).HasMaxLength(20).HasColumnName("first_name");
            entity.Property(e => e.Sex).HasColumnName("sex");

            entity.HasOne(d => d.Class)
                .WithMany(p => p.Students)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_student_class");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.ToTable("subject");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasMaxLength(30).HasColumnName("name");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.ToTable("teacher");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.FirstName).HasMaxLength(20).HasColumnName("first_name");
            entity.Property(e => e.Grade).HasColumnName("grade");
            entity.Property(e => e.LastName).HasMaxLength(30).HasColumnName("last_name");
        });

        this.OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
