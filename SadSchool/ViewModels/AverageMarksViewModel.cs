using SadSchool.Models;
using SadSchool.Services.ApiServices;

namespace SadSchool.ViewModels
{
    public class AverageMarksViewModel
    {
        public AverageMark[,] AverageMarksTable;
        public List<string> Students;
        public List<string> Subjects;
    }
}
