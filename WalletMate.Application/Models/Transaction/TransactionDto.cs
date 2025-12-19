namespace WalletMate.Application.Models.Transaction;

public class TransactionDto
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Comment { get; set; }
    public CurrencyEnum Currency { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid AccountId { get; set; }
    public List<Guid> CategoryIds { get; set; } = [];
}