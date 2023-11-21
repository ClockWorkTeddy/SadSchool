using SadSchool.Services.ClassBook;

namespace SadSchool.ViewModels
{
    public class ClassBookViewModel
    {
        public string ClassName { get; set; }
        public string SubjectName { get; set; }
        public List<string> Dates { get; set; }
        public List<string> Students { get; set; }
        public MarkCell[,] MarkCells { get; set; }
    }
}