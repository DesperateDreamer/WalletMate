namespace WalletMate.Application.Ports.Out;

public interface IPasswordServicePort
{
    public string HashPassword(string plainPassword);
    public bool VerifyPassword(string hashedPassword, string providedPassword);
}