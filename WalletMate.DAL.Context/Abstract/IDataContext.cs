using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WalletMate.DAL.Entities;

namespace WalletMate.DAL.Context.Abstract;

public interface IDataContext
{
    DbSet<Account> Account { get; set; }
    DbSet<Category> Category { get; set; }
    DbSet<Transaction> Transaction { get; set; }
    DbSet<User> User { get; set; }
    DbSet<TransactionCategory> TransactionCategory { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    
    DatabaseFacade Database { get; }
}