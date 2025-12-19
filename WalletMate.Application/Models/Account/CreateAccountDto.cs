namespace WalletMate.Application.Models.Account;

public class CreateAccountDto
{
    public string AccountNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public CurrencyEnum Currency { get; set; }
    public Guid UserId { get; set; }
}