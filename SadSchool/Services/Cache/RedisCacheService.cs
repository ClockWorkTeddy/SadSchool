﻿// <copyright file="RedisCacheService.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Services.Cache
{
    using Newtonsoft.Json;
    using SadSchool.Controllers.Contracts;
    using SadSchool.Models;
    using StackExchange.Redis;

    /// <summary>
    /// Class for Redis caching.
    /// </summary>
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase redis;
        private readonly SadSchoolContext context;

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
        public List<T?> GetObject<T>(int id)
            where T : class
        {
            var rKey = new RedisKey($"{typeof(T)}:{id}");
            var value = this.redis.StringGet(rKey);

            if (value == RedisValue.Null)
            {
                T? dbValue = this.context.Set<T>().Find(id);
                string serializedValue = JsonConvert.SerializeObject(dbValue);
                value = new RedisValue(serializedValue);

                this.redis.StringSet(rKey, value);
            }

            var returnValue = JsonConvert.DeserializeObject<T>(value.ToString());

            return new List<T?> { returnValue };
        }

        /// <summary>
        /// This is for refresh objects.
        /// </summary>
        /// <typeparam name="T">Refreshed object type.</typeparam>
        /// <param name="obj">Refreshed object.</param>
        public void RefreshObject<T>(T obj)
            where T : class
        {
            var redisKey = new RedisKey($"{typeof(T)}:{(obj as BaseModel)?.Id}");
            var redisValue = this.redis.StringGet(redisKey);

            if (redisValue != RedisValue.Null)
            {
                this.redis.StringSet(redisKey, redisValue);
            }
        }
    }
}
