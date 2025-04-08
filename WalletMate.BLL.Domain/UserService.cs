using Microsoft.EntityFrameworkCore;
using WalletMate.BLL.Domain.Abstract;
using WalletMate.BLL.Shared.Abstract;
using WalletMate.BLL.Shared.CustomExceptions;
using WalletMate.BLL.Shared.DTOs;
using WalletMate.DAL.Context.Abstract;
using WalletMate.DAL.Entities;

namespace WalletMate.BLL.Domain;

public class UserService(IDataContext dataContext, IPasswordService passwordService) : IUserService
{
    public async Task<UserDto> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await dataContext.User.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (user is null)
        {
            throw new NotFoundException("User not found.");
        }
        
        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            MiddleName = user.MiddleName,
            Email = user.Email,
            UserName = user.UserName,
            BillingAddress = user.BillingAddress
        };
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        var users = await dataContext.User.ToListAsync(cancellationToken);
        
        return users.Select(u => new UserDto
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            MiddleName = u.MiddleName,
            Email = u.Email,
            UserName = u.UserName,
            BillingAddress = u.BillingAddress
        });
    }

    public async Task<Guid> CreateUserAsync(CreateUserDto userDto, CancellationToken cancellationToken = default)
    {
        var existingUser = await dataContext.User
            .FirstOrDefaultAsync(u => u.Email == userDto.Email
                                      || u.UserName == userDto.UserName, cancellationToken);

        if (existingUser is not null)
        {
            throw new NotAcceptableException("User already exists");
        }

        var user = new User
        {
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            MiddleName = userDto.MiddleName,
            Email = userDto.Email,
            UserName = userDto.UserName,
            BillingAddress = userDto.BillingAddress,
        };

        user.PasswordHash = passwordService.HashPassword(user, userDto.RawPassword);

        await dataContext.User.AddAsync(user, cancellationToken);

        await dataContext.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
    
    public async Task<Guid> UpdateUserAsync(Guid userId, UpdateUserDto dto, CancellationToken cancellationToken = default)
    {
        var user = await dataContext.User.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (user is null)
        {
            throw new NotFoundException("User not found.");
        }
        
        var userWithExistingEmail = await dataContext.User.FirstOrDefaultAsync(u => u.Email == dto.Email, cancellationToken);
        if (userWithExistingEmail is not null)
        {
            throw new NotAcceptableException("User with such email already exists.");
        }

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.MiddleName = dto.MiddleName;
        user.BillingAddress = dto.BillingAddress;
        user.Email = dto.Email;

        dataContext.User.Update(user);
        await dataContext.SaveChangesAsync(cancellationToken);
        
        return user.Id;
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword,
        CancellationToken cancellationToken = default)
    {
        var user = await dataContext.User.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (user is null)
        {
            throw new NotFoundException("User not found.");
        }

        if (!passwordService.VerifyPassword(user, user.PasswordHash, currentPassword))
        {
            throw new NotAcceptableException("Invalid password");
        }

        user.PasswordHash = passwordService.HashPassword(user, newPassword);
        
        dataContext.User.Update(user);
        await dataContext.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}