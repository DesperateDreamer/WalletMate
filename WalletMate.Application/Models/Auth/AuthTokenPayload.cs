namespace WalletMate.Application.Models.Auth;

public class AuthTokenPayload
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; }  = string.Empty;
}