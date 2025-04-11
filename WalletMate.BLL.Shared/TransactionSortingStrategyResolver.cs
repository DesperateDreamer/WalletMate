using Microsoft.Extensions.DependencyInjection;
using WalletMate.BLL.Shared.Abstract;
using WalletMate.BLL.Shared.Enums;
using WalletMate.BLL.Shared.SortStrategies;

namespace WalletMate.BLL.Shared;

// Strategy pattern
public class TransactionSortingStrategyResolver(IServiceProvider provider)
{
    public ITransactionSortingStrategy Resolve(TransactionSortOption option) => option switch
    {
        TransactionSortOption.Date => provider.GetRequiredService<SortByDateStrategy>(),
        TransactionSortOption.Amount => provider.GetRequiredService<SortByAmountStrategy>(),
        TransactionSortOption.Currency => provider.GetRequiredService<SortByCurrencyStrategy>(),
        _ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
    };
}