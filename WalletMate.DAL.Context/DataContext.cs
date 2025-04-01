using Microsoft.EntityFrameworkCore;
using WalletMate.DAL.Context.Abstract;
using WalletMate.DAL.Entities;

namespace WalletMate.DAL.Context;

public class DataContext(DbContextOptions options) : DbContext(options), IDataContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<Account> Account { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<Transaction> Transaction { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<TransactionCategory> TransactionCategory { get; set; }
}