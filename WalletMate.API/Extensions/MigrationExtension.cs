using Microsoft.EntityFrameworkCore;
using WalletMate.DAL.Context.Abstract;

namespace WalletMate.API.Extensions;

public static class MigrationExtension
{
    public static async Task RunMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IDataContext>();
        await db.Database.MigrateAsync();
    }
}