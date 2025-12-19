using Microsoft.AspNetCore.Identity;
using WalletMate.Application.Ports.Out;

namespace WalletMate.Adapters.In.API.Auth;

public class PasswordService(IPasswordHasher<AuthUser> hasher) : IPasswordServicePort
{
    public string HashPassword(string plainPassword)
    {
        var user = new AuthUser();
        return hasher.HashPassword(user, plainPassword);
    }

    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        var user = new AuthUser();
        var result = hasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}