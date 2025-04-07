using WalletMate.External.Monobank.Models;
using WalletMate.External.Shared.Abstract;
using WalletMate.External.Shared.Enums;

namespace WalletMate.External.Monobank;

public class MonobankClient(IHttpRequestBuilderFactory builderFactory) : IMonobankClient
{
    public async Task<ClientInfo?> GetClientInfoAsync(string token)
    {
        var builder = await builderFactory.Create(BankProvider.Monobank)
            .AddHeader("X-Token", token)
            .SendGetAsync("/personal/client-info");

        return await builder.ParseJsonAsync<ClientInfo>();
    }

    public async Task<List<Transaction>?> GetTransactionsAsync(string token, string accountId, string from, string to)
    {
        var builder = await builderFactory.Create(BankProvider.Monobank)
            .AddHeader("X-Token", token)
            .SendGetAsync($"/personal/statement/{accountId}/{from}/{to}");

        return await builder.ParseJsonAsync<List<Transaction>>();
    }
}