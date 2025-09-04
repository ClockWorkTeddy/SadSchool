// <copyright file="MarksAnalyticsService.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Services.ApiServices
{
    using SadSchool.Contracts;
    using SadSchool.Contracts.Repositories;
    using SadSchool.DbContexts;
    using SadSchool.Dtos;
    using SadSchool.Models.Mongo;
    using SadSchool.Models.SqlServer;
    using Serilog;

    /// <summary>
    /// Marks analytics service.
    /// </summary>
    public class MarksAnalyticsService : IMarksAnalyticsService
    {
        private readonly IMarkRepository markRepository;
        private readonly IDerivedRepositories derivedRepositories;
        private readonly ISubjectRepository subjectRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarksAnalyticsService"/> class.
        /// </summary>
        /// <param name="markRepository">Mark repo instance.</param>
        /// <param name="derivedRepositories">Derived repo collection instance.</param>
        /// <param name="subjectRepository">Subject repo instance.</param>
        public MarksAnalyticsService(
            IMarkRepository markRepository,
            IDerivedRepositories derivedRepositories,
            ISubjectRepository subjectRepository)
        {
            this.markRepository = markRepository;
            this.derivedRepositories = derivedRepositories;
            this.subjectRepository = subjectRepository;
        }

        /// <summary>
        /// Returns the average marks for the student and the subject.
        /// </summary>
        /// <param name="studentId">The id of the desirable student.</param>
        /// <param name="subjectId">The id of the desirable subject.</param>
        /// <returns>Average marks collection for selected student and subject
        ///     (for all students\subject if none selected).</returns>
        public async Task<List<AverageMarkDto>> GetAverageMarks(int studentId, int subjectId)
        {
            Log.Information("MarksAnalyticsService.GetAverageMarks(): method called with parameters: studentId = {StudentId}, ubjectId = {SubjectId}", studentId, subjectId);

            List<AverageMarkDto> averageMarks = [];
            var students = await this.GetStudents(studentId);
            var subjects = await this.GetSubjects(subjectId);

            foreach (var student in students)
            {
                foreach (var subject in subjects)
                {
                    var averageMark = await this.GetAveragesMark(student!, subject!);

                    if (!averageMark.MarkValue.Equals(0))
                    {
                        averageMarks.Add(averageMark);
                    }
                }
            }

            return averageMarks;
        }

        private async Task<List<Subject?>> GetSubjects(int? subjectId)
        {
            Log.Information("MarksAnalyticsService.GetSubjects(): method called with parameter: subjectId = {SubjectId}", subjectId);

            if (subjectId == null || subjectId < 1)
            {
                var subjects = await this.subjectRepository.GetAllEntitiesAsync<Subject>();
                return subjects.ToList<Subject?>();
            }
            else
            {
                return [await this.derivedRepositories.StudentRepository.GetEntityByIdAsync<Subject>(subjectId.Value)];
            }
        }

        private async Task<List<Student?>> GetStudents(int? studentId)
        {
            Log.Information("MarksAnalyticsService.GetStudents(): method called with parameter: studentId = {StudentId}", studentId);

            if (studentId == null || studentId < 1)
            {
                var students = await this.derivedRepositories.StudentRepository.GetAllEntitiesAsync<Student>();
                return students.ToList<Student?>();
            }
            else
            {
                var student = await this.derivedRepositories.StudentRepository.GetEntityByIdAsync<Student>(studentId.Value);
                return [student]!;
            }
        }

        private async Task<AverageMarkDto> GetAveragesMark(Student student, Subject subject)
        {
            Log.Information("MarksAnalyticsService.GetAveragesMark(): method called with parameters: student = {Student}, subject = {Subject}", student, subject);

            var marksForStudent = await this.markRepository.GetMarksByStudentIdAsync(student.Id!.Value);

            var foundMarks = new List<Mark>();
            var lessons = await this.derivedRepositories.LessonRepository.GetAllEntitiesAsync<Lesson>();

            foreach (var m in marksForStudent)
            {
                var lesson = lessons.FirstOrDefault(l => l.Id == m.LessonId);

                if (lesson?.ScheduledLesson?.Subject?.Name == subject.Name)
                {
                    foundMarks.Add(m);
                }
            }

            var sum = foundMarks.Sum(mdr => int.Parse(mdr.Value!));

            return new AverageMarkDto
            {
                StudentName = student.ToString(),
                SubjectName = subject.Name,
                MarkValue = foundMarks.Count == 0 ? 0.0 : (double)sum / foundMarks.Count,
            };
        }
    }
}
