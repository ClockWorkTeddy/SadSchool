namespace SadSchool.Services
{
    using Newtonsoft.Json;
    using SadSchool.Controllers.Contracts;
    using SadSchool.Models;
    using StackExchange.Redis;

    /// <summary>
    /// Class for Redis caching.
    /// </summary>
    public class RedisCache : ICacheService
    {
        private readonly IDatabase redis;
        private readonly SadSchoolContext context;

        public RedisCache(IConnectionMultiplexer muxer, SadSchoolContext context) 
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
        public List<T> GetObject<T>(int id)
            where T : class
        {
            var rKey = new RedisKey($"{typeof(T)}:{id}");
            var value = this.redis.StringGet(rKey);

            if (value == RedisValue.Null)
            {
                T dbValue = this.context.Set<T>().Find(id);
                string serializedValue = JsonConvert.SerializeObject(dbValue);
                value = new RedisValue(serializedValue);

                this.redis.SetAdd(rKey, value);
            }

            var returnValue = JsonConvert.DeserializeObject<T>(value.ToString());

            return new List<T> { returnValue };
        }

        public void RefreshObject<T>(T obj) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
