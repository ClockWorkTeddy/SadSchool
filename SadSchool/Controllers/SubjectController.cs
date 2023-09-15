using Microsoft.AspNetCore.Mvc;
using SadSchool.ViewModels;
using SadSchool.Models;
using SadSchool.Services;

namespace SadSchool.Controllers
{
    public class SubjectController : Controller
    {
        private readonly SadSchoolContext _context;
        private readonly INavigationService _navigationService;

        public SubjectController(SadSchoolContext context, INavigationService navigationService)
        {
            _context = context;
            _navigationService = navigationService;
        }

        [HttpGet]
        public IActionResult Subjects()
        {
            List<SubjectViewModel> subjects = new List<SubjectViewModel>();

            foreach (var s in _context.Subjects)
            {
                subjects.Add(new SubjectViewModel
                {
                    Id = s.Id,
                    Name = s.Name
                });
            }

            _navigationService.RefreshBackParams(RouteData);

            return View(@"~/Views/Data/Subjects.cshtml", subjects);
        }

        [HttpGet]
        public IActionResult Add()
        {
            if (User.Identity.IsAuthenticated && !User.IsInRole("user"))
            {
                _navigationService.RefreshBackParams(RouteData);

                return View(@"~/Views/Data/SubjectAdd.cshtml");
            }
                
            return RedirectToAction("Subjects");
        }

        [HttpPost]
        public IActionResult Add(SubjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var subject = new Subject
                {
                    Name = model.Name
                };

                _context.Subjects.Add(subject);
                _context.SaveChanges();
            }

            return RedirectToAction("Subjects");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (User.Identity.IsAuthenticated && !User.IsInRole("user"))
            {
                var editedSubject = _context.Subjects.Find(id);

                var model = new SubjectViewModel
                {
                    Name = editedSubject?.Name
                };

                _navigationService.RefreshBackParams(RouteData);

                return View(@"~/Views/Data/SubjectEdit.cshtml", model);
            }
                
            return RedirectToAction("Subjects");
        }

        [HttpPost]
        public IActionResult Edit(SubjectViewModel model)
        {
            if (ModelState.IsValid && model != null)
            {
                var subject = new Subject
                {
                    Id = model.Id,
                    Name = model.Name
                };

                _context.Subjects.Update(subject);
                _context.SaveChanges();
                return RedirectToAction("Subjects");
            }
            else
            {
                return View(@"~/Views/Data/SubjectEdit.cshtml", model);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (User.Identity.IsAuthenticated && !User.IsInRole("user"))
            {
                var subject = _context.Subjects.Find(id);

                if (subject != null)
                {
                    _context.Subjects.Remove(subject);
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Subjects");
        }   
    }
}
