using WalletMate.Domain.DomainEntities;

namespace WalletMate.Domain.Ports.Out.Repositories;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid transactionId, CancellationToken cancellationToken = default);
    Task<List<Transaction>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<Transaction>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default);
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default);
    void Update(Transaction transaction);
    void Remove(Transaction transaction);
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}