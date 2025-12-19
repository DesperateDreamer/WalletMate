using Microsoft.Extensions.DependencyInjection;
using WalletMate.Application.Adapters;
using WalletMate.Application.Common;
using WalletMate.Application.Ports.In;
using WalletMate.Domain.SortingStrategies;
using WalletMate.Domain.SortingStrategies.Abstract;

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
        services.AddScoped<ITransactionSortingStrategy, SortByDateStrategy>();
        services.AddScoped<ITransactionSortingStrategy, SortByAmountStrategy>();
        services.AddScoped<ITransactionSortingStrategy, SortByCurrencyStrategy>();
        services.AddScoped<TransactionSortingStrategyResolver>();

        return services;
    }
}