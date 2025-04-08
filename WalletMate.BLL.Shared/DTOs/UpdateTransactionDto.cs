using WalletMate.DAL.Entities.Enums;

namespace WalletMate.BLL.Shared.DTOs;

public class UpdateTransactionDto
{
    public decimal Amount { get; init; }
    public Currency Currency { get; init; }
    public List<Guid>? CategoryIds { get; init; }
}