using SadSchool.Models.SqlServer;

namespace SadSchool.Contracts.Repositories
{
    public interface ILessonRepository : IBaseRepository
    {
        /// <summary>
        /// Gets all lessons on the particular date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<List<Lesson>> GetLessonsByDateAsync(string date);

        /// <summary>
        /// Retrieves a list of lessons associated with the specified scheduled lesson ID.
        /// </summary>
        /// <param name="scheduledLessonId">The unique identifier of the scheduled lesson. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of  <see cref="Lesson"/>
        /// objects associated with the specified scheduled lesson ID.  Returns an empty list if no lessons are found.</returns>
        Task<Lesson?> GetLessonByScheduledLessonIdAsync(int scheduledLessonId);
    }
}
