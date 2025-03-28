using Microsoft.AspNetCore.Identity;
using WalletMate.BLL.Shared.Abstract;
using WalletMate.DAL.Entities;

namespace WalletMate.BLL.Shared;

public class PasswordService(IPasswordHasher<User> hasher) : IPasswordService
{
    public string HashPassword(User user, string plainPassword)
    {
        return hasher.HashPassword(user, plainPassword);
    }

    public bool VerifyPassword(User user, string hashedPassword, string providedPassword)
    {
        var result = hasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}