namespace WalletMate.External.Shared.Abstract;

public interface IHttpRequestBuilder
{
    IHttpRequestBuilder AddHeader(string name, string value);
    Task<HttpResponseMessage> SendGetAsync(string url);
}