// <copyright file="SubjectController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SadSchool.Contracts;
    using SadSchool.DbContexts;
    using SadSchool.Models.SqlServer;
    using SadSchool.ViewModels;

    /// <summary>
    /// Processes requests for subject data.
    /// </summary>
    public class SubjectController : Controller
    {
        private readonly SadSchoolContext context;
        private readonly INavigationService navigationService;
        private readonly ICacheService cacheService;
        private readonly IAuthService authService;
        private readonly ICommonMapper commonMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubjectController"/> class.
        /// </summary>
        /// <param name="context">DB context instance.</param>
        /// <param name="navigationService">Service processes the "Back" button.</param>
        /// <param name="cacheService">Cahce instance.</param>
        /// <param name="authService">Service processes user authorization check.</param>
        public SubjectController(
            SadSchoolContext context,
            INavigationService navigationService,
            ICacheService cacheService,
            IAuthService authService,
            ICommonMapper commonMapper)
        {
            this.context = context;
            this.navigationService = navigationService;
            this.cacheService = cacheService;
            this.authService = authService;
            this.commonMapper = commonMapper;
        }

        /// <summary>
        /// Gets the subjects view.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for the "Subjects" view.</returns>
        [HttpGet]
        public IActionResult Subjects()
        {
            this.navigationService.RefreshBackParams(this.RouteData);

            List<SubjectViewModel> subjects = [.. this.context.Subjects.Select(s => this.commonMapper.SubjectToVm(s))];

            return this.View(@"~/Views/Data/Subjects.cshtml", subjects);
        }

        /// <summary>
        /// Gets the form for adding a new subject.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for the "SubjectAdd" view or
        ///     <see cref="RedirectToActionResult"/> for the "Subjects" action.</returns>
        [HttpGet]
        public IActionResult Add()
        {
            if (this.authService.IsAutorized(this.User))
            {
                this.navigationService.RefreshBackParams(this.RouteData);

                return this.View(@"~/Views/Data/SubjectAdd.cshtml");
            }

            return this.RedirectToAction("Subjects");
        }

        /// <summary>
        /// Adds a new subject.
        /// </summary>
        /// <param name="viewModel">View model with data.</param>
        /// <returns><see cref="RedirectToActionResult"/> for the "Subjects".</returns>
        [HttpPost]
        public IActionResult Add(SubjectViewModel viewModel)
        {
            if (this.ModelState.IsValid)
            {
                var subject = this.commonMapper.SubjectToModel(viewModel);

                this.context.Subjects.Add(subject);
                this.context.SaveChanges();

                this.cacheService.GetObject<Subject>(subject.Id!.Value);
            }

            return this.RedirectToAction("Subjects");
        }

        /// <summary>
        /// Gets the form for editing a subject.
        /// </summary>
        /// <param name="id">Edited item id.</param>
        /// <returns><see cref="IActionResult"/> for the "SubjectEdit" view or the "Subjects" action.</returns>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (this.authService.IsAutorized(this.User))
            {
                var editedSubject = this.context.Subjects.Find(id);

                if (editedSubject != null)
                {
                    this.navigationService.RefreshBackParams(this.RouteData);

                    var model = this.commonMapper.SubjectToVm(editedSubject);

                    return this.View(@"~/Views/Data/SubjectEdit.cshtml", model);
                }
            }

            return this.RedirectToAction("Subjects");
        }

        /// <summary>
        /// Edits a subject.
        /// </summary>
        /// <param name="viewModel">View model with data.</param>
        /// <returns><see cref="IActionResult"/> for the "Subjects" action or for the "SubjectEdit" view.</returns>
        [HttpPost]
        public IActionResult Edit(SubjectViewModel viewModel)
        {
            if (this.authService.IsAutorized(this.User))
            {
                if (this.ModelState.IsValid && viewModel != null)
                {
                    var subject = this.commonMapper.SubjectToModel(viewModel);

                    this.context.Subjects.Update(subject);
                    this.context.SaveChanges();

                    this.cacheService.RefreshObject(subject);

                    return this.RedirectToAction("Subjects");
                }
            }

            return this.View(@"~/Views/Data/SubjectEdit.cshtml", viewModel);
        }

        /// <summary>
        /// Deletes a subject.
        /// </summary>
        /// <param name="id">Deleted item id.</param>
        /// <returns><see cref="IActionResult"/> for the "Subjects" action.</returns>
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (this.authService.IsAutorized(this.User))
            {
                var subject = this.context.Subjects.Find(id);

                if (subject != null)
                {
                    this.context.Subjects.Remove(subject);
                    this.context.SaveChanges();

                    this.cacheService.RemoveObject(subject);
                }
            }

            return this.RedirectToAction("Subjects");
        }
    }
}
