using WalletMate.Application.Models;
using WalletMate.Application.Models.Auth;

namespace WalletMate.Application.Ports.In;

public interface IAuthService
{
    Task<AuthResultDto> LoginUserAsync(LoginDto loginDto, CancellationToken cancellationToken = default);
}