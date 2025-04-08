using System.Collections.Concurrent;

namespace WalletMate.BLL.Shared;

public sealed class CacheManager<TValue>
{
    private CacheManager() { }

    private static readonly Lazy<CacheManager<TValue>> _instance =
        new(() => new CacheManager<TValue>());

    public static CacheManager<TValue> Instance => _instance.Value;

    private readonly ConcurrentDictionary<Guid, CacheEntry<TValue>> _cache = new();

    public TValue? GetOrAdd(Guid key, Func<TValue> factory, TimeSpan? ttl = null)
    {
        if (TryGetValidEntry(key, out var value))
            return value;

        var created = factory();
        Set(key, created, ttl);
        return created;
    }

    public async Task<TValue?> GetOrAddAsync(Guid key, Func<Task<TValue>> factory, TimeSpan? ttl = null)
    {
        if (TryGetValidEntry(key, out var value))
            return value;

        var created = await factory();
        Set(key, created, ttl);
        return created;
    }

    public bool TryGetValue(Guid key, out TValue? value)
    {
        return TryGetValidEntry(key, out value);
    }

    public bool ContainsKey(Guid key)
    {
        return TryGetValidEntry(key, out _);
    }

    public void Set(Guid key, TValue? value, TimeSpan? ttl = null)
    {
        var expiresAt = ttl.HasValue ? DateTime.UtcNow.Add(ttl.Value) : (DateTime?)null;
        _cache[key] = new CacheEntry<TValue>(value, expiresAt);
    }

    public bool Remove(Guid key)
    {
        return _cache.TryRemove(key, out _);
    }

    public IEnumerable<KeyValuePair<Guid, TValue?>> GetAll()
    {
        var now = DateTime.UtcNow;

        foreach (var kvp in _cache)
        {
            if (kvp.Value.ExpiresAt == null || now < kvp.Value.ExpiresAt)
                yield return new KeyValuePair<Guid, TValue?>(kvp.Key, kvp.Value.Value);
        }
    }

    public void Clear()
    {
        _cache.Clear();
    }
    
    private bool TryGetValidEntry(Guid key, out TValue? value)
    {
        var now = DateTime.UtcNow;

        if (_cache.TryGetValue(key, out var entry))
        {
            if (entry.ExpiresAt == null || now < entry.ExpiresAt)
            {
                value = entry.Value;
                return true;
            }

            // expired
            _cache.TryRemove(key, out _);
        }

        value = default;
        return false;
    }
    
    private record CacheEntry<T>(T? Value, DateTime? ExpiresAt);
}