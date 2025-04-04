using WalletMate.DAL.Entities;

namespace WalletMate.BLL.Shared.Abstract;

public interface ITokenService
{
    string GenerateToken(User user);
}