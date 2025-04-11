using WalletMate.External.Shared.Abstract;

namespace WalletMate.External.Shared;

// Builder pattern
public class HttpRequestBuilder(HttpClient client) : IHttpRequestBuilder
{
    private readonly HttpRequestMessage _request = new();
    private HttpResponseMessage? _response;

    public IHttpRequestBuilder AddHeader(string name, string value)
    {
        _request.Headers.Add(name, value);
        return this;
    }

    public async Task<HttpResponseMessage> SendGetAsync(string url)
    {
        _request.Method = HttpMethod.Get;
        _request.RequestUri = new Uri(url, UriKind.Relative);
        _response = await client.SendAsync(_request);
        _response.EnsureSuccessStatusCode();
        return _response;
    }
}