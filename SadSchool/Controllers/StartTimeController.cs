using Microsoft.AspNetCore.Mvc;
using SadSchool.Models;
using SadSchool.ViewModels;

namespace SadSchool.Controllers
{
    public class StartTimeController : Controller
    {
        private readonly SadSchoolContext _context;

        public StartTimeController(SadSchoolContext context)
        {
            _context = context;
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

            return View(@"~/Views/Data/StartTimes.cshtml", schedules);
        }

        [HttpGet]
        public IActionResult AddStartTime()
        {
            return View(@"~/Views/Data/StartTimeAdd.cshtml");
        }

        [HttpPost]
        public IActionResult AddStartTime(StartTimeViewModel model)
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
        public IActionResult EditStartTime(int id)
        {
            var editedStartTime = _context.StartTimes.Find(id);

            StartTimeViewModel viewModel = new()
            {
                StartTime = editedStartTime?.Value
            };

            return View(@"~/Views/Data/StartTimeEdit.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditStartTime(StartTimeViewModel viewModel)
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
        public IActionResult DeleteStartTime(int id)
        {
            var schedule = _context.StartTimes.Find(id);

            if (schedule != null)
            {
                _context.StartTimes.Remove(schedule);
                _context.SaveChanges();
            }

            return RedirectToAction("StartTimes");
        }
    }
}
