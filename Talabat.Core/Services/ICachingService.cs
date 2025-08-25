using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Services
{
    public interface ICachingService
    {
        public Task SetAsync(string cachingKey, object objectToCache, TimeSpan cacheLifeTime);
        public Task<string> GetAsync(string cachingKey);
    }
}
