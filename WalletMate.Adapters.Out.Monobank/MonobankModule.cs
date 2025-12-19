using Microsoft.Extensions.DependencyInjection;
using WalletMate.Adapters.Out.Monobank.Client;
using WalletMate.Domain.Ports.Out;

namespace WalletMate.Adapters.Out.Monobank;

public static class MonobankModule
{
    public static IServiceCollection ConfigureMonobankAdapters(this IServiceCollection services)
    {
        services.AddHttpClient<IMonobankClient, MonobankClient>();

        services.AddScoped<IBankProviderPort, MonobankAdapter>();

        return services;
    }
}