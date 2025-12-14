using WalletMate.BLL.Shared.DTOs;
using WalletMate.BLL.Shared.Enums;

namespace WalletMate.BLL.Shared.Abstract;

public interface ITransactionSortingStrategy
{
    IEnumerable<TransactionDto> Sort(IEnumerable<TransactionDto> transactions);
    
    TransactionSortOption SortOption { get; }
}