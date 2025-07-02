// <copyright file="IMarkRepository.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

using MongoDB.Bson;
using SadSchool.Models.Mongo;

namespace SadSchool.Contracts.Repositories
{
    public interface IMarkRepository
    {
        /// <summary>
        /// Gets all marks.
        /// </summary>
        /// <returns>List of marks.</returns>
        Task<List<Mark>> GetAllMarksAsync();
        
        /// <summary>
        /// Gets mark by id.
        /// </summary>
        /// <param name="id">Mark id.</param>
        /// <returns>Mark object.</returns>
        Task<Mark?> GetMarkByIdAsync(ObjectId id);

        /// <summary>
        /// Gets marks by student id.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <returns>Mark object.</returns>
        Task<List<Mark>> GetMarksByStudentIdAsync(int studentId);

        /// <summary>
        /// Gets marks by student id and lesson id.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="lessonId">The id of the lesson.</param>
        /// <returns>Mark object.</returns>
        Task<List<Mark>> GetMarksByStudentIdAndLessonIdAsync(int studentId, int lessonId);

        /// <summary>
        /// Adds a new mark.
        /// </summary>
        /// <param name="mark">Mark object.</param>
        /// <returns>Task (void).</returns>
        Task AddMarkAsync(Mark mark);
        
        /// <summary>
        /// Updates an existing mark.
        /// </summary>
        /// <param name="mark">Mark object.</param>
        /// <returns>Task (void).</returns>
        Task UpdateMarkAsync(Mark mark);
        
        /// <summary>
        /// Deletes a mark by id.
        /// </summary>
        /// <param name="id">Mark id.</param>
        /// <returns>Task (void).</returns>
        Task DeleteMarkByIdAsync(ObjectId id);
    }
}
