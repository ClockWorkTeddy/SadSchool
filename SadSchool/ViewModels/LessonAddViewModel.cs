using SadSchool.Models;

namespace SadSchool.ViewModels
{
    public class LessonAddViewModel
    {
        public List<Class?> ClassesForView { get; set; } = new List<Class?>();
        public List<Subject?> SubjectsForView { get; set; } = new List<Subject?>();
        public List<Teacher?> TeachersForView { get; set; } = new List<Teacher?>();
        public List<StartTime?> SchedulesForView { get; set; } = new List<StartTime?>();
        public int ClassId { get; set; }
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }
        public int ScheduleId { get; set; }
        public string Date { get; set; }
    }
}
