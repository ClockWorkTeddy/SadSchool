using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SadSchool.Models;

public partial class SadSchoolContext : DbContext
{
    public SadSchoolContext()
    {
    }

    public SadSchoolContext(DbContextOptions<SadSchoolContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<SchedulePosition> SchedulePositions { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

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
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_mark_lesson");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.ToTable("class");

            entity.HasKey(c => c.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.LeaderId).HasColumnName("leader_id");
            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .HasColumnName("name");
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Classes)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK_class_teacher");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.ToTable("lesson");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ClassId).HasColumnName("class_id");
            entity.Property(e => e.ScheduledPositionId).HasColumnName("scheduled_position_id");
            entity.Property(e => e.SubjectId).HasColumnName("subject_id");
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

            entity.HasOne(d => d.Class).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_lesson_class");

            entity.HasOne(d => d.ScheduledPosition).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.ScheduledPositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_lesson_schedule_position");

            entity.HasOne(d => d.Subject).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_lesson_subject");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_lesson_teacher");
        });

        modelBuilder.Entity<SchedulePosition>(entity =>
        {
            entity.ToTable("SchedulePosition");

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.StartTime).HasColumnName("StartTime");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("student");

            entity.Property(e => e.Id)
                .HasColumnName("Id");
            entity.Property(e => e.ClassId).HasColumnName("ClassId");
            entity.Property(e => e.DateOfBirth)
                .HasColumnName("DateOfBirth");
            entity.Property(e => e.LastName)
                .HasMaxLength(30)
                .HasColumnName("LastName");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("Name");
            entity.Property(e => e.Sex).HasColumnName("Sex");

            entity.HasOne(d => d.Class).WithMany(p => p.Students)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_student_class");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.ToTable("subject");

            entity.Property(e => e.Id)
                  .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.ToTable("Teacher");

            entity.Property(e => e.Id)
                  .HasColumnName("Id");
            entity.Property(e => e.DateOfBirth)
                .HasColumnName("DateOfBirth");
            entity.Property(e => e.FirstName)
                .HasMaxLength(20)
                .HasColumnName("FirstName");
            entity.Property(e => e.Grade).HasColumnName("Grade");
            entity.Property(e => e.LastName)
                .HasMaxLength(30)
                .HasColumnName("LastName");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
