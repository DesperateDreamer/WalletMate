using WalletMate.Adapters.Out.Database.Entities.BaseEntities;
using WalletMate.Domain.DomainEntities.Enums;

namespace WalletMate.Adapters.Out.Database.Entities;

public class AccountEntity : EfBaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public Currency Currency { get; set; }

    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = null!;

    public List<TransactionEntity> Transactions { get; set; } = [];
}