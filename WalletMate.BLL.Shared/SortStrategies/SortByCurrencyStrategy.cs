using WalletMate.BLL.Shared.Abstract;
using WalletMate.BLL.Shared.DTOs;
using WalletMate.BLL.Shared.Enums;

namespace WalletMate.BLL.Shared.SortStrategies;

public class SortByCurrencyStrategy : ITransactionSortingStrategy
{
    public IEnumerable<TransactionDto> Sort(IEnumerable<TransactionDto> transactions)
    {
        return transactions.OrderBy(t => t.Currency);
    }

    public TransactionSortOption SortOption => TransactionSortOption.Currency;
}