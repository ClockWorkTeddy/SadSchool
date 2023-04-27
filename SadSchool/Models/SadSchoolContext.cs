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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=V03\\SQLEXPRESS;Initial Catalog=SadSchool;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;Encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Class>(entity =>
        {
            entity.ToTable("class");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.LeaderId).HasColumnName("leader_id");
            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .HasColumnName("name");
            entity.Property(e => e.StudentsQuantity).HasColumnName("students_quantity");
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
            entity.ToTable("schedule_position");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("student");

            entity.Property(e => e.ClassId).HasColumnName("class_id");
            entity.Property(e => e.DateOfBirth)
                .HasColumnType("date")
                .HasColumnName("date_of_birth");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LastName)
                .HasMaxLength(30)
                .HasColumnName("last_name");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
            entity.Property(e => e.Sex).HasColumnName("sex");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.ToTable("subject");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.ToTable("teacher");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.DateOfBirth)
                .HasColumnType("date")
                .HasColumnName("date_of_birth");
            entity.Property(e => e.FirstName)
                .HasMaxLength(20)
                .HasColumnName("first_name");
            entity.Property(e => e.Grade).HasColumnName("grade");
            entity.Property(e => e.LastName)
                .HasMaxLength(30)
                .HasColumnName("last_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
