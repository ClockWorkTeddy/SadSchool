// <copyright file="StudentController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using SadSchool.Contracts;
    using SadSchool.Contracts.Repositories;
    using SadSchool.Models.SqlServer;
    using SadSchool.ViewModels;

    /// <summary>
    /// Processes requests for student data.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="StudentController"/> class.
    /// </remarks>
    /// <param name="derivedRepositories">Students repository instance.</param>
    /// <param name="navigationService">Service processes the "Back" button.</param>
    /// <param name="authService">Service processes user authorization check.</param>"
    /// <param name="commonMapper">Service processes mapping operations.</param>
    public class StudentController(
        IDerivedRepositories derivedRepositories,
        INavigationService navigationService,
        IAuthService authService,
        ICommonMapper commonMapper) : Controller
    {
        private readonly IStudentRepository studentRepository = derivedRepositories.StudentRepository;
        private readonly IClassRepository classRepository = derivedRepositories.ClassRepository;
        private readonly INavigationService navigationService = navigationService;
        private readonly IAuthService authService = authService;
        private readonly ICommonMapper commonMapper = commonMapper;

        /// <summary>
        /// Gets the students view.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for the Students view.</returns>
        [HttpGet]
        public async Task<IActionResult> Students()
        {
            var students = await this.studentRepository.GetAllEntitiesAsync<Student>();
            var studentViewModels = students.Select(s => this.commonMapper.StudentToVm(s)).ToList();

            this.navigationService.RefreshBackParams(this.RouteData);

            return this.View(@"~/Views/Data/Students.cshtml", studentViewModels);
        }

        /// <summary>
        /// Gets the student add form.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for the "StudentAdd" view or
        ///     <see cref="RedirectToActionResult"/> for the "Students" action.</returns>
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            if (this.authService.IsAutorized(this.User))
            {
                var viewModel = new StudentViewModel
                {
                    Classes = await this.GetClassesList(null),
                    Sexes = GetSexes(null),
                };

                this.navigationService.RefreshBackParams(this.RouteData);

                return this.View(@"~/Views/Data/StudentAdd.cshtml", viewModel);
            }

            return this.RedirectToAction("Students");
        }

        /// <summary>
        /// Adds a new student.
        /// </summary>
        /// <param name="viewModel">View model with data.</param>
        /// <returns><see cref="RedirectToActionResult"/> for the "Students" action.</returns>
        [HttpPost]
        public async Task<IActionResult> Add(StudentViewModel viewModel)
        {
            if (this.ModelState.IsValid && viewModel != null)
            {
                var student = this.commonMapper.StudentToModel(viewModel);

                await this.studentRepository.AddEntityAsync(student);
            }

            return this.RedirectToAction("Students");
        }

        /// <summary>
        /// Gets the student edit form.
        /// </summary>
        /// <param name="id">Edited item id.</param>
        /// <returns><see cref="ViewResult"/> for the "StudentEdit" view or
        ///     <see cref="RedirectToActionResult"/> to the "Students" action.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (this.authService.IsAutorized(this.User) && this.ModelState.IsValid)
            {
                var editedStudent = await this.studentRepository.GetEntityByIdAsync<Student>(id);

                if (editedStudent != null)
                {
                    StudentViewModel viewModel = this.commonMapper.StudentToVm(editedStudent);
                    viewModel.Sexes = GetSexes(editedStudent?.Sex);
                    viewModel.Classes = await this.GetClassesList(editedStudent?.ClassId);

                    this.navigationService.RefreshBackParams(this.RouteData);

                    return this.View(@"~/Views/Data/StudentEdit.cshtml", viewModel);
                }
            }

            return this.RedirectToAction("Students");
        }

        /// <summary>
        /// Edits a student.
        /// </summary>
        /// <param name="viewModel">View model with data.</param>
        /// <returns><see cref="RedirectToActionResult"/> for the "Students" action or
        ///     <see cref="ViewResult"/> for the "StudentEdit" view.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(StudentViewModel viewModel)
        {
            if (this.ModelState.IsValid)
            {
                var student = this.commonMapper.StudentToModel(viewModel);

                await this.studentRepository.UpdateEntityAsync(student);

                return this.RedirectToAction("Students");
            }

            return this.View(@"~/Views/Data/StudentEdit.cshtml", viewModel);
        }

        /// <summary>
        /// Deletes a student.
        /// </summary>
        /// <param name="id">Deleted item id.</param>
        /// <returns><see cref="RedirectToActionResult"/> for the "Students" actions.</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (this.authService.IsAutorized(this.User) && this.ModelState.IsValid)
            {
                var student = await this.studentRepository.GetEntityByIdAsync<Student>(id);

                if (student != null)
                {
                    await this.studentRepository.DeleteEntityAsync<Student>(id);
                }
            }

            return this.RedirectToAction("Students");
        }

        private static List<SelectListItem> GetSexes(bool? sex)
        {
            List<SelectListItem> sexes =
            [
                new SelectListItem()
                {
                    Value = "False",
                    Text = "Male",
                    Selected = sex.HasValue && !sex.Value,
                },

                new SelectListItem()
                {
                    Value = "True",
                    Text = "Female",
                    Selected = sex.HasValue && sex.Value,
                },
            ];

            return sexes;
        }

        private async Task<List<SelectListItem>> GetClassesList(int? classId)
        {
            var classes = await this.classRepository.GetAllEntitiesAsync<Class>();

            return [.. classes.Select(@class => new SelectListItem
            {
                Value = @class.Id.ToString(),
                Text = $"{@class.Name}",
                Selected = @class.Id == classId,
            })];
        }
    }
}
