using SadSchool.Models;
namespace SadSchool.ViewModels
{
    public class ClassViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Teacher? Teacher { get; set; }
        public Student? Leader { get; set; } 
    }
}
