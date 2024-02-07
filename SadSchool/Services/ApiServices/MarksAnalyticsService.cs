using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SadSchool.Controllers.Contracts;
using SadSchool.Models;

namespace SadSchool.Services.ApiServices
{
    public interface IMarksAnalyticsService
    {
        List<AverageMark> GetAverageMarks(int studentName, int subjectName);
    }

    public class MarksAnalyticsService : IMarksAnalyticsService
    {
        private SadSchoolContext _context;
        private readonly ICacheService _cacheService;

        public MarksAnalyticsService(SadSchoolContext context, ICacheService caceService)
        {
            _context = context;
            _cacheService = caceService;
        }

        public List<AverageMark> GetAverageMarks(int studentId, int subjectId)
        {
            List<AverageMark> averageMarks = new();
            var students = GetStudents(studentId);
            var subjects = GetSubjects(subjectId);

            foreach (var student in students)
                foreach (var subject in subjects)
                {
                    var averageMark = GetAveragesMark(student, subject);

                    if (averageMark.MarkValue != 0)
                        averageMarks.Add(averageMark);
                }

            return averageMarks;
        }

        private List<Subject> GetSubjects(int? subjectId)
        {
            if (subjectId == null || subjectId < 1)
                return _context.Set<Subject>().ToList();
            else
                return _cacheService.GetObject<Subject>(subjectId.Value);

        }

        private List<Student> GetStudents(int? studentId)
        {
            if (studentId == null || studentId < 1)
                return _context.Students.ToList();
            else
                return _cacheService.GetObject<Student>(studentId.Value);
        }

        private AverageMark GetAveragesMark(Student student, Subject subject)
        {
            var marks = _context.Marks
                .Include(m => m.Student)
                .Include(m => m.Lesson)
                    .ThenInclude(l => l.ScheduledLesson)
                        .ThenInclude(sl => sl.Subject).ToList();

            var foundMarks = marks.Where(m => m.Student.ToString() == student.ToString()
                && m.Lesson.ScheduledLesson.Subject.Name == subject.Name).ToList();

            var sum = foundMarks.Sum(mdr => int.Parse(mdr.Value));

            return new AverageMark
            {
                StudentName = student.ToString(),
                SubjectName = subject.Name,
                MarkValue = foundMarks.Count == 0 ? 0.0 : (double)sum / foundMarks.Count
            };
        }
    }
}
