// <copyright file="StudentController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using SadSchool.Controllers.Contracts;
    using SadSchool.DbContexts;
    using SadSchool.Models.SqlServer;
    using SadSchool.ViewModels;

    /// <summary>
    /// Processes requests for student data.
    /// </summary>
    public class StudentController : Controller
    {
        private readonly SadSchoolContext context;
        private readonly INavigationService navigationService;
        private readonly ICacheService cacheService;
        private readonly IAuthService authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentController"/> class.
        /// </summary>
        /// <param name="context">DB context instance.</param>
        /// <param name="navigationService">Service processes the "Back" button.</param>
        /// <param name="cacheService">Cache instance.</param>
        /// <param name="authService">Service processes user authorization check.</param>"
        public StudentController(
            SadSchoolContext context,
            INavigationService navigationService,
            ICacheService cacheService,
            IAuthService authService)
        {
            this.context = context;
            this.navigationService = navigationService;
            this.cacheService = cacheService;
            this.authService = authService;
        }

        /// <summary>
        /// Gets the students view.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for the Students view.</returns>
        [HttpGet]
        public IActionResult Students()
        {
            var students = new List<StudentViewModel>();

            foreach (var student in this.context.Students.ToList())
            {
                students.Add(new StudentViewModel
                {
                    Id = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    DateOfBirth = student.DateOfBirth.ToString(),
                    Sex = student.Sex,
                    ClassName = student.Class?.Name,
                });
            }

            this.navigationService.RefreshBackParams(this.RouteData);

            return this.View(@"~/Views/Data/Students.cshtml", students);
        }

        /// <summary>
        /// Gets the student add form.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for the "StudentAdd" view or
        ///     <see cref="RedirectToActionResult"/> for the "Students" action.</returns>
        [HttpGet]
        public IActionResult Add()
        {
            if (this.authService.IsAutorized(this.User))
            {
                StudentViewModel viewModel = new StudentViewModel()
                {
                    Classes = this.GetClassesList(null),
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
                var dateData = new List<int>();

                dateData = viewModel?.DateOfBirth?
                    .Split('-')
                    .Select(d => Convert.ToInt32(d))
                    .ToList();

                if (dateData != null && dateData.Count == 3)
                {
                    var student = new Student
                    {
                        FirstName = viewModel?.FirstName,
                        LastName = viewModel?.LastName,
                        ClassId = viewModel?.ClassId,
                        Class = this.context.Classes.Find(viewModel?.ClassId),
                        DateOfBirth = new DateOnly(dateData[0], dateData[1], dateData[2]),
                        Sex = viewModel?.Sex,
                    };

                    this.context.Students.Add(student);
                    await this.context.SaveChangesAsync();
                }
            }

            return this.RedirectToAction("Add");
        }

        /// <summary>
        /// Gets the student edit form.
        /// </summary>
        /// <param name="id">Edited item id.</param>
        /// <returns><see cref="ViewResult"/> for the "StudentEdit" view or
        ///     <see cref="RedirectToActionResult"/> to the "Students" action.</returns>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (this.authService.IsAutorized(this.User))
            {
                var editedStudent = this.context.Students.Find(id);

                StudentViewModel viewModel = new()
                {
                    FirstName = editedStudent?.FirstName,
                    LastName = editedStudent?.LastName,
                    DateOfBirth = editedStudent?.DateOfBirth.ToString(),
                    Sexes = this.GetSexes(editedStudent?.Sex),
                    Classes = this.GetClassesList(editedStudent?.ClassId),
                };

                this.navigationService.RefreshBackParams(this.RouteData);

                return this.View(@"~/Views/Data/StudentEdit.cshtml", viewModel);
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
            if (this.ModelState.IsValid && viewModel != null)
            {
                var dateData = viewModel?.DateOfBirth?
                    .Split('-')
                    .Select(d => Convert.ToInt32(d))
                    .ToList();

                if (dateData != null && dateData.Count == 3)
                {
                    var student = new Student
                    {
                        Id = viewModel?.Id,
                        FirstName = viewModel?.FirstName,
                        LastName = viewModel?.LastName,
                        DateOfBirth = new DateOnly(dateData[0], dateData[1], dateData[2]),
                        Sex = viewModel?.Sex,
                        ClassId = viewModel?.ClassId,
                    };

                    this.context.Students.Update(student);
                    await this.context.SaveChangesAsync();
                    this.cacheService.RefreshObject(student);
                }

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
                var student = await this.context.Students.FindAsync(id);

                if (student != null)
                {
                    this.context.Students.Remove(student);
                    await this.context.SaveChangesAsync();
                }
            }

            return this.RedirectToAction("Students");
        }

        private List<SelectListItem> GetClassesList(int? classId)
        {
            var classes = this.context.Classes.ToList();

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
