using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SadSchool.Models;
using SadSchool.ViewModels;

namespace SadSchool.Controllers
{
    public class MarkController : Controller
    {
        private readonly SadSchoolContext _context;

        public MarkController(SadSchoolContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Marks()
        {
            var marks = new List<MarkViewModel>();

            foreach (var mark in _context.Marks
                .Include(m => m.Student)
                .Include(m => m.Lesson)
                .Include(m => m.Lesson.Subject)
                .Include(m => m.Lesson.ScheduledPosition))
            { 
                marks.Add(new MarkViewModel
                {
                    Id = mark.Id,
                    Value = mark.Value,
                    Student = $"{mark.Student.Name} {mark.Student.LastName}",
                    Lesson = $"{mark.Lesson.Subject.Name} {mark.Lesson.Date} {mark.Lesson.ScheduledPosition.StartTime}"
                });
            }

            return View(@"~/Views/Data/Marks.cshtml", marks);
        }

        [HttpGet]
        public IActionResult AddMark()
        {
            MarkAddViewModel viewModel = new MarkAddViewModel()
            {
                StudentsForView = _context.Students.ToList(),
                LessonsForView = _context.Lessons.Include(l => l.Subject).Include(l => l.ScheduledPosition).ToList()
            };

            return View(@"~/Views/Data/MarkAdd.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddMark(MarkAddViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var mark = new Mark
                {
                    Value = viewModel.Value,
                    StudentId = viewModel.StudentId,
                    LessonId = viewModel.LessonId
                };

                _context.Marks.AddAsync(mark);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Marks");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMark(int id)
        {
            var mark = await _context.Marks.FindAsync(id);

            if (mark != null)
            {
                _context.Marks.Remove(mark);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Marks");
        }
    }
}
