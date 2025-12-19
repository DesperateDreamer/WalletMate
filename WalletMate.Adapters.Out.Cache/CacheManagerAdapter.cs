using System.Collections.Concurrent;
using WalletMate.Application.Ports.Out;

namespace WalletMate.Adapters.Out.Cache;

public class CacheManagerAdapter : ICacheManagerPort
{
    private static readonly ConcurrentDictionary<Type, ICacheManagerFacade> _managersByType = new();

    public Task<T?> GetAsync<T>(string key)
    {
        var manager = GetManager<T>();
        
        foreach (var kv in manager.GetAll())
        {
            if (StringComparer.Ordinal.Equals(kv.Key, key))
                return Task.FromResult(kv.Value);
        }

        return Task.FromResult<T?>(default);
    }

    public Task<T?> GetOrAddAsync<T>(string key, Func<Task<T>> factory, TimeSpan? ttl = null)
    {
        var manager = GetManager<T>();
        return manager.GetOrAddAsync(key, factory, ttl);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? ttl = null)
    {
        var manager = GetManager<T>();
        
        manager.Remove(key);
        await manager.GetOrAddAsync(key, () => Task.FromResult(value), ttl);
    }

    public Task RemoveAsync(string key)
    {
        foreach (var facade in _managersByType.Values)
            facade.Remove(key);

        return Task.CompletedTask;
    }

    public Task ClearAsync()
    {
        foreach (var facade in _managersByType.Values)
            facade.Clear();

        return Task.CompletedTask;
    }

    private static CacheManager<T> GetManager<T>()
    {
        _managersByType.TryAdd(typeof(T), new CacheManagerFacade<T>());
        return CacheManager<T>.Instance;
    }

    private interface ICacheManagerFacade
    {
        void Remove(string key);
        void Clear();
    }

    private sealed class CacheManagerFacade<T> : ICacheManagerFacade
    {
        private static CacheManager<T> Manager => CacheManager<T>.Instance;

        public void Remove(string key) => Manager.Remove(key);

        public void Clear() => Manager.Clear();
    }
}