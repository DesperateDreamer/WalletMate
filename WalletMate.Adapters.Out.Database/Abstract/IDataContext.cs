using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WalletMate.Adapters.Out.Database.Entities;

namespace WalletMate.Adapters.Out.Database.Abstract;

public interface IDataContext
{
    DbSet<AccountEntity> Account { get; set; }
    DbSet<CategoryEntity> Category { get; set; }
    DbSet<TransactionEntity> Transaction { get; set; }
    DbSet<UserEntity> User { get; set; }
    DbSet<TransactionCategoryEntity> TransactionCategory { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    
    DatabaseFacade Database { get; }
}