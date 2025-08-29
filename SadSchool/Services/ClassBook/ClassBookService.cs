// <copyright file="ClassBookService.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Services.ClassBook
{
    using SadSchool.Contracts;
    using SadSchool.Contracts.Repositories;
    using SadSchool.Models.Mongo;
    using SadSchool.Models.SqlServer;
    using SadSchool.ViewModels;
    using Serilog;

    /// <summary>
    /// Service for class book data.
    /// </summary>
    public class ClassBookService : IClassBookService
    {
        private readonly List<Mark> rawMarks = [];
        private readonly IMarkRepository markRepository;
        private readonly IDerivedRepositories derivedRepositories;

        private List<string> dates = [];
        private List<string> students = [];
        private List<MarkCellDto> markCells = [];
        private string className = string.Empty;
        private string subjectName = string.Empty;
        private MarkCellDto[,]? markCellsTable = new MarkCellDto[0, 0];

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassBookService"/> class.
        /// </summary>
        /// <param name="lessonRepository">Lesson repo instance.</param>
        /// <param name="markRepository">Mark repo instance.</param>
        /// <param name="derivedRepositories">Derived repositories instance.</param>
        public ClassBookService(ILessonRepository lessonRepository, IMarkRepository markRepository, IDerivedRepositories derivedRepositories)
        {
            this.derivedRepositories = derivedRepositories;
            this.Dates = [];
            this.Students = [];
            this.markRepository = markRepository;
            this.derivedRepositories = derivedRepositories;
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
        public async Task<ClassBookViewModel> GetClassBookViewModel(string subjectName, string className)
        {
            this.className = className;
            this.subjectName = subjectName;

            await this.GetRawMarks();
            await this.GetMarkData();
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
        /// <returns>Task for asynchronous operation.</returns>
        public async Task GetMarkData()
        {
            Log.Information("ClassBookService.GetMarkData(): method called.");

            var markCellsQuery = this.rawMarks.Select(async mc => await this.GetMarkCellData(mc));

            this.markCells = (await Task.WhenAll(markCellsQuery)).ToList();

            this.GetDates();
            this.GetStudents();
        }

        private async Task<MarkCellDto> GetMarkCellData(Mark mc)
        {
            Log.Debug("ClassBookService.GetMarkCellData(): method called with Mark.Id = {MarkId}", mc.Id);
            var lesson = await this.GetLessonData(mc);
            var student = await this.derivedRepositories.StudentRepository.GetEntityByIdAsync<Student>(mc.StudentId!.Value);

            return new MarkCellDto
            {
                Date = $"{lesson.Date} {lesson.ScheduledLesson?.StartTime?.Value}",
                Mark = mc.Value,
                StudentName = $"{student?.LastName} {student?.FirstName}",
            };
        }

        private async Task<Lesson> GetLessonData(Mark mc)
        {
            Log.Debug("ClassBookService.GetLessonData(): method called with Mark.LessonId = {LessonId}", mc.LessonId);
            var lessons = await this.derivedRepositories.LessonRepository.GetAllEntitiesAsync<Lesson>();
            return lessons
                .First(l => l.Id == mc.LessonId);
        }

        private void GetMarkTable()
        {
            Log.Debug("ClassBookService.GetMarkTable(): method called.");

            this.markCellsTable = new MarkCellDto[this.students.Count, this.dates.Count];

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

        private async Task GetRawMarks()
        {
            Log.Debug("ClassBookService.GetRawMarks(): method called.");

            var allMarks = await this.markRepository.GetAllMarksAsync();
            var allLessons = await this.derivedRepositories.LessonRepository.GetAllEntitiesAsync<Lesson>();

            foreach (var mark in allMarks)
            {
                var scheduledLesson = allLessons.FirstOrDefault(l => l.Id == mark.LessonId)?.ScheduledLesson;

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
