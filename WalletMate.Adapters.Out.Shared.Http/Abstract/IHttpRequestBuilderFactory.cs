namespace WalletMate.Adapters.Out.Shared.Http.Abstract;

public interface IHttpRequestBuilderFactory
{
    IHttpRequestBuilder Create(string providerName);
}