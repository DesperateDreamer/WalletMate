using System.Net.Http.Json;
using WalletMate.External.Shared.Abstract;

namespace WalletMate.External.Shared;

public class HttpRequestBuilder(HttpClient client) : IHttpRequestBuilder
{
    private readonly HttpRequestMessage _request = new();
    private HttpResponseMessage? _response;

    public IHttpRequestBuilder AddHeader(string name, string value)
    {
        _request.Headers.Add(name, value);
        return this;
    }

    public async Task<IHttpRequestBuilder> SendGetAsync(string url)
    {
        _request.Method = HttpMethod.Get;
        _request.RequestUri = new Uri(url, UriKind.Relative);
        _response = await client.SendAsync(_request);
        return this;
    }

    public async Task<T?> ParseJsonAsync<T>()
    {
        if (_response is not { IsSuccessStatusCode: true })
            return default;

        return await _response.Content.ReadFromJsonAsync<T>();
    }
}