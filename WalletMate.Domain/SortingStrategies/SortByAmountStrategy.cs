using WalletMate.Domain.DomainEntities;
using WalletMate.Domain.SortingStrategies.Abstract;
using WalletMate.Domain.SortingStrategies.Enums;

namespace WalletMate.Domain.SortingStrategies;

public class SortByAmountStrategy : ITransactionSortingStrategy
{
    public IEnumerable<Transaction> Sort(IEnumerable<Transaction> transactions)
    {
        return transactions.OrderByDescending(t => t.Amount);
    }

    public TransactionSortOption SortOption => TransactionSortOption.Amount;
}