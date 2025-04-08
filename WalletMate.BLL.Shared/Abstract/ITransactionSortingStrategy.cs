using WalletMate.BLL.Shared.DTOs;

namespace WalletMate.BLL.Shared.Abstract;

public interface ITransactionSortingStrategy
{
    IEnumerable<TransactionDto> Sort(IEnumerable<TransactionDto> transactions);
}