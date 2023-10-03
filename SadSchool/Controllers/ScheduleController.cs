using Microsoft.AspNetCore.Mvc;
using SadSchool.Services;
using SadSchool.Services.Schedule;
using SadSchool.Models;
using SadSchool.Services;
using Microsoft.EntityFrameworkCore;
using SadSchool.ViewModels;

namespace SadSchool.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly SadSchoolContext _context;
        private readonly INavigationService _navigationService;

        public ScheduleController(SadSchoolContext context, INavigationService navigationService)
        {
            _context = context;
            _navigationService = navigationService;
        }

        [HttpGet]
        public IActionResult GetSchedule()
        {
            var scheduledLessons = _context.ScheduledLessons
                .Include(sl => sl.Class)
                .Include(sl => sl.Subject)
                .Include(sl => sl.Teacher)
                .Include(sl => sl.StartTime).ToList();

            ScheduleService service = new ScheduleService(scheduledLessons);
            var scheduleCells = service.GetScheduleCells();

            return View(@"~/Views/Data/Representation/Schedule.cshtml", new ScheduleViewModel() 
            { 
                Classes = service.Classes,
                Cells = scheduleCells
            });
        }
    }
}
