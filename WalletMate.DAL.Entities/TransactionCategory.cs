using WalletMate.DAL.Entities.BaseEntities;

namespace WalletMate.DAL.Entities;

public class TransactionCategory : BaseEntity
{
    public Guid TransactionId { get; set; }
    public Transaction Transaction { get; set; } = null!;

    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}