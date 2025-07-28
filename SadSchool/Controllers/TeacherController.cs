// <copyright file="TeacherController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SadSchool.Contracts;
    using SadSchool.Contracts.Repositories;
    using SadSchool.DbContexts;
    using SadSchool.Models.SqlServer;
    using SadSchool.ViewModels;

    /// <summary>
    /// Processes requests for teacher data.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="TeacherController"/> class.
    /// </remarks>
    /// <param name="teacherRepository">DB context.</param>
    /// <param name="navigationService">Services processes "Back" button.</param>
    /// <param name="authService">Service processes user authorization check.</param>
    /// <param name="cacheService">Service processes cache operations.</param>
    /// <param name="commonMapper">Service processes mapping operations.</param>
    public class TeacherController(
        ITeacherRepository teacherRepository,
        INavigationService navigationService,
        IAuthService authService,
        ICommonMapper commonMapper,
        ICacheService cacheService) : Controller
    {
        private readonly ITeacherRepository teacherRepository = teacherRepository;
        private readonly INavigationService navigationService = navigationService;
        private readonly IAuthService authService = authService;
        private readonly ICommonMapper commonMapper = commonMapper;
        private readonly ICacheService cacheService = cacheService;

        /// <summary>
        /// Gets the teachers view.
        /// </summary>
        /// <returns><see cref="IActionResult"/> for the "Teachers" view.</returns>
        [HttpGet]
        public async Task<IActionResult> Teachers()
        {
            var teachers = await this.teacherRepository.GetAllEntitiesAsync<Teacher>();

            List<TeacherViewModel> teacherViewModels =
                [.. teachers.Select(t => this.commonMapper.TeacherToVm(t))];

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
        public async Task<IActionResult> Add(TeacherViewModel viewModel)
        {
            if (this.ModelState.IsValid)
            {
                var teacher = this.commonMapper.TeacherToModel(viewModel);

                await this.teacherRepository.AddEntityAsync(teacher);

                this.cacheService.GetObject<Teacher>(teacher.Id!.Value);
            }

            return this.RedirectToAction("Teachers");
        }

        /// <summary>
        /// Gets the form for editing a teacher.
        /// </summary>
        /// <param name="id">Edited item id.</param>
        /// <returns><see cref="IActionResult"/> for the "TeacherEdit" view.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (this.authService.IsAutorized(this.User))
            {
                var editedTeacher = await this.teacherRepository.GetEntityByIdAsync<Teacher>(id);

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

                await this.teacherRepository.UpdateEntityAsync(teacher);

                this.cacheService.SetObject(teacher);

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
        public async Task<IActionResult> Delete(int id)
        {
            if (this.authService.IsAutorized(this.User))
            {
                var teacher = await this.teacherRepository.GetEntityByIdAsync<Teacher>(id);

                if (teacher != null)
                {
                    await this.teacherRepository.DeleteEntityAsync<Teacher>(id);
                    this.cacheService.RemoveObject(teacher);
                }
            }

            return this.RedirectToAction("Teachers");
        }
    }
}
