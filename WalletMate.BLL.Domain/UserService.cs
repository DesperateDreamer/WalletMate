using Microsoft.EntityFrameworkCore;
using WalletMate.BLL.Domain.DTOs;
using WalletMate.BLL.Shared.Abstract;
using WalletMate.DAL.Context.Abstract;
using WalletMate.DAL.Entities;

namespace WalletMate.BLL.Domain;

public class UserService(IDataContext dataContext, IPasswordService passwordService)
{
    public async Task<Guid> CreateUserAsync(CreateUserDto userDto, CancellationToken cancellationToken = default)
    {
        var existingUser = await dataContext.User
            .FirstOrDefaultAsync(u => u.Email == userDto.Email 
                                      || u.UserName == userDto.UserName, cancellationToken);

        if (existingUser is not null)
        {
            throw new Exception("User already exists"); //TODO: middle point for catching exceptions + logging
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
}