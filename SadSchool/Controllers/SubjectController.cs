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

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var editedSubject = _context.Subjects.Find(id);

            var model = new SubjectViewModel
            {
                Name = editedSubject?.Name
            };

            return View(@"~/Views/Data/SubjectEdit.cshtml", model);
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
