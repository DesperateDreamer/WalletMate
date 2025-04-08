namespace WalletMate.BLL.Shared.DTOs;

public class CreateUserDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string? BillingAddress { get; set; }
    public string Email { get; set; } = string.Empty;
    public string RawPassword { get; set; } = string.Empty;
}