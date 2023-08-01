using Microsoft.AspNetCore.Mvc;
using SadSchool.ViewModels;
using SadSchool.Models;

namespace SadSchool.Controllers
{
    public class SubjectController : Controller
    {
        private readonly SadSchoolContext _context;

        public SubjectController(SadSchoolContext context)
        {
            _context = context;
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

            return View(@"~/Views/Data/Subjects.cshtml", subjects);
        }

        [HttpGet]
        public IActionResult AddSubject()
        {
            return View(@"~/Views/Data/SubjectAdd.cshtml");
        }

        [HttpPost]
        public IActionResult AddSubject(SubjectViewModel model)
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

        [HttpPost]
        public IActionResult DeleteSubject(int id)
        {
            var subject = _context.Subjects.Find(id);

            if (subject != null)
            {
                _context.Subjects.Remove(subject);
                _context.SaveChanges();
            }

            return RedirectToAction("Subjects");
        }   
    }
}
