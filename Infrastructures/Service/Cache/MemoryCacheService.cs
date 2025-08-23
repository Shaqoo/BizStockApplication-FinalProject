using Application.Interfaces.Service;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace Infrastructures.Service.Cache
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly IMemoryCache _cache;
        private readonly static ConcurrentDictionary<string, SemaphoreSlim> _locks = new();
        private readonly HashSet<string> _keys = new HashSet<string>();

        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        private SemaphoreSlim GetLock(string key) =>
    _locks.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));

        public Task<T?> GetAsync<T>(string key)
        {
            if (_cache.TryGetValue(key, out T? value))
                return Task.FromResult(value);

            return Task.FromResult(default(T));
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(30)
            };

            _cache.Set(key, value, options);
            _keys.Add(key);
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key)
        {
            _cache.Remove(key);
            _keys.Remove(key);
            return Task.CompletedTask;
        }

        public async Task<T?> GetOrAddAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
        {
             
            if (_cache.TryGetValue(key, out T? existing))
                return existing;

            var keyLock = GetLock(key);
            await keyLock.WaitAsync();
            try
            {
                if (_cache.TryGetValue(key, out existing))
                    return existing;

                var data = await factory();
                if (data is not null)
                {
                    var options = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(30)
                    };
                    _cache.Set(key, data, options);
                    _keys.Add(key);
                }
                return data;
            }
            finally
            {
                keyLock.Release();
            }
        }
        public Task RemoveByPrefix(string prefix)
        {
            var keysToRemove = _keys.Where(k => k.StartsWith(prefix)).ToList();
            foreach (var key in keysToRemove)
            {
                _cache.Remove(key);
                _keys.Remove(key);
            }
            return Task.CompletedTask;
        }

        public Task RemoveCacheByPrefix(params string[] prefixes)
        {
            lock (_keys)
            {
                var keysToRemove = _keys
                    .Where(k => prefixes.Any(p => k.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
                    .ToList();

                foreach (var key in keysToRemove)
                {
                    _cache.Remove(key);
                    _keys.Remove(key);
                }
            }
            return Task.CompletedTask;
        }

    }


}
