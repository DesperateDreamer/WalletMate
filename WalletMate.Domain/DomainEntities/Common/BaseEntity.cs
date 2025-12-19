namespace WalletMate.Domain.DomainEntities.Common;

public class BaseEntity : IEntity
{
    public Guid Id { get; set; }
}