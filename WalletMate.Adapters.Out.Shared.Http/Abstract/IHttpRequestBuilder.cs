namespace WalletMate.Adapters.Out.Shared.Http.Abstract;

public interface IHttpRequestBuilder
{
    IHttpRequestBuilder AddHeader(string name, string value);
    Task<HttpResponseMessage> SendGetAsync(string url);
}