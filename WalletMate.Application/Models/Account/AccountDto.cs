namespace WalletMate.Application.Models.Account;

public class AccountDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public CurrencyEnum Currency { get; set; }
    public Guid UserId { get; set; }
}