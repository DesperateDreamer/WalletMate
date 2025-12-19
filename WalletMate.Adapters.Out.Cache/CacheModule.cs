using Microsoft.Extensions.DependencyInjection;
using WalletMate.Application.Ports.Out;

namespace WalletMate.Adapters.Out.Cache;

public static class CacheModule
{
    public static IServiceCollection ConfigureCacheAdapters(this IServiceCollection services)
    {
        services.AddScoped<ICacheManagerPort, CacheManagerAdapter>();
        
        return services;
    }
}