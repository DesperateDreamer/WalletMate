using WalletMate.DAL.Entities.BaseEntities;
using WalletMate.DAL.Entities.Enums;

namespace WalletMate.DAL.Entities;

public class Transaction : BaseEntity
{
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Comment { get; set; }
    public Currency Currency { get; set; }
    public DateTime CreatedOn { get; set; }

    public Guid AccountId { get; set; }
    public Account Account { get; set; } = null!;

    public List<TransactionCategory> TransactionCategories { get; set; } = [];
}