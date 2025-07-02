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
    public interface ITeacherRepository
    {
        /// <summary>
        /// Asynchronously retrieves a list of all teachers.
        /// </summary>
        /// <remarks>This method does not filter or paginate the results. It retrieves all available
        /// teachers in the system.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="Teacher"/>
        /// objects representing all teachers. If no teachers are found, the list will be empty.</returns>
        Task<List<Teacher>> GetAllTeachersAsync();

        /// <summary>
        /// Asynchronously retrieves a teacher by their unique identifier.
        /// </summary>
        /// <remarks>Use this method to fetch a teacher's details when their unique identifier is known.
        /// If no teacher is found with the specified ID, the method returns <see langword="null"/>.</remarks>
        /// <param name="id">The unique identifier of the teacher to retrieve. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Teacher"/> object
        /// if a teacher with the specified ID exists; otherwise, <see langword="null"/>.</returns>
        Task<Teacher?> GetTeacherByIdAsync(int id);

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

        /// <summary>
        /// Asynchronously adds a new teacher to the system.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to add a teacher to the system. Ensure
        /// that the provided  <paramref name="teacher"/> object contains valid data before calling this
        /// method.</remarks>
        /// <param name="teacher">The <see cref="Teacher"/> object representing the teacher to be added. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the added <see cref="Teacher"/>
        /// object,  or <see langword="null"/> if the operation fails.</returns>
        Task<Teacher?> AddTeacherAsync(Teacher teacher);

        /// <summary>
        /// Updates the details of an existing teacher in the system asynchronously.
        /// </summary>
        /// <remarks>This method performs the update operation asynchronously. Ensure that the <paramref
        /// name="teacher"/> object contains valid data and that the <c>Id</c> property matches an existing teacher in
        /// the system.</remarks>
        /// <param name="teacher">The <see cref="Teacher"/> object containing the updated information. The <c>Id</c> property must correspond
        /// to an existing teacher.</param>
        /// <returns><see langword="true"/> if the update operation succeeds; otherwise, <see langword="false"/>.</returns>
        Task<bool> UpdateTeacherAsync(Teacher teacher);

        /// <summary>
        /// Deletes a teacher with the specified identifier asynchronously.
        /// </summary>
        /// <remarks>This method performs the deletion operation asynchronously. Ensure that the teacher
        /// with the specified  identifier exists before calling this method to avoid unnecessary operations.</remarks>
        /// <param name="id">The unique identifier of the teacher to delete. Must be a positive integer.</param>
        /// <returns><see langword="true"/> if the teacher was successfully deleted; otherwise, <see langword="false"/>.</returns>
        Task<bool> DeleteTeacherAsync(int id);
    }
}
