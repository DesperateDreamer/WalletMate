using WalletMate.Adapters.Out.Shared.Http.Abstract;

namespace WalletMate.Adapters.Out.Shared.Http;

// Abstract Factory pattern
public class HttpRequestBuilderFactory(IHttpClientFactory clientFactory) : IHttpRequestBuilderFactory
{
    public IHttpRequestBuilder Create(string providerName)
    {
        var client = clientFactory.CreateClient(providerName);
        return new HttpRequestBuilder(client);
    }
}