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
        public string ClassName { get; set; }
        public List<LessonInfo> LessonInfos { get; set; }
    }
}
