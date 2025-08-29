// <copyright file="ClassController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using SadSchool.Contracts;
    using SadSchool.Contracts.Repositories;
    using SadSchool.Models.SqlServer;
    using SadSchool.ViewModels;

    /// <summary>
    /// Processes class entities.
    /// </summary>
    public class ClassController : Controller
    {
        private readonly IClassRepository classRepository;
        private readonly ITeacherRepository teacherRepository;
        private readonly INavigationService navigationService;
        private readonly IAuthService authService;
        private readonly ICommonMapper commonMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassController"/> class.
        /// </summary>
        /// <param name="classRepository">Class repo instance.</param>
        /// <param name="teacherRepository">Teacher repo instance.</param>
        /// <param name="navigationService">The service responses for "Back" button operations.</param>
        /// <param name="authService">The service responses for user authorization.</param>
        /// <param name="commonMapper">The service responses for mapping operations.</param>
        public ClassController(
            IClassRepository classRepository,
            ITeacherRepository teacherRepository,
            INavigationService navigationService,
            IAuthService authService,
            ICommonMapper commonMapper)
        {
            this.classRepository = classRepository;
            this.teacherRepository = teacherRepository;
            this.navigationService = navigationService;
            this.authService = authService;
            this.commonMapper = commonMapper;
        }

        /// <summary>
        /// Gets classes view.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for a classes view.</returns>
        [HttpGet]
        public async Task<IActionResult> Classes()
        {
            var classes = await this.classRepository.GetAllEntitiesAsync<Class>();

            var classViewModels = classes.Select(c => new ClassViewModel
            {
                Id = c.Id,
                Name = c.Name,
                TeacherId = c.TeacherId,
                TeacherName = $"{c?.Teacher?.FirstName} {c?.Teacher?.LastName}",
            }).ToList();

            this.navigationService.RefreshBackParams(this.RouteData);

            return this.View(@"~/Views/Data/Classes.cshtml", classViewModels);
        }

        /// <summary>
        /// Gets add class view.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for class add view.</returns>
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            if (this.authService.IsAutorized(this.User))
            {
                ClassViewModel viewModel = new() { Teachers = await this.GetTeachersList(null) };

                this.navigationService.RefreshBackParams(this.RouteData);

                return this.View(@"~/Views/Data/ClassAdd.cshtml", viewModel);
            }

            return this.RedirectToAction("Classes");
        }

        /// <summary>
        /// Adds <see cref="Class"/> instance to DB.
        /// </summary>
        /// <param name="viewModel"><paramref name="viewModel"/> with data for class instance.</param>
        /// <returns><see cref="ViewResult"/> of redirect to "Classes" action.</returns>
        [HttpPost]
        public async Task<IActionResult> Add(ClassViewModel viewModel)
        {
            if (this.ModelState.IsValid)
            {
                var @class = this.commonMapper.ClassToModel(viewModel);

                await this.classRepository.AddEntityAsync(@class);

                return this.RedirectToAction("Classes");
            }
            else
            {
                return this.View(@"~/Views/Data/ClassAdd.cshtml", viewModel);
            }
        }

        /// <summary>
        /// Gets page for edit <see cref="Class"/> instance.
        /// </summary>
        /// <param name="id">Desirable <see cref="Class"/> id.</param>
        /// <returns><see cref="ViewResult"/> of redirect to "Classes" action.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (this.authService.IsAutorized(this.User) && this.ModelState.IsValid)
            {
                var editedClass = await this.classRepository.GetEntityByIdAsync<Class>(id);

                ClassViewModel viewModel = new()
                {
                    Name = editedClass?.Name,
                    Teachers = await this.GetTeachersList(editedClass?.TeacherId),
                };

                this.navigationService.RefreshBackParams(this.RouteData);

                return this.View(@"~/Views/Data/ClassEdit.cshtml", viewModel);
            }

            return this.RedirectToAction("Classes");
        }

        /// <summary>
        /// Edits selected <see cref="Class"/> instance.
        /// </summary>
        /// <param name="viewModel">Edited data for selected <see cref="Class"/> instance.</param>
        /// <returns>Redirect to "Classes" action or <see cref="ViewResult"/>.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(ClassViewModel viewModel)
        {
            if (this.ModelState.IsValid && viewModel != null)
            {
                var @class = this.commonMapper.ClassToModel(viewModel);

                await this.classRepository.UpdateEntityAsync(@class);

                return this.RedirectToAction("Classes");
            }
            else
            {
                return this.View(@"~/Views/Data/ClassEdit.cshtml", viewModel);
            }
        }

        /// <summary>
        /// Removes selected <see cref="Class"/> instance.
        /// </summary>
        /// <param name="id">Desirable <see cref="Class"/> id.</param>
        /// <returns><see cref="RedirectToActionResult"/> to action "Classes".</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (this.authService.IsAutorized(this.User) && this.ModelState.IsValid)
            {
                await this.classRepository.DeleteEntityAsync<Class>(id);

                return this.RedirectToAction("Classes");
            }

            return this.RedirectToAction("Classes");
        }

        private async Task<List<SelectListItem>> GetTeachersList(int? teacherId)
        {
            var teachers = await this.teacherRepository.GetAllEntitiesAsync<Teacher>();

            return teachers.Select(teacher => new SelectListItem
            {
                Value = teacher.Id.ToString(),
                Text = $"{teacher.FirstName} {teacher.LastName}",
                Selected = teacher.Id == teacherId,
            }).ToList();
        }
    }
}
