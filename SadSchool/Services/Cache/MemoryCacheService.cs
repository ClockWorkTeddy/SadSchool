// <copyright file="MemoryCacheService.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Services.Cache
{
    using Microsoft.Extensions.Caching.Memory;
    using SadSchool.Contracts;
    using SadSchool.DbContexts;
    using SadSchool.Models.SqlServer;
    using Serilog;

    /// <summary>
    /// Memory cache service.
    /// </summary>
    public class MemoryCacheService : ICacheService
    {
        private readonly SadSchoolContext context;
        private readonly IMemoryCache memoryCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryCacheService"/> class.
        /// </summary>
        /// <param name="context">DB context instance.</param>
        /// <param name="memoryCache">Memory cache instance.</param>
        public MemoryCacheService(SadSchoolContext context, IMemoryCache memoryCache)
        {
            this.context = context;
            this.memoryCache = memoryCache;
        }

        /// <summary>
        /// Returns the object from the cache or from the database if it's not in the cache.
        /// </summary>
        /// <typeparam name="T">Desirable object type.</typeparam>
        /// <param name="id">Desirable object id.</param>
        /// <returns>List with the found object.</returns>
        public List<T?> GetObject<T>(int id)
            where T : class
        {
            Log.Information(
                "MemoryCacheService.GetObject(): method called with parameters: id = {id} and for type = {type}",
                id,
                typeof(T));

            var cacheKey = $"{typeof(T)}:{id}";

            if (!this.memoryCache.TryGetValue(cacheKey, out object? cachedObject))
            {
                cachedObject = this.context.Set<T>().Find(id);
                this.memoryCache.Set(cacheKey, cachedObject);
            }

            return new List<T?> { cachedObject as T };
        }

        /// <summary>
        /// Refreshes the object data if the object data was refreshed by the user.
        /// </summary>
        /// <typeparam name="T">Desirable object type.</typeparam>
        /// <param name="obj">Object that being refreshed.</param>
        public void RefreshObject<T>(T obj)
            where T : class
        {
            Log.Information(
                "MemoryCacheService.RefreshObject(): method called with parameters: obj = {obj} and for type = {type}",
                obj,
                typeof(T));

            var cacheKey = $"{typeof(T)}:{(obj as BaseModel)?.Id}";

            if (this.memoryCache.TryGetValue(cacheKey, out object? cachedObject))
            {
                this.memoryCache.Set(cacheKey, obj as T);
            }
        }

        /// <summary>
        /// Removes the object from the cache.
        /// </summary>
        /// <typeparam name="T">Desirable object type.</typeparam>
        /// <param name="obj">Object that being removed.</param>
        public void RemoveObject<T>(T obj)
            where T : class
        {
            Log.Information(
                "MemoryCacheService.RemoveObject(): method called with parameters: obj = {obj} and for type = {type}",
                obj,
                typeof(T));

            var cacheKey = $"{typeof(T)}:{(obj as BaseModel)?.Id}";

            this.memoryCache.Remove(cacheKey);
        }
    }
}
