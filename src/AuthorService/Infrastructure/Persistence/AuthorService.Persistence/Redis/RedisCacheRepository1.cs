using AuthorService.Application.Interfaces.Redis;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace AuthorService.Persistence.Redis
{
    public class RedisCacheRepository1 : IRedisCacheRepository1
    {
        private readonly IDistributedCache _distributedCache1;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        public RedisCacheRepository1(IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer)
        {
            _distributedCache1 = distributedCache;
            _connectionMultiplexer = connectionMultiplexer;
        }
        public async Task SetStringResponseAsync(string cacheKey1, string dataToCache1, TimeSpan expireTimeSeconds1) //Burada string olarak iletilen datalar cachelenecektir.
        {
            if (string.IsNullOrEmpty(cacheKey1)) return;
            await _distributedCache1.SetStringAsync(cacheKey1, dataToCache1, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expireTimeSeconds1
            });
        }
        public async Task SetResponseAsync(string cacheKey1, object dataToCache1, TimeSpan expireTimeSeconds1) //Burada herhangi bir typeta iletilen datalar object içerisine boxlanarak cachelenecektir.
        {
            if (string.IsNullOrEmpty(cacheKey1)) return;
            string serializedResponse1 = JsonConvert.SerializeObject(dataToCache1, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
            await _distributedCache1.SetStringAsync(cacheKey1, serializedResponse1, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expireTimeSeconds1
            });
        }
        public async Task<string> GetCachedStringResponseByKeyAsync(string cacheKey1) //Burada cachelenen responseları çeker ve string olarak döndürür.
        {
            string cachedResponse1 = await _distributedCache1.GetStringAsync(cacheKey1);
            return string.IsNullOrEmpty(cachedResponse1) ? string.Empty : cachedResponse1;
        }
        public async Task<T?> GetCachedResponseByKeyAsync<T>(string cacheKey1) //Burada cachelenen responseları çeker ve Generic olarak döndürür.
        {
            string? cachedResponse1 = await _distributedCache1.GetStringAsync(cacheKey1);
            if (string.IsNullOrEmpty(cachedResponse1)) return default;
            T? deserializedObject1 = JsonConvert.DeserializeObject<T>(cachedResponse1);
            return deserializedObject1;
        }
        public async Task RemoveWithMultipleWildCardsAsync(string[] cacheKeys1)
        {
            foreach (var currentCacheKey1 in cacheKeys1)
            {
                await RemoveWithSingleWildCardAsync(currentCacheKey1);
            }
        }
        public async Task RemoveWithSingleWildCardAsync(string cacheKey1)
        {
            var exceptionKeys1 = new List<string>() { }; //Burada silinmesini ISTEMEDIGIMIZ keyleri belirtiriz.
            if (string.IsNullOrWhiteSpace(cacheKey1)) throw new ArgumentException($"Value cannot be null or whitespace: {cacheKey1}");
            List<string> allChildKeysFromRedisDatabase1 = await GetAllExistedChildKeys(cacheKey1);
            foreach (var currentKey1 in allChildKeysFromRedisDatabase1)
            {
                if (!exceptionKeys1.Any(e => currentKey1.StartsWith(e)))
                {
                    Console.WriteLine($"{currentKey1}=> key has been cleaned from Redis");
                    await _distributedCache1.RemoveAsync(currentKey1);
                }
            }
        }
        private async Task<List<string>> GetAllExistedChildKeys(string cacheKey1) //Verilen keyi ve bu keyin altındaki bütün child keyleri bulur ve döndürür.
        {
            var allChildKeysFromRedisDatabase1 = new List<string>();
            foreach (var currentEndpoint1 in _connectionMultiplexer.GetEndPoints())
            {
                var server1 = _connectionMultiplexer.GetServer(currentEndpoint1);
                await foreach (var currentKey1 in server1.KeysAsync(pattern: cacheKey1 + "*"))
                {
                    allChildKeysFromRedisDatabase1.Add(currentKey1.ToString());
                }
            }
            return allChildKeysFromRedisDatabase1;
        }
    }
}
