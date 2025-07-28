// <copyright file="StudentController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using SadSchool.Contracts;
    using SadSchool.Contracts.Repositories;
    using SadSchool.DbContexts;
    using SadSchool.Models.SqlServer;
    using SadSchool.ViewModels;
    using System.Threading.Tasks;

    /// <summary>
    /// Processes requests for student data.
    /// </summary>
    public class StudentController : Controller
    {
        private readonly IStudentRepository studentRepository;
        private readonly IClassRepository classRepository;
        private readonly INavigationService navigationService;
        private readonly ICacheService cacheService;
        private readonly IAuthService authService;
        private readonly ICommonMapper commonMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentController"/> class.
        /// </summary>
        /// <param name="derivedRepositories">Students repository instance.</param>
        /// <param name="navigationService">Service processes the "Back" button.</param>
        /// <param name="cacheService">Cache instance.</param>
        /// <param name="authService">Service processes user authorization check.</param>"
        /// <param name="commonMapper">Service processes mapping operations.</param>
        public StudentController(
            IDerivedRepositories derivedRepositories,
            INavigationService navigationService,
            ICacheService cacheService,
            IAuthService authService,
            ICommonMapper commonMapper)
        {
            this.studentRepository = derivedRepositories.StudentRepository;
            this.classRepository = derivedRepositories.ClassRepository;
            this.navigationService = navigationService;
            this.cacheService = cacheService;
            this.authService = authService;
            this.commonMapper = commonMapper;
        }

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
                    Sexes = this.GetSexes(null),
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
            if (this.ModelState.IsValid)
            {
                if (viewModel != null)
                {
                    var student = this.commonMapper.StudentToModel(viewModel);

                    await this.studentRepository.AddEntityAsync(student);

                    this.cacheService.GetObject<Student>(student.Id!.Value);
                }
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
            if (this.authService.IsAutorized(this.User))
            {
                var editedStudent = await this.studentRepository.GetEntityByIdAsync<Student>(id);

                if (editedStudent != null)
                {
                    StudentViewModel viewModel = this.commonMapper.StudentToVm(editedStudent);
                    viewModel.Sexes = this.GetSexes(editedStudent?.Sex);
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

                this.cacheService.SetObject(student);

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
            if (this.authService.IsAutorized(this.User))
            {
                var student = await this.studentRepository.GetEntityByIdAsync<Student>(id);

                if (student != null)
                {
                    await this.studentRepository.DeleteEntityAsync<Student>(id);
                    this.cacheService.RemoveObject(student);
                }
            }

            return this.RedirectToAction("Students");
        }

        private async Task<List<SelectListItem>> GetClassesList(int? classId)
        {
            var classes = await this.classRepository.GetAllEntitiesAsync<Class>();

            return classes.Select(@class => new SelectListItem
            {
                Value = @class.Id.ToString(),
                Text = $"{@class.Name}",
                Selected = @class.Id == classId,
            }).ToList();
        }

        private List<SelectListItem> GetSexes(bool? sex)
        {
            List<SelectListItem> sexes = new()
            {
                new SelectListItem()
                {
                    Value = "False",
                    Text = "Male",
                    Selected = sex.HasValue ? !sex.Value : false,
                },

                new SelectListItem()
                {
                    Value = "True",
                    Text = "Female",
                    Selected = sex.HasValue ? sex.Value : false,
                },
            };

            return sexes;
        }
    }
}
