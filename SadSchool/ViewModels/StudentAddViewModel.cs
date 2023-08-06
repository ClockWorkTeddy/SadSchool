using SadSchool.Models;

namespace SadSchool.ViewModels
{
    public class StudentAddViewModel
    {
        public List<Class?> ClassesForView { get; set; } = new List<Class?>();
        public int ClassId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string DateOfBirth { get; set; }
        public string Sex { get; set; }    
    }
}
