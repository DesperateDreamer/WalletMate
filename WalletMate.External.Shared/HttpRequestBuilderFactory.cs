using WalletMate.External.Shared.Abstract;
using WalletMate.External.Shared.Enums;

namespace WalletMate.External.Shared;

public class HttpRequestBuilderFactory(IHttpClientFactory clientFactory) : IHttpRequestBuilderFactory
{
    public IHttpRequestBuilder Create(BankProvider provider)
    {
        var clientName = provider switch
        {
            BankProvider.Monobank => "Monobank",
            BankProvider.FakeBank => "FakeBank",
            _ => throw new ArgumentOutOfRangeException(nameof(provider), $"Unsupported bank: {provider}")
        };

        var client = clientFactory.CreateClient(clientName);
        return new HttpRequestBuilder(client);
    }
}