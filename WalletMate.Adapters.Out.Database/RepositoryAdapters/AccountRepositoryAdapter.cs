using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WalletMate.Adapters.Out.Database.Abstract;
using WalletMate.Adapters.Out.Database.Entities;
using WalletMate.Domain.DomainEntities;
using WalletMate.Domain.Ports.Out.Repositories;

namespace WalletMate.Adapters.Out.Database.RepositoryAdapters;

public class AccountRepositoryAdapter(IDataContext dataContext) : IAccountRepository
{
    public async Task<Account?> FirstOrDefaultAsync(
        Expression<Func<Account, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        var entities = await dataContext.Account.ToListAsync(cancellationToken);
        return entities.Select(ToDomain).AsQueryable().FirstOrDefault(predicate);
    }

    public async Task<List<Account>> ToListAsync(CancellationToken cancellationToken = default)
    {
        var entities = await dataContext.Account.ToListAsync(cancellationToken);
        return entities.Select(ToDomain).ToList();
    }

    public async Task AddAsync(Account account, CancellationToken cancellationToken = default)
    {
        var entity = ToEntity(account);
        await dataContext.Account.AddAsync(entity, cancellationToken);
    }

    public Task AddRangeAsync(IEnumerable<Account> accounts, CancellationToken cancellationToken = default)
    {
        var entities = accounts.Select(ToEntity);
        return dataContext.Account.AddRangeAsync(entities, cancellationToken);
    }

    public void Update(Account account)
    {
        var entity = ToEntity(account);
        dataContext.Account.Update(entity);
    }

    public void Remove(Account account)
    {
        var entity = new AccountEntity { Id = account.Id };
        dataContext.Account.Remove(entity);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dataContext.SaveChangesAsync(cancellationToken);
    }

    private static AccountEntity ToEntity(Account domain)
    {
        return new AccountEntity
        {
            Id = domain.Id,
            Name = domain.Name,
            Description = domain.Description,
            AccountNumber = domain.AccountNumber,
            Balance = domain.Balance,
            Currency = domain.Currency,
            UserId = domain.UserId
        };
    }

    private static Account ToDomain(AccountEntity entity)
    {
        return new Account
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            AccountNumber = entity.AccountNumber,
            Balance = entity.Balance,
            Currency = entity.Currency,
            UserId = entity.UserId,
            Transactions = []
        };
    }
}