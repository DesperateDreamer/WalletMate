using WalletMate.Adapters.Out.Monobank.Models;

namespace WalletMate.Adapters.Out.Monobank.Client;

public interface IMonobankClient
{
    Task<MonobankClientInfo?> GetClientInfoAsync(string token);
    Task<List<MonobankTransaction>?> GetTransactionsAsync(string token, string accountId, long from, long? to = null);
}