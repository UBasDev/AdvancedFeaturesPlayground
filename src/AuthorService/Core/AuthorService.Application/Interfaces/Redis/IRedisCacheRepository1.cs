namespace AuthorService.Application.Interfaces.Redis
{
    public interface IRedisCacheRepository1
    {
        public Task SetStringResponseAsync(string cacheKey1, string dataToCache1, TimeSpan expireTimeSeconds1);
        public Task SetResponseAsync(string cacheKey1, object dataToCache1, TimeSpan expireTimeSeconds1);
        public Task<string> GetCachedStringResponseByKeyAsync(string cacheKey1);
        public Task<T?> GetCachedResponseByKeyAsync<T>(string cacheKey1);
        public Task RemoveWithMultipleWildCardsAsync(string[] cacheKeys1);
        public Task RemoveWithSingleWildCardAsync(string cacheKey1);

    }
}
