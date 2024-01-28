using Microsoft.AspNetCore.Mvc.Rendering;

namespace SadSchool.ViewModels
{
    public class StudentSubjectSelectorViewModel
    {
        public int SelectedStudentId { get; set; } 
        public int SelectedSubjectId { get; set; }
        public List<SelectListItem> Students { get; set; }
        public List<SelectListItem> Subjects { get; set; }
    }
}
