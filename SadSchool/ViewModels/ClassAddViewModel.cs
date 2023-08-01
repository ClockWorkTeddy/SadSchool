using SadSchool.Models;

namespace SadSchool.ViewModels
{
    public class ClassAddViewModel
    {
        public List<Teacher?> TeachersForView { get; set; } = new List<Teacher?>();
        public int TeacherId { get; set; }
        public string Name { get; set; } = null!;
    }
}
