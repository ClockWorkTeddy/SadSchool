// <copyright file="IMarksAnalyticsService.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers.Contracts
{
    using SadSchool.Services.ApiServices;

    /// <summary>
    /// Represents the marks analytics service.
    /// </summary>
    public interface IMarksAnalyticsService
    {
        /// <summary>
        /// Gets the average marks.
        /// </summary>
        /// <param name="studentName">Desirable student's name.</param>
        /// <param name="subjectName">Desirable subject's name.</param>
        /// <returns>List of average marks.</returns>
        List<AverageMarkModel> GetAverageMarks(int studentName, int subjectName);
    }
}
