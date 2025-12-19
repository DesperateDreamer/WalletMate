using WalletMate.Domain.DomainEntities;
using WalletMate.Domain.SortingStrategies.Abstract;
using WalletMate.Domain.SortingStrategies.Enums;

namespace WalletMate.Domain.SortingStrategies;

public class SortByCurrencyStrategy : ITransactionSortingStrategy
{
    public IEnumerable<Transaction> Sort(IEnumerable<Transaction> transactions)
    {
        return transactions.OrderBy(t => t.Currency);
    }

    public TransactionSortOption SortOption => TransactionSortOption.Currency;
}