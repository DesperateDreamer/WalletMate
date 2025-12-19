namespace WalletMate.Application.Models.Transaction;

public class CreateTransactionDto
{
    public decimal Amount { get; set; }
    public CurrencyEnum Currency { get; set; }
    public Guid AccountId { get; set; }
    public List<Guid>? CategoryIds { get; set; }
}