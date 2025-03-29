namespace WalletMate.BLL.Domain.DTOs;

public class VerifyCredentialsDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}