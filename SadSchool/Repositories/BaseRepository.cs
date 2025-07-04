using Microsoft.EntityFrameworkCore;
using SadSchool.Contracts;
using SadSchool.Contracts.Repositories;
using SadSchool.DbContexts;
using SadSchool.Models.SqlServer;
using Serilog;

namespace SadSchool.Repositories
{
    /// <inheritdoc/>
    public abstract class BaseRepository : IBaseRepository
    {
        #pragma warning disable SA1401 // Fields should be private
        /// <summary>
        /// Database context instance.
        /// </summary>
        protected readonly SadSchoolContext context;

        /// <summary>
        /// Cache service instance.
        /// </summary>
        protected readonly ICacheService cacheService;
#pragma warning restore SA1401

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository"/> class.
        /// </summary>
        /// <param name="context">DB context.</param>
        /// <param name="cacheService">Cache service instance.</param>
        public BaseRepository(SadSchoolContext context, ICacheService cacheService)
        {
            this.context = context;
            this.cacheService = cacheService;
        }

        /// <inheritdoc/>
        public async Task<List<T>> GetAllEntitiesAsync<T>()
            where T : BaseModel
        {
            var entities = this.cacheService.GetObjects<T>();

            if (entities != null)
            {
                return entities;
            }

            entities = await this.context.Set<T>().ToListAsync();

            this.cacheService.SetObjects(entities);

            return entities;
        }

        /// <inheritdoc/>
        public async Task<T?> GetEntityByIdAsync<T>(int id)
            where T : BaseModel
        {
            var entity = this.cacheService.GetObject<T>(id);

            if (entity != null)
            {
                return entity;
            }

            entity = await this.context.Set<T>().FindAsync(id);

            if (entity != null)
            {
                this.cacheService.SetObject(entity);
            }

            return entity;
        }

        /// <inheritdoc/>
        public async Task<T?> AddEntityAsync<T>(T entity)
            where T : BaseModel
        {
            try
            {
                await this.context.Set<T>().AddAsync(entity);
                await this.context.SaveChangesAsync();

                this.cacheService.RemoveObjects<T>();
                this.cacheService.SetObject(entity);

                return entity;
            }
            catch (Exception ex)
            {
                // Handle the exception as needed (e.g., log it, rethrow it, etc.)
                // For now, we just return null to indicate failure.
                Log.Error(ex, $"Error adding {typeof(T)} to the database.");

                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateEntityAsync<T>(T entity)
            where T : BaseModel
        {
            var existing = await this.context.Set<T>().FindAsync(entity.Id);

            if (existing == null)
            {
                Log.Error($"Entity of type {typeof(T)} with ID {entity.Id} not found");

                return false;
            }

            this.context.Entry(existing).CurrentValues.SetValues(entity);

            await this.context.SaveChangesAsync();

            this.cacheService.RemoveObject(existing);
            this.cacheService.RemoveObjects<T>();

            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteEntityAsync<T>(int id)
            where T : BaseModel
        {
            var entity = await this.GetEntityByIdAsync<T>(id);

            if (entity != null)
            {
                this.context.Set<T>().Remove(entity);
                await this.context.SaveChangesAsync();

                this.cacheService.RemoveObject(entity);
                this.cacheService.RemoveObjects<T>();

                return true;
            }
            else
            {
                Log.Error($"Entity of type {typeof(T)} with ID {id} not found for deletion.");
                return false;
            }
        }
    }
}
