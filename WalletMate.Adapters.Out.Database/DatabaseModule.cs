using Microsoft.Extensions.DependencyInjection;
using WalletMate.Adapters.Out.Database.Abstract;
using WalletMate.Adapters.Out.Database.RepositoryAdapters;
using WalletMate.Domain.Ports.Out.Repositories;

namespace WalletMate.Adapters.Out.Database;

public static class DatabaseModule
{
    public static IServiceCollection ConfigureDatabaseAdapters(this IServiceCollection services)
    {
        services.AddScoped<IAccountRepository, AccountRepositoryAdapter>();
        services.AddScoped<ICategoryRepository, CategoryRepositoryAdapter>();
        services.AddScoped<ITransactionRepository, TransactionRepositoryAdapter>();
        services.AddScoped<IUserRepository, UserRepositoryAdapter>();

        return services;
    }
}