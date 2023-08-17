﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SadSchool.Models;

#nullable disable

namespace SadSchool.Migrations
{
    [DbContext(typeof(SadSchoolContext))]
    [Migration("20230814091322_nullable_calss_student")]
    partial class nullable_calss_student
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.9");

            modelBuilder.Entity("SadSchool.Models.Class", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<int?>("LeaderId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("leader_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("TEXT")
                        .HasColumnName("name");

                    b.Property<int?>("TeacherId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("teacher_id");

                    b.HasKey("Id");

                    b.HasIndex("TeacherId");

                    b.ToTable("class", (string)null);
                });

            modelBuilder.Entity("SadSchool.Models.Lesson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<int>("ClassId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("class_id");

                    b.Property<string>("Date")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("date");

                    b.Property<int>("StartTimeId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("start_time_id");

                    b.Property<int>("SubjectId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("subject_id");

                    b.Property<int>("TeacherId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("teacher_id");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.HasIndex("StartTimeId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TeacherId");

                    b.ToTable("lesson", (string)null);
                });

            modelBuilder.Entity("SadSchool.Models.Mark", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<int?>("LessonId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("lesson_id");

                    b.Property<int?>("StudentId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("student_id");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("value");

                    b.HasKey("Id");

                    b.HasIndex("LessonId");

                    b.HasIndex("StudentId");

                    b.ToTable("mark", (string)null);
                });

            modelBuilder.Entity("SadSchool.Models.StartTime", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("value");

                    b.HasKey("Id");

                    b.ToTable("start_time", (string)null);
                });

            modelBuilder.Entity("SadSchool.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<int?>("ClassId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("class_id");

                    b.Property<string>("DateOfBirth")
                        .HasColumnType("TEXT")
                        .HasColumnName("date_of_birth");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .HasMaxLength(30)
                        .HasColumnType("TEXT")
                        .HasColumnName("last_name");

                    b.Property<bool?>("Sex")
                        .HasColumnType("INTEGER")
                        .HasColumnName("sex");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.ToTable("student", (string)null);
                });

            modelBuilder.Entity("SadSchool.Models.Subject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("subject", (string)null);
                });

            modelBuilder.Entity("SadSchool.Models.Teacher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<string>("DateOfBirth")
                        .HasColumnType("TEXT")
                        .HasColumnName("date_of_birth");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT")
                        .HasColumnName("first_name");

                    b.Property<int?>("Grade")
                        .HasColumnType("INTEGER")
                        .HasColumnName("grade");

                    b.Property<string>("LastName")
                        .HasMaxLength(30)
                        .HasColumnType("TEXT")
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
                    b.HasOne("SadSchool.Models.Class", "Class")
                        .WithMany("Lessons")
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("FK_lesson_class");

                    b.HasOne("SadSchool.Models.StartTime", "ScheduledPosition")
                        .WithMany("Lessons")
                        .HasForeignKey("StartTimeId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("FK_lesson_schedule_position");

                    b.HasOne("SadSchool.Models.Subject", "Subject")
                        .WithMany("Lessons")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("FK_lesson_subject");

                    b.HasOne("SadSchool.Models.Teacher", "Teacher")
                        .WithMany("Lessons")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("FK_lesson_teacher");

                    b.Navigation("Class");

                    b.Navigation("ScheduledPosition");

                    b.Navigation("Subject");

                    b.Navigation("Teacher");
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
