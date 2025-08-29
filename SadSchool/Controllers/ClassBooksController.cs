// <copyright file="ClassBooksController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MongoDB.Driver;
    using SadSchool.Contracts;
    using SadSchool.Contracts.Repositories;
    using SadSchool.Models.SqlServer;
    using SadSchool.ViewModels;

    /// <summary>
    /// Manages class books processing.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ClassBooksController"/> class.
    /// </remarks>
    /// <param name="classRepository">Class repository instance,
    ///     that responses for class data operations.</param>
    /// <param name="subjectRepository">Subject repository instance,
    ///     that responses for subject data operations.</param>
    /// <param name="navigationService">A navigation service instance,
    ///     that responces for the "Back" button operating.</param>
    /// <param name="classBookService">A class books service instance,
    ///     that responses for class book data operations.</param>
    public class ClassBooksController(
        IClassRepository classRepository,
        ISubjectRepository subjectRepository,
        INavigationService navigationService,
        IClassBookService classBookService) : Controller
    {
        private readonly IClassRepository classRepository = classRepository;
        private readonly ISubjectRepository subjectRepository = subjectRepository;
        private readonly INavigationService navigationService = navigationService;
        private readonly IClassBookService classBookService = classBookService;

        /// <summary>
        /// Gets class books view.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for a class book view.</returns>
        [HttpGet]
        public async Task<IActionResult> ClassBooks()
        {
            this.navigationService.RefreshBackParams(this.RouteData);

            var classes = await this.classRepository.GetAllEntitiesAsync<Class>();
            var classNames = classes.Select(c => c.Name).ToList();

            return this.View(@"~/Views/Data/Representation/ClassBooks.cshtml", classNames);
        }

        /// <summary>
        /// Sends to the class selection page.
        /// </summary>
        /// <param name="className">Name of desirable class.</param>
        /// <returns>A <see cref="ViewResult"/> for redirection to subject selection page.</returns>
        [HttpGet]
        public async Task<IActionResult> ClassSelector(string className)
        {
            if (className != null)
            {
                this.navigationService.StoreClassName(className);
            }
            else
            {
                className = this.navigationService.ClassName;
            }

            this.navigationService.RefreshBackParams(this.RouteData);

            var subjects = await this.subjectRepository.GetAllEntitiesAsync<Subject>();
            var subjectNames = subjects.Select(s => s.Name).ToList();

            return this.View(
                @"~/Views/Data/Representation/ClassSubjects.cshtml",
                new ClassSubjectViewModel
                {
                    ClassName = className,
                    Subjects = subjectNames,
                });
        }

        /// <summary>
        /// Gets table with marks for specified <paramref name="subject"/> and <paramref name="className"/>.
        /// </summary>
        /// <param name="subject">Desirable subject name.</param>
        /// <param name="className">Desirable class name.</param>
        /// <returns><see cref="ViewResult"/> for ClassBook page.</returns>
        [HttpGet]
        public async Task<IActionResult> ClassBookTable(string subject, string className)
        {
            this.navigationService.RefreshBackParams(this.RouteData);

            var viewModel = await this.classBookService.GetClassBookViewModel(subject, className);

            return this.View(@"~/Views/Data/Representation/ClassBook.cshtml", viewModel);
        }
    }
}
