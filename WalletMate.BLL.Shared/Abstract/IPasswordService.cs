using WalletMate.DAL.Entities;

namespace WalletMate.BLL.Shared.Abstract;

public interface IPasswordService
{
    string HashPassword(User user, string password);
    bool VerifyPassword(User user, string hashedPassword, string providedPassword);
}