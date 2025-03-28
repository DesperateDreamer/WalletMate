using WalletMate.BLL.Domain.DTOs;

namespace WalletMate.BLL.Domain.Abstract;

public interface IUserService
{
    Task<Guid> CreateUserAsync(CreateUserDto userDto, CancellationToken cancellationToken = default);
}