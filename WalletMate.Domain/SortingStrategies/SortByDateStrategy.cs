using WalletMate.Domain.DomainEntities;
using WalletMate.Domain.SortingStrategies.Abstract;
using WalletMate.Domain.SortingStrategies.Enums;

namespace WalletMate.Domain.SortingStrategies;

public class SortByDateStrategy : ITransactionSortingStrategy
{
    public IEnumerable<Transaction> Sort(IEnumerable<Transaction> transactions)
    {
        return transactions.OrderByDescending(t => t.CreatedOn);
    }

    public TransactionSortOption SortOption => TransactionSortOption.Date;
}