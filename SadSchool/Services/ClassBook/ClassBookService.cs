// <copyright file="ClassBookService.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Services.ClassBook
{
    using Microsoft.EntityFrameworkCore;
    using SadSchool.Controllers.Contracts;
    using SadSchool.Models;
    using SadSchool.ViewModels;
    using Serilog;

    /// <summary>
    /// Service for class book data.
    /// </summary>
    public class ClassBookService : IClassBookService
    {
        private SadSchoolContext context;
        private MongoContext mongoContext;
        private List<string> dates = [];
        private List<string> students = [];
        private List<MarkCellModel> markCells = [];
        private string className = string.Empty;
        private string subjectName = string.Empty;
        private List<Mark> rawMarks = [];
        private MarkCellModel[,]? markCellsTable = new MarkCellModel[0, 0];

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassBookService"/> class.
        /// </summary>
        /// <param name="context">DB context.</param>
        /// <param name="mongoContext">MongoDB context.</param>
        public ClassBookService(SadSchoolContext context, MongoContext mongoContext)
        {
            this.context = context;
            this.mongoContext = mongoContext;
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
                MarkCells = this.markCellsTable!,
            };
        }

        /// <summary>
        /// Prepares the mark data.
        /// </summary>
        public void GetMarkData()
        {
            Log.Information("ClassBookService.GetMarkData(): method called.");

            this.markCells = this.rawMarks.Select(mc => new MarkCellModel
            {
                Date = $"{this.GetLessonData(mc).Date} {this.GetLessonData(mc)?.ScheduledLesson?.StartTime?.Value}",
                Mark = mc?.Value,
                StudentName = $"{this.GetStudentData(mc!).LastName} {this.GetStudentData(mc!).FirstName}",
            }).ToList();

            this.GetDates();
            this.GetStudents();
        }

        private Lesson GetLessonData(Mark mc)
        {
            Log.Debug($"ClassBookService.GetLessonData(): method called with Mark.LessonId = {mc.LessonId}");

            return this.context.Lessons
                .Include(l => l.ScheduledLesson)
                .ThenInclude(sl => sl!.StartTime)
                .First(l => l.Id == mc.LessonId);
        }

        private Student GetStudentData(Mark mc)
        {
            Log.Debug($"ClassBookService.GetStudentData(): method called with Mark.StudentId = {mc.StudentId}");

            return this.context.Students
                .First(s => s.Id == mc.StudentId);
        }

        private void GetMarkTable()
        {
            Log.Debug("ClassBookService.GetMarkTable(): method called.");

            this.markCellsTable = new MarkCellModel[this.students.Count, this.dates.Count];

            for (int i = 0; i < this.students.Count; i++)
            {
                for (int j = 0; j < this.dates.Count; j++)
                {
                    if (this.dates[j] != null && this.students[i] != null)
                    {
                        var markCell = this.markCells
                            .FirstOrDefault(mc => mc.Date == this.dates[j] && mc.StudentName == this.students[i]);

                        if (markCell != null)
                        {
                            this.markCellsTable[i, j] = markCell;
                        }
                    }
                }
            }
        }

        private void GetRawMarks()
        {
            Log.Debug("ClassBookService.GetRawMarks(): method called.");

            var allMarks = this.mongoContext.Marks.ToList();

            foreach (var mark in this.mongoContext.Marks)
            {
                var scheduledLesson = this.context.Lessons.Find(mark.LessonId)?.ScheduledLesson;

                if (scheduledLesson?.Subject?.Name == this.subjectName 
                    && scheduledLesson?.Class?.Name == this.className)
                {
                    this.rawMarks.Add(mark);
                }
            }
        }

        private void GetDates()
        {
            Log.Debug("ClassBookService.GetDates(): method called.");

            this.dates = this.markCells.Select(cell => cell.Date).Distinct().ToList();
            this.dates = this.dates.Order().ToList();
        }

        private void GetStudents()
        {
            Log.Debug("ClassBookService.GetStudents(): method called.");

            this.students = this.markCells.Select(cell => cell.StudentName).Distinct().Order().ToList();
        }
    }
}
