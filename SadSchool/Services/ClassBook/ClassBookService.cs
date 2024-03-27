// <copyright file="ClassBookService.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Services.ClassBook
{
    using Microsoft.EntityFrameworkCore;
    using SadSchool.Controllers.Contracts;
    using SadSchool.Models;
    using SadSchool.ViewModels;

    /// <summary>
    /// Service for class book data.
    /// </summary>
    public class ClassBookService : IClassBookService
    {
        private SadSchoolContext context;
        private List<string> dates = [];
        private List<string> students = [];
        private List<MarkCellModel> markCells = [];
        private string className = string.Empty;
        private string subjectName = string.Empty;
        private List<Mark> rawMarks = [];
        private MarkCellModel[,] markCellsTable = new MarkCellModel[0, 0];

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassBookService"/> class.
        /// </summary>
        /// <param name="context">DB context.</param>
        public ClassBookService(SadSchoolContext context)
        {
            this.context = context;
            this.Dates = [];
            this.Students = [];
        }

        /// <summary>
        /// Gets or sets the dates.
        /// </summary>
        public List<string> Dates { get; set; }

        /// <summary>
        /// Gets or sets the students.
        /// </summary>
        public List<string> Students { get; set; }

        /// <summary>
        /// Gets the class book view model.
        /// </summary>
        /// <param name="subjectName">The name of the subject.</param>
        /// <param name="className">The name of the class.</param>
        /// <returns>Class book view model instance.</returns>
        public ClassBookViewModel GetClassBookViewModel(string subjectName, string className)
        {
            this.className = className;
            this.subjectName = subjectName;

            this.GetRawMarks();
            this.GetMarkData();
            this.GetMarkTable();

            return new ClassBookViewModel
            {
                ClassName = className,
                SubjectName = subjectName,
                Dates = this.dates,
                Students = this.students,
                MarkCells = this.markCellsTable,
            };
        }

        /// <summary>
        /// Prepares the mark data.
        /// </summary>
        public void GetMarkData()
        {
            this.markCells = this.rawMarks.Select(mc => new MarkCellModel
            {
                Date = $"{mc?.Lesson?.Date} {mc?.Lesson?.ScheduledLesson?.StartTime?.Value}",
                Mark = mc?.Value,
                StudentName = $"{mc?.Student?.LastName} {mc?.Student?.FirstName}",
            }).ToList();

            this.GetDates();
            this.GetStudents();
        }

        private void GetMarkTable()
        {
            this.markCellsTable = new MarkCellModel[this.students.Count, this.dates.Count];

            for (int i = 0; i < this.students.Count; i++)
            {
                for (int j = 0; j < this.dates.Count; j++)
                {
                    this.markCellsTable[i, j] = this.markCells
                        .First(mc => mc.Date == this.dates[j] && mc.StudentName == this.students[i]);
                }
            }
        }

        private void GetRawMarks()
        {
            var allMarks = this.context.Marks
                .Include(m => m.Student)
                .Include(m => m.Lesson!.ScheduledLesson)
                    .ThenInclude(sl => sl!.Subject)
                .Include(m => m.Lesson!.ScheduledLesson)
                    .ThenInclude(sl => sl!.Class)
                .Include(m => m.Lesson!.ScheduledLesson)
                    .ThenInclude(sl => sl!.StartTime)
                .ToList();

            this.rawMarks = allMarks.Where(m => m.Lesson?.ScheduledLesson?.Subject?.Name == this.subjectName
                && m.Lesson?.ScheduledLesson?.Class?.Name == this.className).ToList();
        }

        private void GetDates()
        {
            this.dates = this.markCells.Select(cell => cell.Date).Distinct().ToList();
            this.dates = this.dates.Order().ToList();
        }

        private void GetStudents() =>
            this.students = this.markCells.Select(cell => cell.StudentName).Distinct().Order().ToList();
    }
}
