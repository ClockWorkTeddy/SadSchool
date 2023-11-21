using SadSchool.Services.ClassBook;
using SadSchool.ViewModels;

namespace SadSchool.Controllers.Contracts
{
    public interface IClassBookService
    {
        public void GetMarkData();
        public ClassBookViewModel GetClassBookViewModel(string subjectName, string className);

        public List<string> Dates { get; }
        public List<string> Students { get; }
    }
}
