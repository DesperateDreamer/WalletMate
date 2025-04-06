using Newtonsoft.Json;
using WalletMate.External.Monobank.Models;

namespace WalletMate.External.Monobank;

public class MonobankClient(HttpClient httpClient) : IMonobankClient
{
    public async Task<ClientInfo?> GetClientInfoAsync(string token)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/personal/client-info");
        request.Headers.Add("X-Token", token);
        
        using var response = await httpClient.SendAsync(request);
        
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        
        var content = await response.Content.ReadAsStringAsync();

        try
        {
            return JsonConvert.DeserializeObject<ClientInfo>(content);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    public async Task<List<Transaction>?> GetTransactionsAsync(string token, string accountId, string from, string to)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/personal/statement/{accountId}/{from}/{to}");
        request.Headers.Add("X-Token", token);
        
        using var response = await httpClient.SendAsync(request);
        
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        
        var content = await response.Content.ReadAsStringAsync();

        try
        {
            return JsonConvert.DeserializeObject<List<Transaction>>(content);
        }
        catch (JsonException)
        {
            return null;
        }
    }
}