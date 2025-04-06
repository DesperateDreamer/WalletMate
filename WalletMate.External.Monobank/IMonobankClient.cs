using WalletMate.External.Monobank.Models;

namespace WalletMate.External.Monobank;

public interface IMonobankClient
{
    Task<ClientInfo?> GetClientInfoAsync(string token);
    Task<List<Transaction>?> GetTransactionsAsync(string token, string accountId, string from, string to);
}