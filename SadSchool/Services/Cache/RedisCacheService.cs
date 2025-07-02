// <copyright file="RedisCacheService.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Services.Cache
{
    using System.Text.Json;
    using SadSchool.Contracts;
    using SadSchool.DbContexts;
    using SadSchool.Models.SqlServer;
    using Serilog;
    using StackExchange.Redis;

    /// <summary>
    /// Class for Redis caching.
    /// </summary>
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase redis;
        private readonly JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCacheService"/> class.
        /// </summary>
        /// <param name="muxer">Redis muxer object.</param>
        public RedisCacheService(IConnectionMultiplexer muxer)
        {
            this.redis = muxer.GetDatabase();
        }

        /// <summary>
        /// Retrieves a list of objects of the specified type from the cache or database.
        /// </summary>
        /// <remarks>This method first attempts to retrieve the objects from the Redis cache. If the
        /// objects are not found in the cache, it queries the database, serializes the result, stores it in the cache,
        /// and then returns the deserialized list.</remarks>
        /// <typeparam name="T">The type of objects to retrieve. Must be a reference type.</typeparam>
        /// <returns>A list of objects of type <typeparamref name="T"/>. Returns <see langword="null"/> if the deserialization
        /// fails.</returns>
        public List<T>? GetObjects<T>()
            where T : class
        {
            Log.Information("RedisCacheService.GetObjects(): method called for type = {type}", typeof(T));
            var rKey = new RedisKey($"{typeof(T)}:all");
            var value = this.redis.StringGet(rKey);

            if (value == RedisValue.Null)
            {
                return null;
            }

            var returnValue = JsonSerializer.Deserialize<List<T>>(value.ToString());

            return returnValue;
        }

        /// <summary>
        /// Stores a list of objects in the Redis cache under a key derived from the type of the objects.
        /// </summary>
        /// <remarks>The objects are serialized to JSON before being stored in the cache. The cache key is
        /// generated using the fully qualified name of the type <typeparamref name="T"/> with the suffix
        /// ":all".</remarks>
        /// <typeparam name="T">The type of objects to store in the cache. Must be a reference type.</typeparam>
        /// <param name="objects">The list of objects to store in the cache. Cannot be null.</param>
        public void SetObjects<T>(List<T> objects)
            where T : class
        {
            Log.Information("RedisCacheService.SetObjects(): method called for type = {type}", typeof(T));

            var rKey = new RedisKey($"{typeof(T)}:all");
            var value = new RedisValue(this.JsonSerialize(objects));

            this.redis.StringSet(rKey, value);
        }

        /// <summary>
        /// This is for get objects.
        /// </summary>
        /// <typeparam name="T">Type of desirable object.</typeparam>
        /// <param name="id">Id of desirable object.</param>
        /// <returns>Desirable object of type T.</returns>
        public T? GetObject<T>(int id)
            where T : class
        {
            Log.Information(
                "RedisCacheService.GetObject(): method called with parameters: id = {id} and for type = {type}",
                id,
                typeof(T));

            var rKey = new RedisKey($"{typeof(T)}:{id}");
            var value = this.redis.StringGet(rKey);

            if (value == RedisValue.Null)
            {
                return null;
            }

            var returnValue = JsonSerializer.Deserialize<T>(value.ToString());

            return returnValue;
        }

        /// <summary>
        /// This is for refresh objects.
        /// </summary>
        /// <typeparam name="T">Refreshed object type.</typeparam>
        /// <param name="obj">Refreshed object.</param>
        public void SetObject<T>(T obj)
            where T : class
        {
            Log.Information(
                "RedisCacheService.RefreshObject(): method called with parameters: obj = {obj} and for type = {type}",
                obj,
                typeof(T));

            var redisKey = new RedisKey($"{typeof(T)}:{(obj as BaseModel)?.Id}");
            var redisValue = new RedisValue(this.JsonSerialize(obj));

            if (redisValue != RedisValue.Null)
            {
                this.redis.StringSet(redisKey, redisValue);
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
                "RedisCacheService.RemoveObject(): method called with parameters: id = {id} and for type = {type}",
                obj,
                typeof(T));

            var redisKey = new RedisKey($"{typeof(T)}:{(obj as BaseModel)?.Id}");

            this.redis.KeyDelete(redisKey);
        }

        /// <summary>
        /// Removes all cached objects of the specified type from the Redis cache.
        /// </summary>
        /// <remarks>This method deletes all cached entries associated with the specified type by
        /// constructing a Redis key in the format "<c>{Type}:all</c>". Use this method to clear cached data for a
        /// specific type.</remarks>
        /// <typeparam name="T">The type of objects to remove from the cache. Must be a reference type.</typeparam>
        public void RemoveObjects<T>()
            where T : class
        {
            Log.Information("RedisCacheService.RemoveObjects(): method called for type = {type}", typeof(T));

            var redisKey = new RedisKey($"{typeof(T)}:all");

            this.redis.KeyDelete(redisKey);
        }

        private string JsonSerialize<T>(T obj) =>
            JsonSerializer.Serialize(obj, this.options);
    }
}
