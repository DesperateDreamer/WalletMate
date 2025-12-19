namespace WalletMate.Application.Models.Transaction;

public class UpdateTransactionDto
{
    public decimal Amount { get; init; }
    public CurrencyEnum Currency { get; init; }
    public List<Guid>? CategoryIds { get; init; }
}