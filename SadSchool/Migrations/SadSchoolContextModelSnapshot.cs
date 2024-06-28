﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SadSchool.DbContexts;

#nullable disable

namespace SadSchool.Migrations
{
    [DbContext(typeof(SadSchoolContext))]
    partial class SadSchoolContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SadSchool.Models.Class", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<int?>("LeaderId")
                        .HasColumnType("int")
                        .HasColumnName("leader_id");

                    b.Property<string>("Name")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasColumnName("name");

                    b.Property<int?>("TeacherId")
                        .HasColumnType("int")
                        .HasColumnName("teacher_id");

                    b.HasKey("Id");

                    b.HasIndex("TeacherId");

                    b.ToTable("class", (string)null);
                });

            modelBuilder.Entity("SadSchool.Models.Lesson", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<string>("Date")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("date");

                    b.Property<int?>("ScheduledLessonId")
                        .HasColumnType("int")
                        .HasColumnName("scheduled_lesson_id");

                    b.HasKey("Id");

                    b.HasIndex("ScheduledLessonId");

                    b.ToTable("lesson", (string)null);
                });

            modelBuilder.Entity("SadSchool.Models.Mark", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<int?>("LessonId")
                        .HasColumnType("int")
                        .HasColumnName("lesson_id");

                    b.Property<int?>("StudentId")
                        .HasColumnType("int")
                        .HasColumnName("student_id");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("value");

                    b.HasKey("Id");

                    b.HasIndex("LessonId");

                    b.HasIndex("StudentId");

                    b.ToTable("mark", (string)null);
                });

            modelBuilder.Entity("SadSchool.Models.ScheduledLesson", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<int?>("ClassId")
                        .HasColumnType("int")
                        .HasColumnName("class_id");

                    b.Property<string>("Day")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("day");

                    b.Property<int?>("StartTimeId")
                        .HasColumnType("int")
                        .HasColumnName("start_time_id");

                    b.Property<int?>("SubjectId")
                        .HasColumnType("int")
                        .HasColumnName("subject_id");

                    b.Property<int?>("TeacherId")
                        .HasColumnType("int")
                        .HasColumnName("teacher_id");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.HasIndex("StartTimeId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TeacherId");

                    b.ToTable("scheduled_lessons", (string)null);
                });

            modelBuilder.Entity("SadSchool.Models.StartTime", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("value");

                    b.HasKey("Id");

                    b.ToTable("start_time", (string)null);
                });

            modelBuilder.Entity("SadSchool.Models.Student", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<int?>("ClassId")
                        .HasColumnType("int")
                        .HasColumnName("class_id");

                    b.Property<DateOnly?>("DateOfBirth")
                        .HasColumnType("date")
                        .HasColumnName("date_of_birth");

                    b.Property<string>("FirstName")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasColumnName("last_name");

                    b.Property<bool?>("Sex")
                        .HasColumnType("bit")
                        .HasColumnName("sex");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.ToTable("student", (string)null);
                });

            modelBuilder.Entity("SadSchool.Models.Subject", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<string>("Name")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("subject", (string)null);
                });

            modelBuilder.Entity("SadSchool.Models.Teacher", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<DateOnly?>("DateOfBirth")
                        .HasColumnType("date")
                        .HasColumnName("date_of_birth");

                    b.Property<string>("FirstName")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("first_name");

                    b.Property<int?>("Grade")
                        .HasColumnType("int")
                        .HasColumnName("grade");

                    b.Property<string>("LastName")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasColumnName("last_name");

                    b.HasKey("Id");

                    b.ToTable("teacher", (string)null);
                });

            modelBuilder.Entity("SadSchool.Models.Class", b =>
                {
                    b.HasOne("SadSchool.Models.Teacher", "Teacher")
                        .WithMany("Classes")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK_class_teacher");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("SadSchool.Models.Lesson", b =>
                {
                    b.HasOne("SadSchool.Models.ScheduledLesson", "ScheduledLesson")
                        .WithMany("Lessons")
                        .HasForeignKey("ScheduledLessonId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK_mark_scheduled_lessons");

                    b.Navigation("ScheduledLesson");
                });

            modelBuilder.Entity("SadSchool.Models.Mark", b =>
                {
                    b.HasOne("SadSchool.Models.Lesson", "Lesson")
                        .WithMany("Marks")
                        .HasForeignKey("LessonId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK_mark_lesson");

                    b.HasOne("SadSchool.Models.Student", "Student")
                        .WithMany("Marks")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK_mark_student");

                    b.Navigation("Lesson");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("SadSchool.Models.ScheduledLesson", b =>
                {
                    b.HasOne("SadSchool.Models.Class", "Class")
                        .WithMany("Lessons")
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK_lesson_class");

                    b.HasOne("SadSchool.Models.StartTime", "StartTime")
                        .WithMany("Lessons")
                        .HasForeignKey("StartTimeId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK_lesson_schedule");

                    b.HasOne("SadSchool.Models.Subject", "Subject")
                        .WithMany("Lessons")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK_lesson_subject");

                    b.HasOne("SadSchool.Models.Teacher", "Teacher")
                        .WithMany("Lessons")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK_lesson_teacher");

                    b.Navigation("Class");

                    b.Navigation("StartTime");

                    b.Navigation("Subject");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("SadSchool.Models.Student", b =>
                {
                    b.HasOne("SadSchool.Models.Class", "Class")
                        .WithMany("Students")
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK_student_class");

                    b.Navigation("Class");
                });

            modelBuilder.Entity("SadSchool.Models.Class", b =>
                {
                    b.Navigation("Lessons");

                    b.Navigation("Students");
                });

            modelBuilder.Entity("SadSchool.Models.Lesson", b =>
                {
                    b.Navigation("Marks");
                });

            modelBuilder.Entity("SadSchool.Models.ScheduledLesson", b =>
                {
                    b.Navigation("Lessons");
                });

            modelBuilder.Entity("SadSchool.Models.StartTime", b =>
                {
                    b.Navigation("Lessons");
                });

            modelBuilder.Entity("SadSchool.Models.Student", b =>
                {
                    b.Navigation("Marks");
                });

            modelBuilder.Entity("SadSchool.Models.Subject", b =>
                {
                    b.Navigation("Lessons");
                });

            modelBuilder.Entity("SadSchool.Models.Teacher", b =>
                {
                    b.Navigation("Classes");

                    b.Navigation("Lessons");
                });
#pragma warning restore 612, 618
        }
    }
}
