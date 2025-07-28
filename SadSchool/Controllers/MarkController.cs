// <copyright file="MarkController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using SadSchool.Contracts;
    using SadSchool.Contracts.Repositories;
    using SadSchool.Dtos;
    using SadSchool.Models.Mongo;
    using SadSchool.Models.SqlServer;
    using SadSchool.ViewModels;

    /// <summary>
    /// Processes <see cref="Mark"/> entities.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="MarkController"/> class.
    /// </remarks>
    /// <param name="subjectRepository">Subject repository instance.</param>"
    /// <param name="derivedRepositories">Derived repositories instance.</param>
    /// <param name="navigationService">Service instance processing Back button logic.</param>
    /// <param name="marksAnalyticsService">Service operates marks analytics.</param>
    /// <param name="authService">Authentication check service instance.</param>
    public class MarkController(
        ISubjectRepository subjectRepository,
        IDerivedRepositories derivedRepositories,
        INavigationService navigationService,
        IMarksAnalyticsService marksAnalyticsService,
        IAuthService authService) : Controller
    {
        private readonly ISubjectRepository subjectRepository = subjectRepository;
        private readonly IDerivedRepositories derivedRepositories = derivedRepositories;
        private readonly INavigationService navigationService = navigationService;
        private readonly IMarksAnalyticsService marksAnalyticsService = marksAnalyticsService;
        private readonly IAuthService authService = authService;

        /// <summary>
        /// Gets marks view.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for marks view.</returns>
        [HttpGet]
        public async Task<IActionResult> Marks()
        {
            var markViewModels = new List<MarkViewModel>();
            var marks = await this.derivedRepositories.MarkRepository.GetAllMarksAsync();

            foreach (var mark in marks)
            {
                var student = await this.derivedRepositories.StudentRepository.GetEntityByIdAsync<Student>(mark.StudentId!.Value);
                var lesson = await this.derivedRepositories.LessonRepository.GetEntityByIdAsync<Lesson>(mark.LessonId!.Value);

                markViewModels.Add(new MarkViewModel
                {
                    Id = mark.Id,
                    Value = mark.Value,
                    Student = $"{student?.LastName} {student?.FirstName}",
                    Lesson = $"{lesson}",
                });
            }

            this.navigationService.RefreshBackParams(this.RouteData);

            return this.View(@"~/Views/Data/Marks.cshtml", markViewModels);
        }

        /// <summary>
        /// Gets add mark view.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for "Add" form or
        ///     <see cref="RedirectToActionResult"/> for action "Marks" in case of failure.</returns>
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            if (this.authService.IsAutorized(this.User))
            {
                MarkViewModel viewModel = new()
                {
                    Students = await this.GetStudentsList(null),
                    Lessons = await this.GetLessonsList(null),
                };

                this.navigationService.RefreshBackParams(this.RouteData);

                return this.View(@"~/Views/Data/MarkAdd.cshtml", viewModel);
            }

            return this.RedirectToAction("Marks");
        }

        /// <summary>
        /// Adds mark insstance to DB.
        /// </summary>
        /// <param name="viewModel"><see cref="MarkViewModel"/> instance with data.</param>
        /// <returns><see cref="RedirectToActionResult"/> to action "Marks" or
        /// <see cref="ViewResult"/> for "Add" form in case of failure.</returns>
        [HttpPost]
        public async Task<IActionResult> Add(MarkViewModel viewModel)
        {
            if (this.ModelState.IsValid)
            {
                var mark = new Mark
                {
                    Value = viewModel.Value,
                    StudentId = viewModel.StudentId,
                    LessonId = viewModel.LessonId,
                };

                await this.derivedRepositories.MarkRepository.AddMarkAsync(mark);

                return this.RedirectToAction("Marks");
            }
            else
            {
                return this.View(@"~/Views/Data/MarkAdd.cshtml", viewModel);
            }
        }

        /// <summary>
        /// Gets edit mark view.
        /// </summary>
        /// <param name="id">Desiarable <see cref="Mark"/> id.</param>
        /// <returns><see cref="ViewResult"/> for "Add" form or
        ///     <see cref="RedirectToActionResult"/> for action "Marks" in case of failure.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(ObjectId id)
        {
            if (this.authService.IsAutorized(this.User))
            {
                var editedMark = await this.derivedRepositories.MarkRepository.GetMarkByIdAsync(id);

                MarkViewModel viewModel = new()
                {
                    Value = editedMark!.Value,
                    Lessons = await this.GetLessonsList(editedMark?.LessonId),
                    Students = await this.GetStudentsList(editedMark?.StudentId),
                };

                this.navigationService.RefreshBackParams(this.RouteData);

                return this.View(@"~/Views/Data/MarkEdit.cshtml", viewModel);
            }

            return this.RedirectToAction("Marks");
        }

        /// <summary>
        /// Edits mark insstance in DB.
        /// </summary>
        /// <param name="viewModel"><see cref="MarkViewModel"/> istance with data.</param>
        /// <returns><see cref="RedirectToActionResult"/> to action "Marks" or
        ///     <see cref="ViewResult"/> for "Edit" form in case of failure.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(MarkViewModel viewModel)
        {
            if (this.authService.IsAutorized(this.User))
            {
                var mark = await this.derivedRepositories.MarkRepository.GetMarkByIdAsync(viewModel.Id);

                if (mark != null)
                {
                    mark.LessonId = viewModel.LessonId;
                    mark.StudentId = viewModel.StudentId;
                    mark.Value = viewModel.Value;

                    await this.derivedRepositories.MarkRepository.UpdateMarkAsync(mark);
                }

                return this.RedirectToAction("Marks");
            }
            else
            {
                return this.View(@"~/Views/Data/MarkEdit.cshtml", viewModel);
            }
        }

        /// <summary>
        /// Deletes mark instance from DB.
        /// </summary>
        /// <param name="id">Desiarable <see cref="Mark"/> id.</param>
        /// <returns><see cref="RedirectToActionResult"/> to action "Marks".</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(ObjectId id)
        {
            if (this.authService.IsAutorized(this.User))
            {
                var mark = await this.derivedRepositories.MarkRepository.GetMarkByIdAsync(id);

                if (mark != null)
                {
                    await this.derivedRepositories.MarkRepository.DeleteMarkByIdAsync(mark.Id);
                }
            }

            return this.RedirectToAction("Marks");
        }

        /// <summary>
        /// Gets student-subject selector view.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for "StudentSubjectSelector" view.</returns>
        [HttpGet]
        public async Task<IActionResult> GetStudentSubject()
        {
            StudentSubjectSelectorViewModel viewModel = new()
            {
                Students = await this.GetStudents(),
                Subjects = await this.GetSubjects(),
            };

            this.navigationService.RefreshBackParams(this.RouteData);

            return this.View(@"~/Views/Data/Representation/StudentSubjectSelector.cshtml", viewModel);
        }

        /// <summary>
        /// Gets average marks view.
        /// </summary>
        /// <param name="viewModel"><see cref="StudentSubjectSelectorViewModel"/> instance with data.</param>
        /// <returns><see cref="ViewResult"/> for "AverageMarks" view.</returns>
        [HttpGet]
        public IActionResult GetAverageMarks(StudentSubjectSelectorViewModel viewModel)
        {
            var studentId = viewModel.SelectedStudentId;
            var subjectId = viewModel.SelectedSubjectId;

            var marks = this.marksAnalyticsService.GetAverageMarks(studentId, subjectId);
            var students = marks.Select(m => m.StudentName).Distinct().Order().ToList();
            var subjects = marks.Select(m => m.SubjectName).Distinct().Order().ToList();

            var aveMarksTable = new AverageMarkDto?[students.Count, subjects.Count];

            for (int i = 0; i < students.Count; i++)
            {
                for (int j = 0; j < subjects.Count; j++)
                {
                    aveMarksTable[i, j] = marks.FirstOrDefault(m => m!.StudentName == students![i] && m!.SubjectName == subjects![j]);
                }
            }

            this.navigationService.RefreshBackParams(this.RouteData);

            return this.View(@"~/Views/Data/Representation/AverageMarks.cshtml", new AverageMarksViewModel
            {
                AverageMarksTable = aveMarksTable,
                Subjects = subjects,
                Students = students,
            });
        }

        private async Task<List<SelectListItem>> GetLessonsList(int? lessonId)
        {
            var lessons = await this.derivedRepositories.LessonRepository.GetAllEntitiesAsync<Lesson>();

            return [.. lessons.Select(lesson => new SelectListItem
            {
                Value = lesson.Id.ToString(),
                Text = $"{lesson.Date} {lesson.ScheduledLesson}",
                Selected = lesson.Id == lessonId,
            })];
        }

        private async Task<List<SelectListItem>> GetStudentsList(int? studentId)
        {
            var students = await this.derivedRepositories.StudentRepository.GetAllEntitiesAsync<Student>();

            return [.. students.Select(student => new SelectListItem
            {
                Value = student.Id.ToString(),
                Text = $"{student.FirstName} {student.LastName}",
                Selected = student.Id == studentId,
            })];
        }

        private async Task<List<SelectListItem>> GetStudents()
        {
            var students = await this.derivedRepositories.StudentRepository.GetAllEntitiesAsync<Student>();

            var studentsItemList = new List<SelectListItem>()
            {
                new()
                {
                    Value = 0.ToString(),
                    Text = string.Empty,
                    Selected = true,
                },
            };

            studentsItemList.AddRange([.. students.Select(student => new SelectListItem
            {
                Value = student.Id.ToString(),
                Text = student.ToString(),
                Selected = false,
            })]);

            return studentsItemList;
        }

        private async Task<List<SelectListItem>> GetSubjects()
        {
            var subjects = await this.subjectRepository.GetAllEntitiesAsync<Subject>();
            var subjectsList = new List<SelectListItem>()
            {
                new() { Value = 0.ToString(), Text = string.Empty, Selected = true },
            };

            subjectsList.AddRange([.. subjects.Select(subject => new SelectListItem
            {
                Value = subject.Id.ToString(),
                Text = subject.Name,
                Selected = false,
            })]);

            return subjectsList;
        }
    }
}
