using WalletMate.BLL.Shared.Abstract;
using WalletMate.BLL.Shared.DTOs;

namespace WalletMate.BLL.Shared.SortStrategies;

public class SortByCurrencyStrategy : ITransactionSortingStrategy
{
    public IEnumerable<TransactionDto> Sort(IEnumerable<TransactionDto> transactions)
    {
        return transactions.OrderBy(t => t.Currency);
    }
}