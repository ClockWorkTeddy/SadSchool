using SadSchool.Models.SqlServer;

namespace SadSchool.Contracts.Repositories
{
    public interface IBaseRepository
    {
        /// <summary>
        /// Gets all entities of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the entities.</typeparam>
        /// <returns></returns>
        Task<List<T>> GetAllEntitiesAsync<T>()
            where T : BaseModel;

        /// <summary>
        /// Gets an entity by its identifier.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="id">Id of the entity.</param>
        /// <returns></returns>
        Task<T?> GetEntityByIdAsync<T>(int id)
            where T : BaseModel;

        /// <summary>
        /// Adds a new entity of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">Added entity object.</param>
        /// <returns></returns>
        Task<T?> AddEntityAsync<T>(T entity)
            where T : BaseModel;

        /// <summary>
        /// Updates an existing entity of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">Updated entity object.</param>
        /// <returns></returns>
        Task<bool> UpdateEntityAsync<T>(T entity)
            where T : BaseModel;

        /// <summary>
        /// Deletes an entity of the specified type by its identifier.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="id">Id of the entity.</param>
        /// <returns></returns>
        Task<bool> DeleteEntityAsync<T>(int id)
            where T : BaseModel;
    }
}
