using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Services;

namespace Talabat.Services
{
    public class CachingService : ICachingService
    {
        private readonly StackExchange.Redis.IDatabase inMemoryDB;

        public CachingService(IConnectionMultiplexer connectionMultiplexer)
        {
            inMemoryDB = connectionMultiplexer.GetDatabase();
        }

        public async Task<string> GetAsync(string cachingKey)
        {
            var response = inMemoryDB.StringGet(cachingKey);
            return response.IsNullOrEmpty ? null : response.ToString();
        }

        public Task SetAsync(string cachingKey, object objectToCache, TimeSpan cacheLifeTime)
            => inMemoryDB.StringSetAsync(cachingKey, JsonSerializer.Serialize(objectToCache), expiry: cacheLifeTime);
    }
}
