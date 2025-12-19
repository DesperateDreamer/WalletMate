using Microsoft.Extensions.DependencyInjection;
using WalletMate.Adapters.Out.Shared.Http.Abstract;

namespace WalletMate.Adapters.Out.Shared.Http;

public static class HttpSharedModule
{
    public static IServiceCollection ConfigureSharedHttpAdapters(this IServiceCollection services)
    {
        services.AddHttpClient();

        services.AddScoped<IHttpRequestBuilderFactory, HttpRequestBuilderFactory>();
        services.AddScoped<IHttpRequestBuilder, HttpRequestBuilder>();

        return services;
    }
}