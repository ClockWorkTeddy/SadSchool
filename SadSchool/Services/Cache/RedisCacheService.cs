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

        public RedisCacheService(IConnectionMultiplexer muxer, SadSchoolContext context)
        {
            redis = muxer.GetDatabase();
            this.context = context;
        }

        /// <summary>
        /// This is for get objects.
        /// </summary>
        /// <typeparam name="T">Type of desirable object.</typeparam>
        /// <param name="id">Id of desirable object.</param>
        /// <returns>Desirable object of type T.</returns>
        public List<T> GetObject<T>(int id)
            where T : class
        {
            var rKey = new RedisKey($"{typeof(T)}:{id}");
            var value = this.redis.StringGet(rKey);

            if (value == RedisValue.Null)
            {
                T dbValue = context.Set<T>().Find(id);
                string serializedValue = JsonConvert.SerializeObject(dbValue);
                value = new RedisValue(serializedValue);

                redis.StringSet(rKey, value);
            }

            var returnValue = JsonConvert.DeserializeObject<T>(value.ToString());

            return new List<T> { returnValue };
        }

        public void RefreshObject<T>(T obj) where T : class
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
