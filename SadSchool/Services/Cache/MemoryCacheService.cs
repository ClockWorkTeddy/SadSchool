using Microsoft.Extensions.Caching.Memory;
using SadSchool.Controllers.Contracts;
using SadSchool.Models;

namespace SadSchool.Services.Cache
{
    public class MemoryCacheService : ICacheService
    {
        private readonly SadSchoolContext _context;
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheService(SadSchoolContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public List<T> GetObject<T>(int id) where T : class
        {
            var cacheKey = $"{typeof(T)}:{id}";

            if (!_memoryCache.TryGetValue(cacheKey, out object? cachedObject))
            {
                cachedObject = _context.Set<T>().Find(id);
                _memoryCache.Set(cacheKey, cachedObject);
            }

            return new List<T> { cachedObject as T };
        }

        public void RefreshObject<T>(T obj) where T : class
        {
            var cacheKey = $"{typeof(T)}:{(obj as BaseModel).Id}";

            if (_memoryCache.TryGetValue(cacheKey, out object? cachedObject))
                _memoryCache.Set(cacheKey, obj as T);
        }
    }
}
