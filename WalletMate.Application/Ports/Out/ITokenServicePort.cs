using WalletMate.Application.Models.Auth;

namespace WalletMate.Application.Ports.Out;

public interface ITokenServicePort
{
    public string GenerateToken(AuthTokenPayload payload);
}