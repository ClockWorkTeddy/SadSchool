// <copyright file="ScheduleService.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Services.Schedule
{
    using SadSchool.Models;

    /// <summary>
    /// Service for schedule.
    /// </summary>
    public class ScheduleService
    {
        private List<ScheduledLesson> scheduledLessons = new();
        private List<ScheduleCell> unsortedCells = new();
        private List<string?> classes = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleService"/> class.
        /// </summary>
        /// <param name="scheduledLessons">List of sheculed lessons objects.</param>
        public ScheduleService(List<ScheduledLesson> scheduledLessons)
        {
            this.scheduledLessons = scheduledLessons;
        }

        /// <summary>
        /// Gets class names collection.
        /// </summary>
        public List<string?> Classes => this.classes;

        /// <summary>
        /// Gets the schedule cells.
        /// </summary>
        /// <returns>Schedule cells table.</returns>
        public ScheduleCell[,] GetScheduleCells()
        {
            this.GetOverallCells();

            return this.SortCells()!;
        }

        private void GetOverallCells()
        {
            List<ScheduleCell> cells = new();

            foreach (var scheduledLesson in this.scheduledLessons)
            {
                var cell = this.unsortedCells.FirstOrDefault(c =>
                       c.Day == scheduledLesson.Day
                    && c.ClassName == scheduledLesson?.Class?.Name);

                if (cell == null)
                {
                    cell = new ScheduleCell
                    {
                        Day = scheduledLesson?.Day,
                        ClassName = scheduledLesson?.Class?.Name,
                        LessonInfos = new List<LessonInfo>
                        {
                            new LessonInfo
                            {
                                Teacher = $"{scheduledLesson?.Teacher?.FirstName} {scheduledLesson?.Teacher?.LastName}",
                                StartTime = scheduledLesson?.StartTime?.Value,
                                Name = scheduledLesson?.Subject?.Name,
                            },
                        },
                    };

                    this.unsortedCells.Add(cell);
                }
                else
                {
                    cell?.LessonInfos?.Add(new LessonInfo
                    {
                        Teacher = $"{scheduledLesson?.Teacher?.FirstName} {scheduledLesson?.Teacher?.LastName}",
                        StartTime = scheduledLesson?.StartTime?.Value,
                        Name = scheduledLesson?.Subject?.Name,
                    });
                }
            }
        }

        private ScheduleCell?[,] SortCells()
        {
            this.GetClasses();
            var days = Enum.GetNames(typeof(Days));
            ScheduleCell?[,] table = new ScheduleCell[days.Length, this.classes.Count];

            for (int i = 0; i < days.Length; i++)
            {
                for (int j = 0; j < this.classes.Count; j++)
                {
                    table[i, j] = this.unsortedCells.FirstOrDefault(cell => this.Check(cell, days[i], j));
                }
            }

            return table;
        }

        private bool Check(ScheduleCell cell, string day, int classIndex) =>
            cell.Day == day && cell.ClassName == this.classes[classIndex];

        private void GetClasses()
        {
            this.classes = this.unsortedCells.Select(cell => cell.ClassName).Distinct().ToList();
            this.classes.OrderBy(s => s, new MixedNumericStringComparer()).ToList();
        }
    }
}
