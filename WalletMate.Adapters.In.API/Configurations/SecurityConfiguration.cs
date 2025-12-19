using WalletMate.Adapters.In.API.Auth;
using WalletMate.Application.Ports.Out;

namespace WalletMate.Adapters.In.API.Configurations;

public static class SecurityConfiguration
{
    public static IServiceCollection ConfigureSecurityAdapters(this IServiceCollection services)
    {
        services.AddScoped<IPasswordServicePort, PasswordService>();
        services.AddScoped<ITokenServicePort, TokenService>();

        return services;
    }
}