using Microsoft.AspNetCore.Mvc;
using SadSchool.Models;
using SadSchool.ViewModels;

namespace SadSchool.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly SadSchoolContext _context;

        public ScheduleController(SadSchoolContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Schedules()
        {
            List<ScheduleViewModel> schedules = new List<ScheduleViewModel>();

            foreach (var schedule in _context.SchedulePositions)
            {
                schedules.Add(new ScheduleViewModel
                {
                    Id = schedule.Id,
                    StartTime = schedule.Value,
                });
            }

            return View(@"~/Views/Data/Schedules.cshtml", schedules);
        }

        [HttpGet]
        public IActionResult AddSchedule()
        {
            return View(@"~/Views/Data/ScheduleAdd.cshtml");
        }

        [HttpPost]
        public IActionResult AddSchedule(ScheduleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var schedule = new StartTime
                {
                    Value = model.StartTime,
                };

                _context.SchedulePositions.Add(schedule);
                _context.SaveChanges();
            }

            return RedirectToAction("Schedules");
        }

        [HttpPost]
        public IActionResult DeleteSchedule(int id)
        {
            var schedule = _context.SchedulePositions.Find(id);

            if (schedule != null)
            {
                _context.SchedulePositions.Remove(schedule);
                _context.SaveChanges();
            }

            return RedirectToAction("Schedules");
        }
    }
}
