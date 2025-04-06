using System.Collections.Concurrent;

namespace WalletMate.BLL.Shared;

public sealed class CacheManager<TValue>
{
    private CacheManager()
    {
    }
    
    private static readonly Lazy<CacheManager<TValue>> _instance =
        new(() => new CacheManager<TValue>());
    
    public static CacheManager<TValue> Instance => _instance.Value;
    
    private readonly ConcurrentDictionary<Guid, TValue?> _cache = new();
    
    public TValue? GetOrAdd(Guid key, Func<TValue> factory)
    {
        return _cache.GetOrAdd(key, _ => factory());
    }

    public bool TryGetValue(Guid key, out TValue? value)
    {
        return _cache.TryGetValue(key, out value);
    }

    public void Set(Guid key, TValue? value)
    {
        _cache[key] = value;
    }

    public bool Remove(Guid key)
    {
        return _cache.TryRemove(key, out _);
    }

    public IEnumerable<KeyValuePair<Guid, TValue?>> GetAll()
    {
        return _cache.ToArray();
    }

    public bool ContainsKey(Guid key)
    {
        return _cache.ContainsKey(key);
    }
    
    public void Clear()
    {
        _cache.Clear();
    }
}