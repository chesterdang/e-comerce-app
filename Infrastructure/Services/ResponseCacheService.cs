using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Services
{
    public class ResponseCacheService(IConnectionMultiplexer redis) : IResponseCachesService
    {
        private readonly IDatabase database = redis.GetDatabase(1);
        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
        {
            var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
            var serializedResponse = JsonSerializer.Serialize(response, options);
            await database.StringSetAsync(cacheKey, serializedResponse, timeToLive);
        }

        public async Task<string?> GetCachedResponseAsync(string cacheKey)
        {
            var cachedResponse = await database.StringGetAsync(cacheKey);
            if (cachedResponse.IsNullOrEmpty) return null;
            return cachedResponse;
        }

        public async Task RemoveCacheByPattern(string pattern)
        {
            var server = redis.GetServer(redis.GetEndPoints().First());
            var keys = server.Keys(database: 1, pattern: $"*{pattern}*").ToArray();
            if (keys.Length != 0) 
            {
                await database.KeyDeleteAsync(keys);
            }
        }
    }
}