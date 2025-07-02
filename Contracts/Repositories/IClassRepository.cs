using SadSchool.Models.SqlServer;

namespace SadSchool.Contracts.Repositories
{
    public interface IClassRepository
    {
        /// <summary>
        /// Gets all classes.
        /// </summary>
        /// <returns>List of classes.</returns>
        Task<List<Class>> GetAllClassesAsync();

        /// <summary>
        /// Gets class by id.
        /// </summary>
        /// <param name="id">Class id.</param>
        /// <returns>Class object.</returns>
        Task<Class?> GetClassByIdAsync(int id);

        /// <summary>
        /// Adds a new class.
        /// </summary>
        /// <param name="theClass">Class object.</param>
        /// <returns>Task (void).</returns>
        Task<Class?> AddClassAsync(Class theClass);

        /// <summary>
        /// Updates an existing class.
        /// </summary>
        /// <param name="theClass">Class object.</param>
        /// <returns>Task (void).</returns>
        Task<bool> UpdateClassAsync(Class theClass);

        /// <summary>
        /// Deletes a class by id.
        /// </summary>
        /// <param name="id">Class id.</param>
        /// <returns>Task (void).</returns>
        Task<bool> DeleteClassByIdAsync(int id);
    }
}
