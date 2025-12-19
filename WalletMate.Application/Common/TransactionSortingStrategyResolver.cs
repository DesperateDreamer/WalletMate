using WalletMate.Domain.SortingStrategies.Abstract;
using WalletMate.Domain.SortingStrategies.Enums;

namespace WalletMate.Application.Common;

// Strategy pattern
public class TransactionSortingStrategyResolver(IEnumerable<ITransactionSortingStrategy?> strategies)
{
    public ITransactionSortingStrategy Resolve(TransactionSortOption option)
    {
        return strategies.FirstOrDefault(strategy => strategy?.SortOption == option) 
               ?? throw new ArgumentException($"{nameof(TransactionSortOption)} must be one of: {string.Join(", ", strategies)}");
    }
}