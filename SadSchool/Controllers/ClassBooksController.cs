// <copyright file="ClassBooksController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MongoDB.Driver;
    using SadSchool.Contracts;
    using SadSchool.DbContexts;
    using SadSchool.ViewModels;

    /// <summary>
    /// Manages class books processing.
    /// </summary>
    public class ClassBooksController : Controller
    {
        private readonly SadSchoolContext context;
        private readonly INavigationService navigationService;
        private readonly IClassBookService classBookService;
        private readonly MongoContext mongoContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassBooksController"/> class.
        /// </summary>
        /// <param name="sadSchoolContext">DB Context.</param>
        /// <param name="navigationService">A navigation service instance,
        ///     that responces for the "Back" button operating.</param>
        /// <param name="classBookService">A class books service instance,
        ///     that responses for class book data operations.</param>
        /// <param name="mongoContext">MongoDB context.</param>
        public ClassBooksController(
            SadSchoolContext sadSchoolContext,
            INavigationService navigationService,
            IClassBookService classBookService,
            MongoContext mongoContext)
        {
            this.classBookService = classBookService;
            this.context = sadSchoolContext;
            this.navigationService = navigationService;
            this.mongoContext = mongoContext;
        }

        /// <summary>
        /// Gets class books view.
        /// </summary>
        /// <returns><see cref="ViewResult"/> for a class book view.</returns>
        [HttpGet]
        public IActionResult ClassBooks()
        {
            this.navigationService.RefreshBackParams(this.RouteData);

            var classNames = this.context.Classes.Select(cl => cl.Name).ToList();

            return this.View(@"~/Views/Data/Representation/ClassBooks.cshtml", classNames);
        }

        /// <summary>
        /// Sends to the class selection page.
        /// </summary>
        /// <param name="className">Name of desirable class.</param>
        /// <returns>A <see cref="ViewResult"/> for redirection to subject selection page.</returns>
        [HttpGet]
        public IActionResult ClassSelector(string className)
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

            var subjects = this.context.Subjects.Select(subject => subject.Name).ToList();

            return this.View(
                @"~/Views/Data/Representation/ClassSubjects.cshtml",
                new ClassSubjectViewModel
                {
                    ClassName = className,
                    Subjects = subjects,
                });
        }

        /// <summary>
        /// Gets table with marks for specified <paramref name="subject"/> and <paramref name="className"/>.
        /// </summary>
        /// <param name="subject">Desirable subject name.</param>
        /// <param name="className">Desirable class name.</param>
        /// <returns><see cref="ViewResult"/> for ClassBook page.</returns>
        [HttpGet]
        public IActionResult ClassBookTable(string subject, string className)
        {
            this.navigationService.RefreshBackParams(this.RouteData);

            var viewModel = this.classBookService.GetClassBookViewModel(subject, className);

            return this.View(@"~/Views/Data/Representation/ClassBook.cshtml", viewModel);
        }
    }
}
