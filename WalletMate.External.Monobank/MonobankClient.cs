using System.Net.Http.Json;
using WalletMate.External.Monobank.Models;
using WalletMate.External.Shared.Abstract;
using WalletMate.External.Shared.Enums;

namespace WalletMate.External.Monobank;

// Adapter pattern
public class MonobankClient(IHttpRequestBuilderFactory builderFactory) : IMonobankClient
{
    private static readonly SemaphoreSlim _semaphore = new(1, 1);
    
    public async Task<ClientInfo?> GetClientInfoAsync(string token)
    {
        await _semaphore.WaitAsync();
        
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(60));
            var response = await builderFactory.Create(BankProvider.Monobank)
                .AddHeader("X-Token", token)
                .SendGetAsync("/personal/client-info");

            return await response.Content.ReadFromJsonAsync<ClientInfo>();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<List<Transaction>?> GetTransactionsAsync(string token, string accountId, long from, long? to)
    {
        await _semaphore.WaitAsync();
        
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(60));
            var response = await builderFactory.Create(BankProvider.Monobank)
                .AddHeader("X-Token", token)
                .SendGetAsync($"/personal/statement/{accountId}/{from}/{to}");

            return await response.Content.ReadFromJsonAsync<List<Transaction>>();
        }
        finally
        {
            _semaphore.Release();
        }
    }
}