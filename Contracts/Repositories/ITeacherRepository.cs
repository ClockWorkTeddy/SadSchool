using SadSchool.Models.SqlServer;

namespace SadSchool.Contracts.Repositories
{
    /// <summary>
    /// Defines a contract for managing teacher-related data operations in an asynchronous manner.
    /// </summary>
    /// <remarks>This interface provides methods to perform CRUD operations and query teachers based on
    /// various criteria, such as name, date of birth, or grade level. All methods are asynchronous and return tasks to
    /// ensure non-blocking operations. Implementations of this interface should handle data persistence and retrieval
    /// while adhering to the specified method contracts.</remarks>
    public interface ITeacherRepository : IBaseRepository
    {
        /// <summary>
        /// Asynchronously retrieves a list of teachers whose first name matches the specified value.
        /// </summary>
        /// <param name="firstName">The first name to search for. This parameter is case-insensitive and cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of  <see cref="Teacher"/>
        /// objects whose first name matches the specified value. If no matches are found,  the list will be empty.</returns>
        Task<List<Teacher>> GetTeachersByFirstNameAsync(string firstName);

        /// <summary>
        /// Asynchronously retrieves a list of teachers whose last names match the specified value.
        /// </summary>
        /// <param name="lastName">The last name to search for. This value is case-insensitive and cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of  <see cref="Teacher"/>
        /// objects whose last names match the specified value. If no matches are found,  the list will be empty.</returns>
        Task<List<Teacher>> GetTeachersByLastNameAsync(string lastName);

        /// <summary>
        /// Asynchronously retrieves a list of teachers who were born on the specified date.
        /// </summary>
        /// <remarks>This method performs a query to find all teachers with the specified date of birth.
        /// The operation is asynchronous and does not block the calling thread.</remarks>
        /// <param name="dateOfBirth">The date of birth to filter teachers by.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="Teacher"/>
        /// objects whose date of birth matches the specified value. If no teachers are found, the list will be empty.</returns>
        Task<List<Teacher>> GetTeachersByDateOfBirthAsync(DateOnly dateOfBirth);

        /// <summary>
        /// Asynchronously retrieves a list of teachers assigned to the specified grade.
        /// </summary>
        /// <param name="grade">The grade level for which to retrieve the teachers. Must be a non-negative integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="Teacher"/>
        /// objects assigned to the specified grade. If no teachers are found, the list will be empty.</returns>
        Task<List<Teacher>> GetTeachersByGradeAsync(int grade);
    }
}
