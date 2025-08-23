namespace Application.Interfaces.Service
{
    public interface IMemoryCacheService
    {
        Task<T?> GetOrAddAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null);
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
        Task RemoveAsync(string key);
        Task RemoveByPrefix(string prefix);
        Task RemoveCacheByPrefix(params string[] prefixes);
    }

}
