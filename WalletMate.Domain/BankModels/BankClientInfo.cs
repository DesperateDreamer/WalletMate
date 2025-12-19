namespace WalletMate.Domain.BankModels;

public class BankClientInfo
{
    public required string ClientId { get; set; }
    public required string Name { get; set; }
    public required IEnumerable<BankAccount>? Accounts { get; set; }
}