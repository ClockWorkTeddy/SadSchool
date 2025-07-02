using SadSchool.Models.SqlServer;

namespace SadSchool.Contracts.Repositories
{
    public interface ILessonRepository
    {

        /// <summary>
        /// Gets all lessons.
        /// </summary>
        /// <returns></returns>
        Task<List<Lesson>> GetAllLessonsAsync();

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

        /// <summary>
        /// Asynchronously retrieves a lesson by its unique identifier.
        /// </summary>
        /// <remarks>Use this method to fetch a specific lesson when you know its unique identifier.
        /// Ensure that the provided <paramref name="lessonId"/> is valid and greater than zero.</remarks>
        /// <param name="lessonId">The unique identifier of the lesson to retrieve. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Lesson"/> object
        /// corresponding to the specified <paramref name="lessonId"/>, or <see langword="null"/> if no lesson is found.</returns>
        Task<Lesson?> GetLessonByIdAsync(int lessonId);

        /// <summary>
        /// Asynchronously adds a new lesson to the system.
        /// </summary>
        /// <param name="lesson">The lesson to add. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<Lesson?> AddLessonAsync(Lesson lesson);

        /// <summary>
        /// Updates the specified lesson with new information asynchronously.
        /// </summary>
        /// <remarks>Use this method to persist changes to an existing lesson. Ensure that the provided
        /// <see cref="Lesson"/> object contains valid and complete data before calling this method.</remarks>
        /// <param name="lesson">The <see cref="Lesson"/> object containing the updated lesson details. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<bool> UpdateLessonAsync(Lesson lesson);

        /// <summary>
        /// Deletes the lesson with the specified identifier asynchronously.
        /// </summary>
        /// <param name="lessonId">The unique identifier of the lesson to delete. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        Task<bool> DeleteLessonAsync(int lessonId);
    }
}
