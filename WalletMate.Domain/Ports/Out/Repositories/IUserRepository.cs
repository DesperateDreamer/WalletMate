using WalletMate.Domain.DomainEntities;

namespace WalletMate.Domain.Ports.Out.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsByUserNameAsync(string userName, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailOrUserNameAsync(string email, string userName,
        CancellationToken cancellationToken = default);
    Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(
        User user,
        string userName,
        string passwordHash,
        string? billingAddress = null,
        CancellationToken cancellationToken = default);
    Task UpdateProfileAsync(
        Guid userId,
        string firstName,
        string lastName,
        string? middleName,
        string email,
        string? billingAddress,
        CancellationToken cancellationToken = default);
    Task UpdatePasswordHashAsync(
        Guid userId,
        string passwordHash,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}