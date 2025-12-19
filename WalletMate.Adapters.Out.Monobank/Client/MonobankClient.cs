using System.Net.Http.Json;
using WalletMate.Adapters.Out.Monobank.Models;
using WalletMate.Adapters.Out.Shared.Http.Abstract;

namespace WalletMate.Adapters.Out.Monobank.Client;

// Adapter pattern
public class MonobankClient(IHttpRequestBuilderFactory builderFactory) : IMonobankClient
{
    private const string MonobankProvider = "Monobank";
    private static readonly SemaphoreSlim _semaphore = new(1, 1);
    
    public async Task<MonobankClientInfo?> GetClientInfoAsync(string token)
    {
        await _semaphore.WaitAsync();
        
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(60));
            var response = await builderFactory.Create(MonobankProvider)
                .AddHeader("X-Token", token)
                .SendGetAsync("/personal/client-info");

            return await response.Content.ReadFromJsonAsync<MonobankClientInfo>();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<List<MonobankTransaction>?> GetTransactionsAsync(string token, string accountId, long from, long? to)
    {
        await _semaphore.WaitAsync();
        
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(60));
            var response = await builderFactory.Create(MonobankProvider)
                .AddHeader("X-Token", token)
                .SendGetAsync($"/personal/statement/{accountId}/{from}/{to}");

            return await response.Content.ReadFromJsonAsync<List<MonobankTransaction>>();
        }
        finally
        {
            _semaphore.Release();
        }
    }
}