using WalletMate.BLL.Domain.DTOs;

namespace WalletMate.BLL.Domain.Abstract;

public interface ITransactionService
{
    Task<TransactionDto?> GetTransactionByIdAsync(Guid transactionId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync(CancellationToken cancellationToken = default);
        
    Task<Guid> CreateTransactionAsync(CreateTransactionDto dto, CancellationToken cancellationToken = default);
    Task<Guid> UpdateTransactionAsync(Guid transactionId, UpdateTransactionDto dto, CancellationToken cancellationToken = default);
    Task DeleteTransactionAsync(Guid transactionId, CancellationToken cancellationToken = default);

    Task<IEnumerable<TransactionDto>> GetTransactionsByAccountAsync(Guid accountId, CancellationToken cancellationToken = default);
}