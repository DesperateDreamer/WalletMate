using WalletMate.External.Shared.Enums;

namespace WalletMate.External.Shared.Abstract;

public interface IHttpRequestBuilderFactory
{
    IHttpRequestBuilder Create(BankProvider provider);
}