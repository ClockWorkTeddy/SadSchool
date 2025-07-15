using SadSchool.Models.SqlServer;

namespace SadSchool.Contracts.Repositories
{
    public interface IClassRepository : IBaseRepository
    {
        /// <summary>
        /// Gets a class by its name asynchronously.
        /// </summary>
        /// <param name="className">The name of the class.</param>
        /// <returns>Class object.</returns>
        Task<Class?> GetClassByNameAsync(string className);

        /// <summary>
        /// Retrieves a list of classes associated with a specific teacher by their identifier asynchronously.
        /// </summary>
        /// <param name="teacherId">The Id of the teacher.</param>
        /// <returns>The list of classes.</returns>
        Task<List<Class>?> GetClassesByTeacherIdAsync(int teacherId);
    }
}
