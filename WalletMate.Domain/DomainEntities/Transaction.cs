using WalletMate.Domain.DomainEntities.Common;
using WalletMate.Domain.DomainEntities.Enums;

namespace WalletMate.Domain.DomainEntities;

public class Transaction : BaseEntity
{
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string? Comment { get; set; }
    public Currency Currency { get; set; }
    public DateTime CreatedOn { get; set; }

    public Guid AccountId { get; set; }
}