using WalletMate.Adapters.Out.Database.Entities.BaseEntities;
using WalletMate.Domain.DomainEntities.Enums;

namespace WalletMate.Adapters.Out.Database.Entities;

public class TransactionEntity : EfBaseEntity
{
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Comment { get; set; }
    public Currency Currency { get; set; }
    public DateTime CreatedOn { get; set; }

    public Guid AccountId { get; set; }
    public AccountEntity Account { get; set; } = null!;

    public List<TransactionCategoryEntity> TransactionCategories { get; set; } = [];
}