namespace WalletMate.Application.Ports.Out;

public interface ICacheManagerPort
{
    Task<T?> GetAsync<T>(string key);

    Task<T?> GetOrAddAsync<T>(
        string key,
        Func<Task<T>> factory,
        TimeSpan? ttl = null);

    Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? ttl = null);

    Task RemoveAsync(string key);

    Task ClearAsync();
}