using SadSchool.Models.SqlServer;

namespace SadSchool.Contracts.Repositories
{
    public interface IStudentRepository : IBaseRepository
    {
        /// <summary>
        /// Asynchronously retrieves a list of students with the specified first name.
        /// </summary>
        /// <param name="firstName">The first name of the students to search for. Cannot be null or empty.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of <see cref="Student"/>
        /// objects whose first name matches the specified value. The list will be empty if no students are found.</returns>
        Task<List<Student>> GetStudentsByFirstNameAsync(string firstName);

        /// <summary>
        /// Asynchronously retrieves a list of students whose last name matches the specified value.
        /// </summary>
        /// <param name="lastName">The last name to search for. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="Student"/>
        /// objects with the specified last name. The list will be empty if no students are found.</returns>
        Task<List<Student>> GetStudentsByLastNameAsync(string lastName);

        /// <summary>
        /// Asynchronously retrieves a list of students born on the specified date.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth to filter students by.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of students born on the
        /// specified date. The list will be empty if no students match the criteria.</returns>
        Task<List<Student>> GetStudentsByDateOfBirthAsync(DateOnly dateOfBirth);

        /// <summary>
        /// Asynchronously retrieves a list of students enrolled in the specified class.
        /// </summary>
        /// <param name="classId">The unique identifier of the class for which to retrieve students. Must be a positive integer.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of <see cref="Student"/>
        /// objects enrolled in the specified class. The list will be empty if no students are found.</returns>
        Task<List<Student>> GetStudentsByClassIdAsync(int classId);

        /// <summary>
        /// Asynchronously retrieves a list of students filtered by their sex.
        /// </summary>
        /// <param name="sex">A boolean value indicating the sex to filter by.  <see langword="true"/> for male students; <see
        /// langword="false"/> for female students.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of  <see cref="Student"/>
        /// objects that match the specified sex. The list will be empty if no students match.</returns>
        Task<List<Student>> GetStudentsBySexAsync(bool sex);
    }
}
