using SadSchool.Models;

namespace SadSchool.Controllers
{
    public class ClassController
    {
        private readonly SadSchoolContext _context;

        public ClassController(SadSchoolContext context)
        {
            _context = context;
        }
    }
}
