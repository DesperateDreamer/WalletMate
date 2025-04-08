using WalletMate.BLL.Shared.Abstract;
using WalletMate.BLL.Shared.DTOs;

namespace WalletMate.BLL.Shared.SortStrategies;

public class SortByAmountStrategy : ITransactionSortingStrategy
{
    public IEnumerable<TransactionDto> Sort(IEnumerable<TransactionDto> transactions)
    {
        return transactions.OrderByDescending(t => t.Amount);
    }
}