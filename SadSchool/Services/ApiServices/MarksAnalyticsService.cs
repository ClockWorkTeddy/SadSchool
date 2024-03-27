﻿// <copyright file="MarksAnalyticsService.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Services.ApiServices
{
    using Microsoft.EntityFrameworkCore;
    using SadSchool.Controllers.Contracts;
    using SadSchool.Models;

    /// <summary>
    /// Marks analytics service.
    /// </summary>
    public class MarksAnalyticsService : IMarksAnalyticsService
    {
        private readonly ICacheService cacheService;
        private SadSchoolContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarksAnalyticsService"/> class.
        /// </summary>
        /// <param name="context">DB context instance.</param>
        /// <param name="cacheService">Cache servece instance.</param>
        public MarksAnalyticsService(SadSchoolContext context, ICacheService cacheService)
        {
            this.context = context;
            this.cacheService = cacheService;
        }

        /// <summary>
        /// Returns the average marks for the student and the subject.
        /// </summary>
        /// <param name="studentId">The id of the desirable student.</param>
        /// <param name="subjectId">The id of the desirable subject.</param>
        /// <returns>Average marks collection for selected student and subject
        ///     (for all students\subject if none selected).</returns>
        public List<AverageMarkModel> GetAverageMarks(int studentId, int subjectId)
        {
            List<AverageMarkModel> averageMarks = [];
            var students = this.GetStudents(studentId);
            var subjects = this.GetSubjects(subjectId);

            foreach (var student in students)
            {
                foreach (var subject in subjects)
                {
                    var averageMark = this.GetAveragesMark(student, subject);

                    if (averageMark.MarkValue != 0)
                    {
                        averageMarks.Add(averageMark);
                    }
                }
            }

            return averageMarks;
        }

        private List<Subject> GetSubjects(int? subjectId)
        {
            if (subjectId == null || subjectId < 1)
            {
                return this.context.Set<Subject>().ToList();
            }
            else
            {
                return this.cacheService.GetObject<Subject>(subjectId.Value);
            }
        }

        private List<Student> GetStudents(int? studentId)
        {
            if (studentId == null || studentId < 1)
            {
                return this.context.Students.ToList();
            }
            else
            {
                return this.cacheService.GetObject<Student>(studentId.Value);
            }
        }

        private AverageMarkModel GetAveragesMark(Student student, Subject subject)
        {
            var marks = this.context.Marks
                .Include(m => m.Student)
                .Include(m => m.Lesson)
                    .ThenInclude(l => l!.ScheduledLesson)
                        .ThenInclude(sl => sl!.Subject).ToList();

            var foundMarks = marks.Where(m => m?.Student?.ToString() == student.ToString()
                && m?.Lesson?.ScheduledLesson?.Subject?.Name == subject.Name).ToList();

            var sum = foundMarks.Sum(mdr => int.Parse(mdr.Value));

            return new AverageMarkModel
            {
                StudentName = student.ToString(),
                SubjectName = subject.Name,
                MarkValue = foundMarks.Count == 0 ? 0.0 : (double)sum / foundMarks.Count,
            };
        }
    }
}
