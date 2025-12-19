using WalletMate.Domain.DomainEntities.Common;
using WalletMate.Domain.DomainEntities.Enums;

namespace WalletMate.Domain.DomainEntities;

public class Account : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public Currency Currency { get; set; }
    public Guid UserId { get; set; }
    public IEnumerable<Transaction> Transactions { get; set; } = [];
}