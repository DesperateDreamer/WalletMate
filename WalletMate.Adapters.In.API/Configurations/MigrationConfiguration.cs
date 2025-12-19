using Microsoft.EntityFrameworkCore;
using WalletMate.Adapters.Out.Database.Abstract;

namespace WalletMate.Adapters.In.API.Configurations;

public static class MigrationConfiguration
{
    public static async Task RunMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IDataContext>();
        await db.Database.MigrateAsync();
    }
}