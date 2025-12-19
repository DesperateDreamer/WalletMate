using WalletMate.Application.Exceptions;
using WalletMate.Application.Models.User;
using WalletMate.Application.Ports.In;
using WalletMate.Application.Ports.Out;
using WalletMate.Domain.DomainEntities;
using WalletMate.Domain.Ports.Out.Repositories;

namespace WalletMate.Application.Adapters;

public class UserService(IUserRepository userRepository, IPasswordServicePort passwordHasher) : IUserService
{
    public async Task<UserDto> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            throw new EntityNotFoundException(nameof(User), userId);

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            MiddleName = user.MiddleName,
            Email = user.Email,
            UserName = user.Username
        };
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        var users = await userRepository.GetAllAsync(cancellationToken);
        
        var result = new List<UserDto>(users.Count);
        result.AddRange(users.Select(user => new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            MiddleName = user.MiddleName,
            Email = user.Email,
            UserName = user.Username
        }));

        return result;
    }

    public async Task<Guid> CreateUserAsync(CreateUserDto userDto, CancellationToken cancellationToken = default)
    {
        var exists = await userRepository.ExistsByEmailOrUserNameAsync(
            userDto.Email,
            userDto.UserName,
            cancellationToken);

        if (exists)
            throw new BusinessRuleViolationException("User already exists");

        var passwordHash = passwordHasher.HashPassword(userDto.RawPassword);

        var user = new User
        {
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            MiddleName = userDto.MiddleName,
            Email = userDto.Email
        };

        await userRepository.AddAsync(
            user,
            userName: userDto.UserName,
            passwordHash: passwordHash,
            billingAddress: userDto.BillingAddress,
            cancellationToken: cancellationToken);

        await userRepository.SaveChangesAsync(cancellationToken);

        return user.Id;
    }

    public async Task<Guid> UpdateUserAsync(Guid userId, UpdateUserDto dto, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            throw new EntityNotFoundException(nameof(User), userId);

        var emailOwner = await userRepository.GetByEmailAsync(dto.Email, cancellationToken);
        if (emailOwner is not null && emailOwner.Id != userId)
            throw new BusinessRuleViolationException("User with such email already exists.");

        await userRepository.UpdateProfileAsync(
            userId,
            dto.FirstName,
            dto.LastName,
            dto.MiddleName,
            dto.Email,
            dto.BillingAddress,
            cancellationToken);

        await userRepository.SaveChangesAsync(cancellationToken);

        return userId;
    }

    public async Task<bool> ChangePasswordAsync(
        Guid userId,
        string currentPassword,
        string newPassword,
        CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            throw new EntityNotFoundException(nameof(User), userId);

        var isValid = passwordHasher.VerifyPassword(user.PasswordHash, currentPassword);
        if (!isValid)
            throw new BusinessRuleViolationException("Invalid password");

        var newHash = passwordHasher.HashPassword(newPassword);

        await userRepository.UpdatePasswordHashAsync(userId, newHash, cancellationToken);
        await userRepository.SaveChangesAsync(cancellationToken);

        return true;
    }
}