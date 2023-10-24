using SadSchool.Models;

namespace SadSchool.Services.ClassBook
{
    public class ClassBookService
    {
        private List<Mark> _marks = new();
        private List<string> _dates = new();
        private List<string> _students = new();
        private List<MarkCell> _markCells = new();
        
        public MarkCell[,] MarkCells { get; set; }
        public List<string> Dates { get; set; }
        public List<string> Students { get; set; }

        public ClassBookService(List<Mark> marks)
        {
            _marks = marks;
        }

        public void GetMarkRawData(string subjectName)
        {
            List<Mark> subjectMarks = _marks.Where(m => m.Lesson.ScheduledLesson.Subject.Name == subjectName).ToList();
            _markCells = subjectMarks.Select(mc => new MarkCell
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

        public void GetMarkTable()
        {
            for (int i = 0; i < _students.Count; i++)
                for (int j = 0; j < _dates.Count; i++)
                {
                    MarkCells[i, j] = new MarkCell
                    {
                        Date = _dates[j],
                        StudentName = _students[i],
                        Mark = _markCells.First(m => m.Date == _dates[j] && m.StudentName == _students[i]).Mark
                    };
                }
        }
    }
}
