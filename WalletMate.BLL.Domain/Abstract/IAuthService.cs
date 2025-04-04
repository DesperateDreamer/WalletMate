using WalletMate.BLL.Domain.DTOs;

namespace WalletMate.BLL.Domain.Abstract;

public interface IAuthService
{
    Task<string> LoginUserAsync(LoginDto loginDto, CancellationToken cancellationToken = default);
}