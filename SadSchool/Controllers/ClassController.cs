// <copyright file="ClassController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Models.SqlServer;
    using SadSchool.Controllers.Contracts;
    using SadSchool.DbContexts;
    using SadSchool.ViewModels;

    /// <summary>
    /// Processes class entities.
    /// </summary>
    public class ClassController : Controller
    {
        private readonly SadSchoolContext context;
        private readonly INavigationService navigationService;
        private readonly IAuthService authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassController"/> class.
        /// </summary>
        /// <param name="context">DB context.</param>
        /// <param name="navigationService">The service responses for "Back" button operations.</param>
        /// <param name="authService">The service responses for user authorization.</param>
        public ClassController(SadSchoolContext context, INavigationService navigationService, IAuthService authService)
        {
            this.context = context;
            this.navigationService = navigationService;
            this.authService = authService;
        }

        /// <summary>
        /// Gets classes view.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for a classes view.</returns>
        [HttpGet]
        public IActionResult Classes()
        {
            List<ClassViewModel> classes = new();
            try
            {
                foreach (var theClass in this.context.Classes.Include(c => c.Teacher).ToList())
                {
                    classes.Add(new ClassViewModel
                    {
                        Id = theClass.Id,
                        Name = theClass.Name,
                        TeacherId = theClass.TeacherId,
                        TeacherName = this.GetTeacherName(theClass.TeacherId),
                        LeaderName = this.GetLeaderName(theClass.LeaderId),
                    });
                }
            }
            catch (Exception ex)
            {
                StreamWriter sw = new StreamWriter("log.txt", true);
                sw.WriteLine($"{DateTime.Now}: {ex.Message}");
                sw.WriteLine($"{DateTime.Now}: {ex.InnerException?.Message}");
                sw.Close();
            }

            this.navigationService.RefreshBackParams(this.RouteData);

            return this.View(@"~/Views/Data/Classes.cshtml", classes);
        }

        /// <summary>
        /// Gets add class view.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for class add view.</returns>
        [HttpGet]
        public IActionResult Add()
        {
            if (this.authService.IsAutorized(this.User))
            {
                ClassViewModel viewModel = new() { Teachers = this.GetTeachersList(null) };

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
                var @class = new Class
                {
                    Name = viewModel.Name ?? string.Empty,
                    TeacherId = viewModel.TeacherId,
                };

                this.context.Classes.Add(@class);
                await this.context.SaveChangesAsync();

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
        public IActionResult Edit(int id)
        {
            if (this.authService.IsAutorized(this.User))
            {
                var editedClass = this.context.Classes.Find(id);

                ClassViewModel viewModel = new()
                {
                    Name = editedClass?.Name,
                    Teachers = this.GetTeachersList(editedClass?.TeacherId),
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
                var @class = new Class
                {
                    Id = viewModel.Id,
                    Name = viewModel?.Name,
                    TeacherId = viewModel?.TeacherId,
                };

                this.context.Classes.Update(@class);
                await this.context.SaveChangesAsync();
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
            if (this.authService.IsAutorized(this.User))
            {
                var @class = await this.context.Classes.FindAsync(id);

                if (@class != null)
                {
                    this.context.Classes.Remove(@class);
                    await this.context.SaveChangesAsync();
                }

                return this.RedirectToAction("Classes");
            }

            return this.RedirectToAction("Classes");
        }

        private List<SelectListItem> GetTeachersList(int? teacherId)
        {
            var teachers = this.context.Teachers.ToList();

            return teachers.Select(teacher => new SelectListItem
            {
                Value = teacher.Id.ToString(),
                Text = $"{teacher.FirstName} {teacher.LastName}",
                Selected = teacher.Id == teacherId,
            }).ToList();
        }

        private string GetLeaderName(int? leaderId)
        {
            var leader = this.context.Students.Find(leaderId);

            return $"{leader?.FirstName} {leader?.LastName}";
        }

        private string GetTeacherName(int? teacherId)
        {
            var teacher = this.context.Teachers.Find(teacherId);

            return $"{teacher?.FirstName} {teacher?.LastName}";
        }
    }
}
