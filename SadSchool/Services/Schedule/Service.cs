using SadSchool.Models;

namespace SadSchool.Services.Schedule
{
    public class Service
    {
        private List<ScheduledLesson> _scheduledLessons = new();

        public Service(List<ScheduledLesson> scheduledLessons)
        {
            _scheduledLessons = scheduledLessons;
        }

        public List<ScheduleCell> GetScheduleCells()
        {
            List<ScheduleCell> cells = new();

            foreach (var scheduledLesson in _scheduledLessons)
            {
                var cell = cells.FirstOrDefault(c => c.Day == scheduledLesson.Day);

                if (cell == null)
                {
                    cell = new ScheduleCell
                    {
                        Day = scheduledLesson.Day,
                        ClassName = scheduledLesson.Class.Name,
                        LessonName = new List<string> { scheduledLesson.Subject.Name }
                    };

                    cells.Add(cell);
                }
                else
                {
                    cell.LessonName.Add(scheduledLesson.Subject.Name);
                }
                cells.Add(cell);
            }
            return cells;
        }
    }
}
