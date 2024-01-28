﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SadSchool.Models;
using SadSchool.ViewModels;
using SadSchool.Services;

namespace SadSchool.Controllers
{
    public class StudentController : Controller
    {
        private readonly SadSchoolContext _context;
        private readonly INavigationService _navigationService;
        private readonly ICacheService _cacheService;
        public StudentController(SadSchoolContext context, INavigationService navigationService, ICacheService cacheService)
        {
            _context = context;
            _navigationService = navigationService;
            _cacheService = cacheService;
        }

        [HttpGet]
        public IActionResult Students()
        {
            var students = new List<StudentViewModel>();

            foreach (var student in _context.Students.Include(s => s.Class).ToList())
            {
                students.Add(new StudentViewModel
                {
                    Id = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    DateOfBirth = student.DateOfBirth,
                    Sex = student.Sex,
                    ClassName = student.Class?.Name
                });
            }

            _navigationService.RefreshBackParams(RouteData);

            return View(@"~/Views/Data/Students.cshtml", students);
        }

        [HttpGet]
        public IActionResult Add()
        {
            if (User.Identity.IsAuthenticated && !User.IsInRole("user"))
            {

                StudentViewModel viewModel = new StudentViewModel()
                {
                    Classes = GetClassesList(null),
                    Sexes = GetSexes(null)
                };

                _navigationService.RefreshBackParams(RouteData);

                return View(@"~/Views/Data/StudentAdd.cshtml", viewModel);
            }

            return RedirectToAction("Students");
        }

        private List<SelectListItem> GetClassesList(int? classId)
        {
            var classes = _context.Classes.ToList();

            return classes.Select(Class => new SelectListItem
            {
                Value = Class.Id.ToString(),
                Text = $"{Class.Name}",
                Selected = Class.Id == classId
            }).ToList();
        }

        [HttpPost]
        public async Task<IActionResult> Add(StudentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var student = new Student
                {
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    ClassId = viewModel.ClassId,
                    Class = _context.Classes.Find(viewModel.ClassId),
                    DateOfBirth = viewModel.DateOfBirth,
                    Sex = viewModel.Sex
                };

                _context.Students.Add(student);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Students");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (User.Identity.IsAuthenticated && !User.IsInRole("user"))
            {

                var editedStudent = _context.Students.Find(id);

                StudentViewModel viewModel = new()
                {
                    FirstName = editedStudent?.FirstName,
                    LastName = editedStudent?.LastName,
                    DateOfBirth = editedStudent?.DateOfBirth,
                    Sexes = GetSexes(editedStudent?.Sex),
                    Classes = GetClassesList(editedStudent?.ClassId)
                };

                _navigationService.RefreshBackParams(RouteData);

                return View(@"~/Views/Data/StudentEdit.cshtml", viewModel);
            }
                
            return RedirectToAction("Students");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StudentViewModel viewModel)
        {
            if (ModelState.IsValid && viewModel != null)
            {
                var student = new Student
                {
                    Id = viewModel.Id.Value,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    DateOfBirth = viewModel.DateOfBirth,
                    Sex = viewModel.Sex,
                    ClassId = viewModel.ClassId
                };

                _context.Students.Update(student);
                await _context.SaveChangesAsync();
                _cacheService.RefreshObject(student);

                return RedirectToAction("Students");
            }
            else
            {
                return View(@"~/Views/Data/StudentEdit.cshtml", viewModel);
            }
        }

        private List<SelectListItem> GetSexes(bool? sex)
        {
            List<SelectListItem> sexes = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Value = "False",
                    Text = "Male",
                    Selected = sex.HasValue ? !sex.Value : false
                },

                new SelectListItem()
                {
                    Value = "True",
                    Text = "Female",
                    Selected = sex.HasValue ? sex.Value : false
                }
            };

            return sexes;
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (User.Identity.IsAuthenticated && !User.IsInRole("user"))
            {
                var student = await _context.Students.FindAsync(id);

                if (student != null)
                {
                    _context.Students.Remove(student);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Students");
        }
    }
}
