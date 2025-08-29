// <copyright file="IMarksAnalyticsService.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Contracts
{
    using SadSchool.Dtos;

    /// <summary>
    /// Represents the marks analytics service.
    /// </summary>
    public interface IMarksAnalyticsService
    {
        /// <summary>
        /// Gets the average marks.
        /// </summary>
        /// <param name="studentId">Desirable student's ID.</param>
        /// <param name="subjectId">Desirable subject's ID.</param>
        /// <returns>List of average marks.</returns>
        Task<List<AverageMarkDto>> GetAverageMarks(int studentId, int subjectId);
    }
}
