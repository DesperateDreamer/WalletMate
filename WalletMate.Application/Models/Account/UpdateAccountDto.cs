namespace WalletMate.Application.Models.Account;

public class UpdateAccountDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public CurrencyEnum Currency { get; set; }
}