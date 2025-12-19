using WalletMate.Application.Models.Transaction;
using TransactionSortOption = WalletMate.Application.Models.Transaction.TransactionSortOption;

namespace WalletMate.Application.Ports.In;

public interface ITransactionService
{
    Task<TransactionDto> GetTransactionByIdAsync(Guid transactionId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync(CancellationToken cancellationToken = default);
        
    Task<Guid> CreateTransactionAsync(CreateTransactionDto dto, CancellationToken cancellationToken = default);
    Task<Guid> UpdateTransactionAsync(Guid transactionId, UpdateTransactionDto dto, CancellationToken cancellationToken = default);
    Task DeleteTransactionAsync(Guid transactionId, CancellationToken cancellationToken = default);

    Task<IEnumerable<TransactionDto>> GetTransactionsByAccountAsync(Guid accountId, TransactionSortOption sortBy, CancellationToken cancellationToken = default);
}