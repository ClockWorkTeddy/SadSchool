using SadSchool.Models;

namespace SadSchool.Services.Schedule
{
    public class ScheduleService
    {
        private List<ScheduledLesson> _scheduledLessons = new();
        private List<ScheduleCell> _unsortedCells = new();
        private List<string> _classes = new();

        public List<string> Classes => _classes;

        public ScheduleService(List<ScheduledLesson> scheduledLessons)
        {
            _scheduledLessons = scheduledLessons;
        }

        public ScheduleCell[,] GetScheduleCells()
        {
            GetOverallCells();
            
            return SortCells();
        }

        private void GetOverallCells()
        {
            List<ScheduleCell> cells = new();

            foreach (var scheduledLesson in _scheduledLessons)
            {
                var cell = _unsortedCells.FirstOrDefault(c => c.Day == scheduledLesson.Day && c.ClassName == scheduledLesson.Class.Name);

                if (cell == null)
                {
                    cell = new ScheduleCell
                    {
                        Day = scheduledLesson.Day,
                        ClassName = scheduledLesson.Class.Name,
                        LessonInfos = new List<LessonInfo> 
                        { 
                            new LessonInfo
                            {
                                Teacher = $"{scheduledLesson.Teacher.FirstName} {scheduledLesson.Teacher.LastName}",
                                StartTime = scheduledLesson.StartTime.Value,
                                Name = scheduledLesson.Subject.Name
                            }
                        }
                    };

                    _unsortedCells.Add(cell);
                }
                else
                {
                    cell.LessonInfos.Add(new LessonInfo
                    {
                        Teacher = $"{scheduledLesson.Teacher.FirstName} {scheduledLesson.Teacher.LastName}",
                        StartTime = scheduledLesson.StartTime.Value,
                        Name = scheduledLesson.Subject.Name
                    });
                }
            }
        }

        private ScheduleCell[,] SortCells()
        {
            GetClasses();
            List<string> days = new List<string>() { "Mon", "Tue", "Wed", "Thu", "Fri"};
            int daysCount = 5;
            ScheduleCell[,] table = new ScheduleCell[daysCount, _classes.Count];

            for (int i = 0; i < daysCount; i++)
                for (int j = 0; j < _classes.Count; j++)
                    table[i, j] = _unsortedCells.FirstOrDefault(cell => cell.Day == days[i] && cell.ClassName == _classes[j]);

            return table;
        }

        private void GetClasses()
        {
            _classes = _unsortedCells.Select(cell => cell.ClassName).Distinct().ToList();
            _classes.OrderBy(s => s, new MixedNumericStringComparer()).ToList();
        }
    }

    public class MixedNumericStringComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            int numX, numY;
            string alphaX, alphaY;

            // Separate numeric and non-numeric parts
            SplitNumericAndAlpha(x, out numX, out alphaX);
            SplitNumericAndAlpha(y, out numY, out alphaY);

            // Compare numeric parts first
            int numComparison = numX.CompareTo(numY);

            if (numComparison == 0)
                return string.Compare(alphaX, alphaY, StringComparison.Ordinal);

            return numComparison;
        }

        private void SplitNumericAndAlpha(string input, out int numericPart, out string alphaPart)
        {
            numericPart = 0;
            alphaPart = string.Empty;

            int index = 0;

            while (index < input.Length && char.IsDigit(input[index]))
                numericPart = numericPart * 10 + (input[index++] - '0');

            alphaPart = input.Substring(index);
        }
    }
}
