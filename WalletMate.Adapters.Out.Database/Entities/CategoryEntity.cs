using WalletMate.Adapters.Out.Database.Entities.BaseEntities;

namespace WalletMate.Adapters.Out.Database.Entities;

public class CategoryEntity : EfBaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public List<TransactionCategoryEntity> TransactionCategories { get; set; } = [];
}