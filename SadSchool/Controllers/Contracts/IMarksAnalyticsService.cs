using SadSchool.Services.ApiServices;

namespace SadSchool.Controllers.Contracts
{
    public interface IMarksAnalyticsService
    {
        List<AverageMarkModel> GetAverageMarks(int studentName, int subjectName);
    }
}
