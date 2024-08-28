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
        private readonly SadSchoolContext context;
        private readonly JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCacheService"/> class.
        /// </summary>
        /// <param name="muxer">Redis muxer object.</param>
        /// <param name="context">DB context.</param>
        public RedisCacheService(IConnectionMultiplexer muxer, SadSchoolContext context)
        {
            this.redis = muxer.GetDatabase();
            this.context = context;
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
                T? dbValue = this.context.Set<T>().Find(id);

                if (dbValue != null)
                {
                    value = new RedisValue(this.JsonSerialize(dbValue));
                    this.redis.StringSet(rKey, value);
                }
                else
                {
                    return null;
                }
            }

            var returnValue = JsonSerializer.Deserialize<T>(value.ToString());

            return returnValue;
        }

        /// <summary>
        /// This is for refresh objects.
        /// </summary>
        /// <typeparam name="T">Refreshed object type.</typeparam>
        /// <param name="obj">Refreshed object.</param>
        public void RefreshObject<T>(T obj)
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

        private string JsonSerialize<T>(T obj) =>
            JsonSerializer.Serialize(obj, this.options);
    }
}
