using System.Collections.Concurrent;

namespace WalletMate.BLL.Shared;

public sealed class CacheManager<TKey, TValue> where TKey : notnull
{
    private CacheManager()
    {
    }
    
    private static readonly Lazy<CacheManager<TKey, TValue>> _instance =
        new(() => new CacheManager<TKey, TValue>());
    
    public static CacheManager<TKey, TValue> Instance => _instance.Value;
    
    private readonly ConcurrentDictionary<TKey, TValue?> _cache = new();
    
    public TValue? GetOrAdd(TKey key, Func<TValue> factory)
    {
        return _cache.GetOrAdd(key, _ => factory());
    }

    public bool TryGetValue(TKey key, out TValue? value)
    {
        return _cache.TryGetValue(key, out value);
    }

    public void Set(TKey key, TValue? value)
    {
        _cache[key] = value;
    }

    public bool Remove(TKey key)
    {
        return _cache.TryRemove(key, out _);
    }

    public IEnumerable<KeyValuePair<TKey, TValue?>> GetAll()
    {
        return _cache.ToArray();
    }

    public bool ContainsKey(TKey key)
    {
        return _cache.ContainsKey(key);
    }
    
    public void Clear()
    {
        _cache.Clear();
    }
}