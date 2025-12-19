using Microsoft.EntityFrameworkCore;
using WalletMate.Adapters.Out.Database.Abstract;
using WalletMate.Adapters.Out.Database.Entities;

namespace WalletMate.Adapters.Out.Database;

public class DataContext(DbContextOptions options) : DbContext(options), IDataContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<AccountEntity> Account { get; set; }
    public DbSet<CategoryEntity> Category { get; set; }
    public DbSet<TransactionEntity> Transaction { get; set; }
    public DbSet<UserEntity> User { get; set; }
    public DbSet<TransactionCategoryEntity> TransactionCategory { get; set; }
}