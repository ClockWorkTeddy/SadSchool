using Microsoft.AspNetCore.Mvc;
using SadSchool.Models;
using SadSchool.ViewModels;
using SadSchool.Services;

namespace SadSchool.Controllers
{
    public class StartTimeController : Controller
    {
        private readonly SadSchoolContext _context;
        private readonly INavigationService _navigationService;

        public StartTimeController(SadSchoolContext context, INavigationService navigationService)
        {
            _context = context;
            _navigationService = navigationService;
        }

        [HttpGet]
        public IActionResult StartTimes()
        {
            List<StartTimeViewModel> schedules = new List<StartTimeViewModel>();

            foreach (var schedule in _context.StartTimes)
            {
                schedules.Add(new StartTimeViewModel
                {
                    Id = schedule.Id,
                    StartTime = schedule.Value,
                });
            }

            _navigationService.RefreshBackParams(RouteData);

            return View(@"~/Views/Data/StartTimes.cshtml", schedules);
        }

        [HttpGet]
        public IActionResult Add()
        {
            if (User.Identity.IsAuthenticated && !User.IsInRole("user"))
            {
                _navigationService.RefreshBackParams(RouteData);

                return View(@"~/Views/Data/StartTimeAdd.cshtml");
            }

            return RedirectToAction("StartTimes");
        }

        [HttpPost]
        public IActionResult Add(StartTimeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var schedule = new StartTime
                {
                    Value = model.StartTime,
                };

                _context.StartTimes.Add(schedule);
                _context.SaveChanges();
            }

            return RedirectToAction("StartTimes");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (User.Identity.IsAuthenticated && !User.IsInRole("user"))
            {

                var editedStartTime = _context.StartTimes.Find(id);

                StartTimeViewModel viewModel = new()
                {
                    StartTime = editedStartTime?.Value
                };

                _navigationService.RefreshBackParams(RouteData);

                return View(@"~/Views/Data/StartTimeEdit.cshtml", viewModel);
            }

            return RedirectToAction("StartTimes");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StartTimeViewModel viewModel)
        {
            if (ModelState.IsValid && viewModel != null)
            {
                var startTime = new StartTime
                {
                    Id = viewModel.Id,
                    Value = viewModel.StartTime
                };

                _context.StartTimes.Update(startTime);
                await _context.SaveChangesAsync();
                return RedirectToAction("StartTimes");
            }
            else
            {
                return View(@"~/Views/Data/StartTimeEdit.cshtml", viewModel);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (User.Identity.IsAuthenticated && !User.IsInRole("user"))
            {
                var schedule = _context.StartTimes.Find(id);

                if (schedule != null)
                {
                    _context.StartTimes.Remove(schedule);
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("StartTimes");
        }
    }
}
