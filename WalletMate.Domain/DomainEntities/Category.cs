using WalletMate.Domain.DomainEntities.Common;

namespace WalletMate.Domain.DomainEntities;

public class Category : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
}
