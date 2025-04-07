namespace WalletMate.External.Shared.Abstract;

public interface IHttpRequestBuilder
{
    IHttpRequestBuilder AddHeader(string name, string value);
    Task<IHttpRequestBuilder> SendGetAsync(string url);
    Task<T?> ParseJsonAsync<T>();
}