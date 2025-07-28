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
    /// <remarks>
    /// Initializes a new instance of the <see cref="MemoryCacheService"/> class.
    /// </remarks>
    /// <param name="context">DB context instance.</param>
    /// <param name="memoryCache">Memory cache instance.</param>
    public class MemoryCacheService(SadSchoolContext context, IMemoryCache memoryCache) : ICacheService
    {
        private readonly SadSchoolContext context = context;
        private readonly IMemoryCache memoryCache = memoryCache;

        /// <summary>
        /// Returns the object from the cache or from the database if it's not in the cache.
        /// </summary>
        /// <typeparam name="T">Desirable object type.</typeparam>
        /// <param name="id">Desirable object id.</param>
        /// <returns>List with the found object.</returns>
        public T? GetObject<T>(int id)
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

            return cachedObject as T;
        }

        /// <summary>
        /// Retrieves a list of objects of the specified type from the cache or database.
        /// </summary>
        /// <remarks>This method first attempts to retrieve the objects from the memory cache. If the
        /// objects are not found in the cache, it queries the database context to retrieve them and stores the result
        /// in the cache for future requests. This can improve performance by reducing database access for frequently
        /// requested data.</remarks>
        /// <typeparam name="T">The type of objects to retrieve. Must be a reference type.</typeparam>
        /// <returns>A list of objects of type <typeparamref name="T"/>. Returns an empty list if no objects are found.</returns>
        public List<T>? GetObjects<T>()
            where T : class
        {
            Log.Information(
                "MemoryCacheService.GetObjects(): method called for type = {type}",
                typeof(T));
            var cacheKey = $"{typeof(T)}:All";

            if (!this.memoryCache.TryGetValue(cacheKey, out List<T>? cachedObjects))
            {
                cachedObjects = [.. this.context.Set<T>()];
                this.memoryCache.Set(cacheKey, cachedObjects);
            }

            return cachedObjects;
        }

        /// <inheritdoc />
        public void SetObject<T>(T obj)
            where T : class
        {
            Log.Information(
                "MemoryCacheService.RefreshObject(): method called with parameters: obj = {obj} and for type = {type}",
                obj,
                typeof(T));

            var cacheKey = $"{typeof(T)}:{(obj as BaseModel)?.Id}";

            if (this.memoryCache.TryGetValue(cacheKey, out _))
            {
                this.memoryCache.Set(cacheKey, obj as T);
            }
        }

        /// <inheritdoc />
        public void SetObjects<T>(List<T> objects)
            where T : class
        {
            Log.Information(
                "MemoryCacheService.SetObjects(): method called for type = {type}",
                typeof(T));
            var cacheKey = $"{typeof(T)}:All";
            this.memoryCache.Set(cacheKey, objects);
        }

        /// <inheritdoc />
        public void RemoveObjects<T>()
            where T : class
        {
            Log.Information(
                "MemoryCacheService.RemoveObjects(): method called for type = {type}",
                typeof(T));
            var cacheKey = $"{typeof(T)}:All";
            this.memoryCache.Remove(cacheKey);
        }

        /// <inheritdoc />
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
