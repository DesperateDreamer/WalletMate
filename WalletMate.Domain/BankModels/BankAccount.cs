namespace WalletMate.Domain.BankModels;

public class BankAccount
{
    public required string Id { get; set; }
    public long Balance { get; set; }
    public required string Type { get; set; }
    public required int CurrencyCode { get; set; }
    public required string Iban { get; set; }
}