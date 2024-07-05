// <copyright file="TeacherController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SadSchool.Controllers.Contracts;
    using SadSchool.DbContexts;
    using SadSchool.Models.SqlServer;
    using SadSchool.ViewModels;

    /// <summary>
    /// Processes requests for teacher data.
    /// </summary>
    public class TeacherController : Controller
    {
        private readonly SadSchoolContext context;
        private readonly INavigationService navigationService;
        private readonly IAuthService authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeacherController"/> class.
        /// </summary>
        /// <param name="context">DB context.</param>
        /// <param name="navigationService">Services processes "Back" button.</param>
        /// <param name="authService">Service processes user authorization check.</param>
        public TeacherController(
            SadSchoolContext context,
            INavigationService navigationService,
            IAuthService authService)
        {
            this.context = context;
            this.navigationService = navigationService;
            this.authService = authService;
        }

        /// <summary>
        /// Gets the teachers view.
        /// </summary>
        /// <returns><see cref="IActionResult"/> for the "Teachers" view.</returns>
        [HttpGet]
        public IActionResult Teachers()
        {
            List<TeacherViewModel> teachers = new();

            foreach (var t in this.context.Teachers)
            {
                teachers.Add(new TeacherViewModel
                {
                    Id = t.Id,
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    DateOfBirth = t.DateOfBirth?.ToString(),
                    Grade = t.Grade,
                });
            }

            this.navigationService.RefreshBackParams(this.RouteData);

            return this.View(@"~/Views/Data/Teachers.cshtml", teachers);
        }

        /// <summary>
        /// Gets the form for adding a new teacher.
        /// </summary>
        /// <returns><see cref="IActionResult"/> for the "TeacherAdd" view or
        ///     for the "Teachers" redirect to action.</returns>
        [HttpGet]
        public IActionResult Add()
        {
            if (this.authService.IsAutorized(this.User))
            {
                this.navigationService.RefreshBackParams(this.RouteData);

                return this.View(@"~/Views/Data/TeacherAdd.cshtml");
            }

            return this.RedirectToAction("Teachers");
        }

        /// <summary>
        /// Receives the data from the form and adds a new teacher to the database.
        /// </summary>
        /// <param name="model">ViewModel with data.</param>
        /// <returns><see cref="IActionResult"/> reditrect to the "Teachers" action.</returns>
        [HttpPost]
        public IActionResult Add(TeacherViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var dateData = model?.DateOfBirth?
                    .Split('-')
                    .Select(d => Convert.ToInt32(d))
                    .ToList();

                if (dateData != null && dateData.Count == 3)
                {
                    var teacher = new Teacher
                    {
                        FirstName = model?.FirstName,
                        LastName = model?.LastName,
                        DateOfBirth = new DateOnly(dateData[0], dateData[1], dateData[2]),
                        Grade = model?.Grade,
                    };

                    this.context.Teachers.Add(teacher);
                    this.context.SaveChanges();
                }
            }

            return this.RedirectToAction("Teachers");
        }

        /// <summary>
        /// Gets the form for editing a teacher.
        /// </summary>
        /// <param name="id">Edited item id.</param>
        /// <returns><see cref="IActionResult"/> for the "TeacherEdit" view.</returns>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (this.authService.IsAutorized(this.User))
            {
                var editedTeacher = this.context.Teachers.FirstOrDefault(_ => _.Id == id);

                if (editedTeacher != null)
                {
                    var model = new TeacherViewModel
                    {
                        FirstName = editedTeacher.FirstName,
                        LastName = editedTeacher.LastName,
                        DateOfBirth = editedTeacher.DateOfBirth?.ToString(),
                        Grade = editedTeacher.Grade,
                    };

                    this.navigationService.RefreshBackParams(this.RouteData);

                    return this.View(@"~/Views/Data/TeacherEdit.cshtml", model);
                }
            }

            return this.RedirectToAction("Teachers");
        }

        /// <summary>
        /// Edits a teacher.
        /// </summary>
        /// <param name="viewModel">View model object with data.</param>
        /// <returns><see cref="IActionResult"/> for the "Teachers" redirect to action in case of success
        ///     or for the "TeacherEdit" view in case of failure.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(TeacherViewModel viewModel)
        {
            if (this.ModelState.IsValid && viewModel != null)
            {
                var dateData = viewModel?.DateOfBirth?
                    .Split('-')
                    .Select(d => Convert.ToInt32(d))
                    .ToList();

                if (dateData != null && dateData.Count == 3)
                {
                    var teacher = new Teacher
                    {
                        Id = viewModel?.Id,
                        FirstName = viewModel?.FirstName,
                        LastName = viewModel?.LastName,
                        DateOfBirth = new DateOnly(dateData[0], dateData[1], dateData[2]),
                        Grade = viewModel?.Grade,
                    };

                    this.context.Teachers.Update(teacher);
                    await this.context.SaveChangesAsync();
                }

                return this.RedirectToAction("Teachers");
            }
            else
            {
                return this.View(@"~/Views/Data/TeacherEdit.cshtml", viewModel);
            }
        }

        /// <summary>
        /// Deletes a teacher.
        /// </summary>
        /// <param name="id">Deleted item id.</param>
        /// <returns><see cref="IActionResult"/> for the "Teachers" action.</returns>
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (this.authService.IsAutorized(this.User))
            {
                var teacher = this.context.Teachers.FirstOrDefault(_ => _.Id == id);

                if (teacher != null)
                {
                    this.context.Teachers.Remove(teacher);
                    this.context.SaveChanges();
                }
            }

            return this.RedirectToAction("Teachers");
        }
    }
}
