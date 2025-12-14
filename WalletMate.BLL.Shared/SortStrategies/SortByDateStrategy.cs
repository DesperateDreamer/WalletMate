using WalletMate.BLL.Shared.Abstract;
using WalletMate.BLL.Shared.DTOs;
using WalletMate.BLL.Shared.Enums;

namespace WalletMate.BLL.Shared.SortStrategies;

public class SortByDateStrategy : ITransactionSortingStrategy
{
    public IEnumerable<TransactionDto> Sort(IEnumerable<TransactionDto> transactions)
    {
        return transactions.OrderByDescending(t => t.CreatedOn);
    }

    public TransactionSortOption SortOption => TransactionSortOption.Date;
}