// <copyright file="BaseRepository.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using SadSchool.Contracts;
    using SadSchool.Contracts.Repositories;
    using SadSchool.DbContexts;
    using SadSchool.Models.SqlServer;
    using Serilog;

    /// <inheritdoc/>
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseRepository"/> class.
    /// </summary>
    /// <param name="context">DB context.</param>
    /// <param name="cacheService">Cache service instance.</param>
    public abstract class BaseRepository(SadSchoolContext context, ICacheService cacheService) : IBaseRepository
    {
        /// <summary>
        /// Gets database context instance.
        /// </summary>
        protected SadSchoolContext Context { get; } = context;

        /// <summary>
        /// Gets cache service instance.
        /// </summary>
        protected ICacheService CacheService { get; } = cacheService;

        /// <inheritdoc/>
        public async Task<List<T>> GetAllEntitiesAsync<T>()
            where T : BaseModel
        {
            var entities = this.CacheService.GetObjects<T>();

            if (entities != null)
            {
                return entities;
            }

            entities = await this.Context.Set<T>().ToListAsync();

            this.CacheService.SetObjects(entities);

            return entities;
        }

        /// <inheritdoc/>
        public async Task<T?> GetEntityByIdAsync<T>(int id)
            where T : BaseModel
        {
            var entity = this.CacheService.GetObject<T>(id);

            if (entity != null)
            {
                return entity;
            }

            entity = await this.Context.Set<T>().FindAsync(id);

            if (entity != null)
            {
                this.CacheService.SetObject(entity);
            }

            return entity;
        }

        /// <inheritdoc/>
        public async Task<T?> AddEntityAsync<T>(T entity)
    where T : BaseModel
        {
            try
            {
                await this.Context.Set<T>().AddAsync(entity);
                await this.Context.SaveChangesAsync();

                this.CacheService.RemoveObjects<T>();
                this.CacheService.SetObject(entity);

                return entity;
            }
            catch (Exception ex)
            {
                // Handle the exception as needed (e.g., log it, rethrow it, etc.)
                // For now, we just return null to indicate failure.
                Log.Error(ex, "Error adding to the database {Message}", ex.Message);

                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateEntityAsync<T>(T entity)
            where T : BaseModel
        {
            var existing = await this.Context.Set<T>().FindAsync(entity.Id);

            if (existing == null)
            {
                Log.Error("Entity of type {EntityType} with ID {EntityId} not found", typeof(T), entity.Id);

                return false;
            }

            this.Context.Entry(existing).CurrentValues.SetValues(entity);

            await this.Context.SaveChangesAsync();

            this.CacheService.RemoveObject(existing);
            this.CacheService.RemoveObjects<T>();

            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteEntityAsync<T>(int id)
            where T : BaseModel
        {
            var entity = await this.GetEntityByIdAsync<T>(id);

            if (entity != null)
            {
                this.Context.Set<T>().Remove(entity);
                await this.Context.SaveChangesAsync();

                this.CacheService.RemoveObject(entity);
                this.CacheService.RemoveObjects<T>();

                return true;
            }
            else
            {
                Log.Error("Entity of type {EntityType} with ID {EntityId} not found", typeof(T), id);
                return false;
            }
        }
    }
}
