using WalletMate.BLL.Domain.DTOs;
using WalletMate.DAL.Entities;

namespace WalletMate.BLL.Domain.Abstract;

public interface IUserService
{
    Task<Guid> CreateUserAsync(CreateUserDto userDto, CancellationToken cancellationToken = default);
    Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    Task<Guid> UpdateUserAsync(Guid userId, UpdateUserDto dto, CancellationToken cancellationToken = default);
    Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default);
}