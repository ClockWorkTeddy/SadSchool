using SadSchool.Services.Schedule;

namespace SadSchool.ViewModels
{
    public class ScheduleViewModel
    {
        public readonly List<string> Days = new() { "Mon", "Tue", "Wed", "Thu", "Fri" };
        public List<string> Classes { get; set; } = new();
        public ScheduleCell[,] Cells { get; set; }
    }
}
