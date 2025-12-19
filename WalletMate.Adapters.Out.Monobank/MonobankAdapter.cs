using WalletMate.Adapters.Out.Monobank.Client;
using WalletMate.Domain.BankModels;
using WalletMate.Domain.Ports.Out;

namespace WalletMate.Adapters.Out.Monobank;

public class MonobankAdapter(IMonobankClient client) : IBankProviderPort
{
    public async Task<BankClientInfo?> GetClientInfoAsync(string token)
    {
        var monobankClientInfo = await client.GetClientInfoAsync(token);

        return new BankClientInfo
        {
            Name = monobankClientInfo?.Name ?? string.Empty,
            ClientId = monobankClientInfo?.ClientId ?? string.Empty,
            Accounts = monobankClientInfo?.Accounts.Select(m => new BankAccount
            {
                Balance = m.Balance,
                CurrencyCode = m.CurrencyCode,
                Id = m.Id,
                Iban = m.Iban,
                Type = m.Type
            }) ?? []
        };
    }

    public async Task<List<BankTransaction>?> GetTransactionsAsync(string token, string accountId, long from, long? to = null)
    {
        var monobankTransactions = await client.GetTransactionsAsync(token, accountId, from, to);

        var domainTransactions = monobankTransactions?.Select(mt => new BankTransaction
        {
            Amount = mt.Amount,
            Balance = mt.Balance,
            Id = mt.Id,
            Time = mt.Time,
            CashbackAmount = mt.CashbackAmount,
            Comment = mt.Comment,
            CurrencyCode = mt.CurrencyCode,
            Description = mt.Description,
            Mcc = mt.Mcc
        }) ?? [];
        
        return domainTransactions.ToList();
    }
}