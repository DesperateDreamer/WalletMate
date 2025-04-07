using Microsoft.EntityFrameworkCore;
using WalletMate.BLL.Domain.Abstract;
using WalletMate.BLL.Domain.DTOs;
using WalletMate.BLL.Shared.Abstract;
using WalletMate.DAL.Context.Abstract;

namespace WalletMate.BLL.Domain;

public class AuthService(ITokenService tokenService, IDataContext dataContext, IPasswordService passwordService) : IAuthService
{
    public async Task<string> LoginUserAsync(LoginDto loginDto, CancellationToken cancellationToken = default)
    {
        var user = await dataContext.User.FirstOrDefaultAsync(u => u.Email == loginDto.Email, cancellationToken);

        var isValid = user is not null && passwordService.VerifyPassword(user, user.PasswordHash, loginDto.Password);

        if (!isValid)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }
        
        var token = tokenService.GenerateToken(user!);
        return token;
    }
}