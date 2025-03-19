using WalletMate.DAL.Entities.BaseEntities;

namespace WalletMate.DAL.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public List<TransactionCategory> TransactionCategories { get; set; } = [];
}