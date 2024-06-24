// <copyright file="MarksAnalyticsService.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Services.ApiServices
{
    using SadSchool.Controllers.Contracts;
    using SadSchool.Models;
    using Serilog;

    /// <summary>
    /// Marks analytics service.
    /// </summary>
    public class MarksAnalyticsService : IMarksAnalyticsService
    {
        private readonly ICacheService cacheService;
        private MongoContext mongoContext;
        private SadSchoolContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarksAnalyticsService"/> class.
        /// </summary>
        /// <param name="mongoContext">DB context instance.</param>
        /// <param name="cacheService">Cache servece instance.</param>
        /// <param name="context">SadSchool context.</param>
        public MarksAnalyticsService(MongoContext mongoContext, ICacheService cacheService, SadSchoolContext context)
        {
            this.context = context;
            this.mongoContext = mongoContext;
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
            Log.Information("MarksAnalyticsService.GetAverageMarks(): method called with parameters: studentId = {studentId}, subjectId = {subjectId}", studentId, subjectId);

            List<AverageMarkModel> averageMarks = [];
            var students = this.GetStudents(studentId);
            var subjects = this.GetSubjects(subjectId);

            foreach (var student in students)
            {
                foreach (var subject in subjects)
                {
                    var averageMark = this.GetAveragesMark(student, subject!);

                    if (averageMark.MarkValue != 0)
                    {
                        averageMarks.Add(averageMark);
                    }
                }
            }

            return averageMarks;
        }

        private List<Subject?> GetSubjects(int? subjectId)
        {
            Log.Information("MarksAnalyticsService.GetSubjects(): method called with parameter: subjectId = {subjectId}", subjectId);

            if (subjectId == null || subjectId < 1)
            {
                return this.context.Set<Subject>().ToList<Subject?>();
            }
            else
            {
                return this.cacheService.GetObject<Subject>(subjectId.Value);
            }
        }

        private List<Student> GetStudents(int? studentId)
        {
            Log.Information("MarksAnalyticsService.GetStudents(): method called with parameter: studentId = {studentId}", studentId);

            if (studentId == null || studentId < 1)
            {
                return this.context.Students.ToList();
            }
            else
            {
                return this.cacheService.GetObject<Student>(studentId.Value)!;
            }
        }

        private AverageMarkModel GetAveragesMark(Student student, Subject subject)
        {
            Log.Information("MarksAnalyticsService.GetAveragesMark(): method called with parameters: student = {student}, subject = {subject}", student, subject);

            var marksForStudent = this.mongoContext.Marks
                .Where(m => m.StudentId == student.Id).ToList();

            var foundMarks = marksForStudent.Where(m => m.LessonId != null
                && this.context.Lessons.Find(m.LessonId)!.ScheduledLesson!.Subject!.Name == subject.Name).ToList();

            var sum = foundMarks.Sum(mdr => int.Parse(mdr.Value!));

            return new AverageMarkModel
            {
                StudentName = student.ToString(),
                SubjectName = subject.Name,
                MarkValue = foundMarks.Count == 0 ? 0.0 : (double)sum / foundMarks.Count,
            };
        }
    }
}
