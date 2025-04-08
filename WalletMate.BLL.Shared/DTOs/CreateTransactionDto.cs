using WalletMate.DAL.Entities.Enums;

namespace WalletMate.BLL.Shared.DTOs;

public class CreateTransactionDto
{
    public decimal Amount { get; set; }
    public Currency Currency { get; set; }
    public Guid AccountId { get; set; }
    public List<Guid>? CategoryIds { get; set; }
}