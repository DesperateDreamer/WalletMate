using WalletMate.Domain.BankModels;

namespace WalletMate.Domain.Ports.Out;

public interface IBankProviderPort
{
    Task<BankClientInfo?> GetClientInfoAsync(string token);
    Task<List<BankTransaction>?> GetTransactionsAsync(string token, string accountId, long from, long? to = null);
}