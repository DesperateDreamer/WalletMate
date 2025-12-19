using WalletMate.Application.Exceptions;
using WalletMate.Application.Models.Auth;
using WalletMate.Application.Ports.In;
using WalletMate.Application.Ports.Out;
using WalletMate.Domain.Ports.Out.Repositories;

namespace WalletMate.Application.Adapters;

public class AuthService(IUserRepository userRepository, IPasswordServicePort passwordHasher,
    ITokenServicePort tokenGenerator) : IAuthService
{
    public async Task<AuthResultDto> LoginUserAsync(
        LoginDto login,
        CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByEmailAsync(login.Email, cancellationToken);

        if (user is null || !passwordHasher.VerifyPassword(login.Password, user.PasswordHash))
        {
            throw new ForbiddenOperationException("Invalid email or password.");
        }

        var authPayload = new AuthTokenPayload
        {
            Email = user.Email,
            Id = user.Id,
            UserName = user.Username
        };
        
        var token = tokenGenerator.GenerateToken(authPayload);

        return new AuthResultDto
        {
            AccessToken = token
        };
    }
}