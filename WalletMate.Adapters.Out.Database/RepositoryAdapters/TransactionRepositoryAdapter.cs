using Microsoft.EntityFrameworkCore;
using WalletMate.Adapters.Out.Database.Abstract;
using WalletMate.Adapters.Out.Database.Entities;
using WalletMate.Domain.DomainEntities;
using WalletMate.Domain.Ports.Out.Repositories;

namespace WalletMate.Adapters.Out.Database.RepositoryAdapters;

public class TransactionRepositoryAdapter(IDataContext dataContext) : ITransactionRepository
{
    public async Task<Transaction?> GetByIdAsync(Guid transactionId, CancellationToken cancellationToken = default)
    {
        var entity = await dataContext.Transaction
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == transactionId, cancellationToken);

        return entity is null ? null : ToDomain(entity);
    }

    public async Task<List<Transaction>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await dataContext.Transaction
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return entities.Select(ToDomain).ToList();
    }

    public async Task<List<Transaction>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        var entities = await dataContext.Transaction
            .AsNoTracking()
            .Where(t => t.AccountId == accountId)
            .ToListAsync(cancellationToken);

        return entities.Select(ToDomain).ToList();
    }

    public async Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default)
    {
        var entity = ToEntity(transaction);
        await dataContext.Transaction.AddAsync(entity, cancellationToken);
    }

    public void Update(Transaction transaction)
    {
        var entity = ToEntity(transaction);
        dataContext.Transaction.Update(entity);
    }

    public void Remove(Transaction transaction)
    {
        var entity = new TransactionEntity { Id = transaction.Id };
        dataContext.Transaction.Remove(entity);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dataContext.SaveChangesAsync(cancellationToken);
    }

    private static TransactionEntity ToEntity(Transaction domain)
    {
        return new TransactionEntity
        {
            Id = domain.Id,
            Amount = domain.Amount,
            Description = domain.Description,
            Comment = domain.Comment,
            Currency = domain.Currency,
            CreatedOn = domain.CreatedOn,
            AccountId = domain.AccountId
        };
    }

    private static Transaction ToDomain(TransactionEntity entity)
    {
        return new Transaction
        {
            Id = entity.Id,
            Amount = entity.Amount,
            Description = entity.Description,
            Comment = entity.Comment,
            Currency = entity.Currency,
            CreatedOn = entity.CreatedOn,
            AccountId = entity.AccountId
        };
    }
}