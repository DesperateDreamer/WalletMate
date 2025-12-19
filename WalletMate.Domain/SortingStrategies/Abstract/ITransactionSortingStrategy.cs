using WalletMate.Domain.DomainEntities;
using WalletMate.Domain.SortingStrategies.Enums;

namespace WalletMate.Domain.SortingStrategies.Abstract;

public interface ITransactionSortingStrategy
{
    IEnumerable<Transaction> Sort(IEnumerable<Transaction> transactions);
    
    TransactionSortOption SortOption { get; }
}