using System.Collections.Concurrent;

namespace WalletMate.BLL.Shared;

/// <summary>
/// A thread-safe, generic cache manager that supports expiration of cached items. THREAD-SAFE SINGLETON PATTERN
/// </summary>
/// <typeparam name="TValue">The type of the values to be stored in the cache.</typeparam>
public sealed class CacheManager<TValue>
{
    /// <summary>
    /// Private constructor to enforce a singleton pattern.
    /// </summary>
    private CacheManager() { }
    
    private static readonly Lazy<CacheManager<TValue>> _instance =
        new(() => new CacheManager<TValue>());

    /// <summary>
    /// The singleton instance of the cache manager.
    /// </summary>
    public static CacheManager<TValue> Instance => _instance.Value;

    private readonly ConcurrentDictionary<string, CacheEntry<TValue>> _cache = new();
    
    /// <summary>
    /// Retrieves a value from the cache or adds it if it does not exist.
    /// </summary>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="factory">A factory function to create the value if it does not exist in the cache.</param>
    /// <param name="ttl">The time-to-live for the cache entry. If null, the entry does not expire.</param>
    /// <returns>The cached or newly created value.</returns>
    public TValue? GetOrAdd(string key, Func<TValue> factory, TimeSpan? ttl = null)
    {
        if (TryGetValidEntry(key, out var value))
            return value;

        var created = factory();
        Set(key, created, ttl);
        return created;
    }

    /// <summary>
    /// Asynchronously retrieves a value from the cache or adds it if it does not exist.
    /// </summary>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="factory">An asynchronous factory function to create the value if it does not exist in the cache.</param>
    /// <param name="ttl">The time-to-live for the cache entry. If null, the entry does not expire.</param>
    /// <returns>A task representing the cached or newly created value.</returns>
    public async Task<TValue?> GetOrAddAsync(string key, Func<Task<TValue>> factory, TimeSpan? ttl = null)
    {
        if (TryGetValidEntry(key, out var value))
            return value;

        var created = await factory();
        Set(key, created, ttl);
        return created;
    }

    /// <summary>
    /// Adds or updates a value in the cache with an optional expiration time.
    /// </summary>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="value">The value to store in the cache.</param>
    /// <param name="ttl">The time-to-live for the cache entry. If null, the entry does not expire.</param>
    private void Set(string key, TValue? value, TimeSpan? ttl = null)
    {
        var expiresAt = ttl.HasValue ? DateTime.UtcNow.Add(ttl.Value) : (DateTime?)null;
        _cache[key] = new CacheEntry<TValue>(value, expiresAt);
    }
    
    /// <summary>
    /// Removes a cache entry by its key.
    /// </summary>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <returns>True if the entry was removed; otherwise, false.</returns>
    public bool Remove(string key)
    {
        return _cache.TryRemove(key, out _);
    }

    /// <summary>
    /// Retrieves all valid cache entries.
    /// </summary>
    /// <returns>An enumerable of key-value pairs representing valid cache entries.</returns>
    public IEnumerable<KeyValuePair<string, TValue?>> GetAll()
    {
        var now = DateTime.UtcNow;

        foreach (var kvp in _cache)
        {
            if (kvp.Value.ExpiresAt == null || now < kvp.Value.ExpiresAt)
                yield return new KeyValuePair<string, TValue?>(kvp.Key, kvp.Value.Value);
        }
    }

    public void Clear()
    {
        _cache.Clear();
    }
    
    /// <summary>
    /// Attempts to retrieve a valid cache entry by its key.
    /// </summary>
    /// <param name="key">The unique identifier for the cache entry.</param>
    /// <param name="value">The retrieved value if found and valid; otherwise, null.</param>
    /// <returns>True if the value was found and valid; otherwise, false.</returns>
    private bool TryGetValidEntry(string key, out TValue? value)
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
    
    /// <summary>
    /// Represents a cache entry with an optional expiration time.
    /// </summary>
    /// <typeparam name="T">The type of the value stored in the cache entry.</typeparam>
    private record CacheEntry<T>(T? Value, DateTime? ExpiresAt);
}