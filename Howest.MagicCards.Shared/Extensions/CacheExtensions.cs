using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Howest.MagicCards.Shared.Extensions
{
    public static class CacheExtensions
    {
        public static async Task<T> GetCachedDataAsync<T>(this IDistributedCache cache, string cacheKey)
        {
            string cachedData = await cache.GetStringAsync(cacheKey);
            if (string.IsNullOrEmpty(cachedData))
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(cachedData);
        }

        public static async Task SetCacheDataAsync<T>(this IDistributedCache cache, string cacheKey, T data, TimeSpan expiration)
        {
            var serializedData = JsonConvert.SerializeObject(data);
            await cache.SetStringAsync(cacheKey, serializedData, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            });
        }
    }
}
