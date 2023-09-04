﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                .Include(m => m.Lesson.StartTime))
            { 
                marks.Add(new MarkViewModel
                {
                    Id = mark.Id,
                    Value = mark.Value,
                    Student = $"{mark.Student?.FirstName} {mark.Student?.LastName}",
                    Lesson = $"{mark.Lesson?.Subject.Name} {mark.Lesson?.Date} {mark.Lesson?.StartTime?.Value}"
                });
            }

            return View(@"~/Views/Data/Marks.cshtml", marks);
        }

        [HttpGet]
        public IActionResult AddMark()
        {
            MarkViewModel viewModel = new MarkViewModel()
            {
                Students = GetStudentsList(null),
                Lessons = GetLessonsList(null),
            };

            return View(@"~/Views/Data/MarkAdd.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddMark(MarkViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var mark = new Mark
                {
                    Value = viewModel.Value,
                    StudentId = viewModel.StudentId,
                    LessonId = viewModel.LessonId
                };

                _context.Marks.Add(mark);
                await _context.SaveChangesAsync();

                return RedirectToAction("Marks");
            }
            else
            {
                return View(@"~/Views/Data/MarkAdd.cshtml", viewModel);
            }
        }

        [HttpGet]
        public IActionResult EditMark(int id)
        {
            var editedMark = _context.Marks.Find(id);

            MarkViewModel viewModel = new()
            {
                Value = editedMark?.Value,
                Lessons = GetLessonsList(editedMark?.LessonId),
                Students = GetStudentsList(editedMark?.StudentId)
            };

            return View(@"~/Views/Data/MarkEdit.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditMark(MarkViewModel viewModel)
        {
            if (ModelState.IsValid && viewModel != null)
            {
                var mark = new Mark
                {
                    Id = viewModel.Id,
                    Value = viewModel.Value,
                    LessonId = viewModel.LessonId,
                    StudentId = viewModel.StudentId,
                };

                _context.Marks.Update(mark);
                await _context.SaveChangesAsync();
                return RedirectToAction("Marks");
            }
            else
            {
                return View(@"~/Views/Data/MarkEdit.cshtml", viewModel);
            }
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

        private List<SelectListItem> GetLessonsList(int? lessonId)
        {
            var lessons = _context.Lessons
                .Include(l => l.StartTime)
                .Include(l => l.Subject)
                .Include(l => l.Class).ToList();

            return lessons.Select(lesson => new SelectListItem
            {
                Value = lesson.Id.ToString(),
                Text = $"{lesson.StartTime.Value} {lesson.Subject.Name} {lesson.Class.Name} {lesson.Date}",
                Selected = lesson.Id == lessonId
            }).ToList();
        }

        private List<SelectListItem> GetStudentsList(int? studentId)
        {
            var students = _context.Students.ToList();

            return students.Select(student => new SelectListItem
            {
                Value = student.Id.ToString(),
                Text = $"{student.FirstName} {student.LastName}",
                Selected = student.Id == studentId
            }).ToList();
        }

    }
}
