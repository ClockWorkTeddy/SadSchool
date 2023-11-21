using Microsoft.AspNetCore.Mvc;
using SadSchool.Models;
using SadSchool.Services;
using SadSchool.Controllers.Contracts;
using SadSchool.ViewModels;

namespace SadSchool.Controllers
{
    public class ClassBooksController : Controller
    {
        private readonly SadSchoolContext _context;
        private readonly INavigationService _navigationService;
        private readonly IClassBookService _classBookService;

        public ClassBooksController(SadSchoolContext sadSchoolContext, INavigationService navigationService, IClassBookService classBookService)
        {
            _classBookService = classBookService;
            _context = sadSchoolContext;
            _navigationService = navigationService;
        }

        [HttpGet]
        public IActionResult ClassBooks()
        {
            _navigationService.RefreshBackParams(RouteData);

            var class_names = _context.Classes.Select(cl => cl.Name).ToList();

            return View(@"~/Views/Data/Representation/ClassBooks.cshtml", class_names);
        }

        [HttpGet]
        public IActionResult ClassBook(string className)
        {
            _navigationService.RefreshBackParams(RouteData);

            var subjects = _context.Subjects.Select(subject => subject.Name).ToList();

            return View(@"~/Views/Data/Representation/ClassSubjects.cshtml", 
                new ClassSubjectViewModel
                {
                    ClassName = className,
                    Subjects = subjects
                });
        }

        [HttpGet]
        public IActionResult ClassBookTable(string subject, string className)
        {
            _navigationService.RefreshBackParams(RouteData);

            var viewModel = _classBookService.GetClassBookViewModel(subject, className);

            return View(@"~/Views/Data/Representation/ClassBook.cshtml", viewModel);
        }
    }
}
