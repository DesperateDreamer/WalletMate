using Microsoft.EntityFrameworkCore;
using WalletMate.Adapters.Out.Database;
using WalletMate.Adapters.Out.Database.Abstract;

namespace WalletMate.Adapters.In.API.Configurations;

public static class DatabaseConfiguration
{
    public static void ConfigureDbContext(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");

        builder.Services.AddDbContext<IDataContext, DataContext>((_, options) =>
        {
            options
                .UseNpgsql(connectionString, x => 
                    x.MigrationsAssembly("WalletMate.Adapters.Out.Database.Migrations"));
        });
    }
}