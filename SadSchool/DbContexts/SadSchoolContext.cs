// <copyright file="SadSchoolContext.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.DbContexts;

using Microsoft.EntityFrameworkCore;
using SadSchool.Models.SqlServer;

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
    /// Fluent API configuration.
    /// </summary>
    /// <param name="modelBuilder">Model builder object.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.ScheduledLessonId).HasColumnName("scheduled_lesson_id");

            entity.HasOne(d => d.ScheduledLesson)
                .WithMany(p => p.Lessons)
                .HasForeignKey(d => d.ScheduledLessonId)
                .OnDelete(DeleteBehavior.SetNull);
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

    /// <summary>
    /// Methos for db context configuration.
    /// </summary>
    /// <param name="optionsBuilder">Options builder object.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseLazyLoadingProxies();

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
