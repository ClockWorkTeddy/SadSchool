// <copyright file="TeacherController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SadSchool.Contracts;
    using SadSchool.DbContexts;
    using SadSchool.Mappers;
    using SadSchool.Models.SqlServer;
    using SadSchool.ViewModels;
    using System.Diagnostics;

    /// <summary>
    /// Processes requests for teacher data.
    /// </summary>
    public class TeacherController : Controller
    {
        private readonly SadSchoolContext context;
        private readonly INavigationService navigationService;
        private readonly IAuthService authService;
        private readonly ICommonMapper commonMapper;
        private readonly ICacheService cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeacherController"/> class.
        /// </summary>
        /// <param name="context">DB context.</param>
        /// <param name="navigationService">Services processes "Back" button.</param>
        /// <param name="authService">Service processes user authorization check.</param>
        public TeacherController(
            SadSchoolContext context,
            INavigationService navigationService,
            IAuthService authService,
            ICommonMapper commonMapper,
            ICacheService cacheService)
        {
            this.context = context;
            this.navigationService = navigationService;
            this.authService = authService;
            this.commonMapper = commonMapper;
            this.cacheService = cacheService;
        }

        /// <summary>
        /// Gets the teachers view.
        /// </summary>
        /// <returns><see cref="IActionResult"/> for the "Teachers" view.</returns>
        [HttpGet]
        public IActionResult Teachers()
        {
            List<TeacherViewModel> teacherViewModels =
                [.. this.context.Teachers.Select(t => this.commonMapper.TeacherToVm(t))];

            this.navigationService.RefreshBackParams(this.RouteData);

            return this.View(@"~/Views/Data/Teachers.cshtml", teacherViewModels);
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
        /// <param name="viewModel">ViewModel with data.</param>
        /// <returns><see cref="IActionResult"/> reditrect to the "Teachers" action.</returns>
        [HttpPost]
        public IActionResult Add(TeacherViewModel viewModel)
        {
            if (this.ModelState.IsValid)
            {
                var teacher = this.commonMapper.TeacherToModel(viewModel);

                this.context.Teachers.Add(teacher);
                this.context.SaveChanges();

                this.cacheService.RefreshObject<Teacher>(teacher);
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
                    this.navigationService.RefreshBackParams(this.RouteData);

                    var viewModel = this.commonMapper.TeacherToVm(editedTeacher);

                    return this.View(@"~/Views/Data/TeacherEdit.cshtml", viewModel);
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
            if (this.ModelState.IsValid)
            {
                var teacher = this.commonMapper.TeacherToModel(viewModel);

                this.context.Teachers.Update(teacher);
                await this.context.SaveChangesAsync();

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
