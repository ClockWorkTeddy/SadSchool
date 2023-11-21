using Microsoft.EntityFrameworkCore;
using SadSchool.Controllers.Contracts;
using SadSchool.Models;
using SadSchool.ViewModels;

namespace SadSchool.Services.ClassBook
{
    public class ClassBookService : IClassBookService
    {
        private SadSchoolContext _context = new();
        private List<string> _dates = new();
        private List<string> _students = new();
        private List<MarkCell> _markCells = new();
        private string _className = "";
        private string _subjectName = "";
        private List<Mark> _rawMarks = new();
        
        private MarkCell[,] _markCellsTable { get; set; }
        public List<string> Dates { get; set; }
        public List<string> Students { get; set; }

        public ClassBookService(SadSchoolContext context)
        {
            _context = context;
        }

        public ClassBookViewModel GetClassBookViewModel(string subjectName, string className)
        {
            _className = className;
            _subjectName = subjectName;

            GetRawMarks();
            GetMarkData();

            return new ClassBookViewModel
            {
                ClassName = className,
                SubjectName = subjectName,
                Dates = _dates,
                Students = _students,
                MarkCells = _markCellsTable
            };
        }

        private void GetRawMarks()
        {
            var allMarks = _context.Marks
                .Include(m => m.Student)
                .Include(m => m.Lesson.ScheduledLesson.Class)
                .Include(m => m.Lesson.ScheduledLesson.Subject)
                .ToList(); 
            
            _rawMarks = allMarks.Where(m => m.Lesson?.ScheduledLesson?.Subject?.Name == _subjectName 
                && m.Lesson?.ScheduledLesson?.Class?.Name == _className).ToList();
        }

        public void GetMarkData()
        {
            _markCells = _rawMarks.Select(mc => new MarkCell
            {
                Date = mc.Lesson.Date,
                Mark = mc.Value,
                StudentName = $"{mc.Student.LastName} {mc.Student.FirstName}"
            }).ToList();

            GetDates();
            GetStudents();
        }

        private void GetDates()
        {
            _dates = _markCells.Select(cell => cell.Date).Distinct().ToList();
            _dates.Order();
        }

        private void GetStudents() =>
            _students = _markCells.Select(cell => cell.StudentName).Distinct().Order().ToList();
    }
}
