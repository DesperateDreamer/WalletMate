using Microsoft.Extensions.DependencyInjection;
using WalletMate.Application.Adapters;
using WalletMate.Application.Ports.In;

namespace WalletMate.Application;

public static class ApplicationModule
{
    public static IServiceCollection ConfigureApplicationAdapters(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}