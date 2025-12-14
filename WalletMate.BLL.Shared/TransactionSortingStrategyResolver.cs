using WalletMate.BLL.Shared.Abstract;
using WalletMate.BLL.Shared.Enums;

namespace WalletMate.BLL.Shared;

// Strategy pattern
public class TransactionSortingStrategyResolver(IEnumerable<ITransactionSortingStrategy?> strategies)
{
    public ITransactionSortingStrategy Resolve(TransactionSortOption option)
    {
        return strategies.FirstOrDefault(strategy => strategy?.SortOption == option) 
               ?? throw new ArgumentException($"{nameof(TransactionSortOption)} must be one of: {string.Join(", ", strategies)}");
    }
}