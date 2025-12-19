using WalletMate.Adapters.Out.Database.Entities.BaseEntities;

namespace WalletMate.Adapters.Out.Database.Entities;

public class TransactionCategoryEntity : EfBaseEntity
{
    public Guid TransactionId { get; set; }
    public TransactionEntity Transaction { get; set; } = null!;

    public Guid CategoryId { get; set; }
    public CategoryEntity Category { get; set; } = null!;
}