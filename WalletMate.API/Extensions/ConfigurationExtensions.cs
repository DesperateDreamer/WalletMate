using Microsoft.EntityFrameworkCore;
using WalletMate.DAL.Context;
using WalletMate.DAL.Context.Abstract;

namespace WalletMate.API.Extensions;

public static class ConfigurationExtensions
{
    public static void ConfigureInAppServices(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
        
        builder.Services.AddDbContext<IDataContext, DataContext>((provider, options) =>
        {
            options
                .UseNpgsql(connectionString, x => x.MigrationsAssembly("WalletMate.DAL.Migrations"));
        });
    }
}