using SadSchool.Models;

namespace SadSchool.Services.Schedule
{
    public enum ScheduleCellDay
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    };

    public class ScheduleCell
    {
        public string Day { get; set; }
        public string StartTime { get; set; }
        public string ClassName { get; set; }
        public List<string> LessonInfo { get; set; }

        public override string ToString() =>
            String.Join("\r\n", $"{StartTime}: {LessonInfo}");
    }
}
