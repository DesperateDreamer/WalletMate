namespace WalletMate.Application.Models.User;

public class UpdateUserDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string? BillingAddress { get; set; }
    public string Email { get; set; } = string.Empty;
}