using WalletMate.Adapters.Out.Database.Entities.BaseEntities;

namespace WalletMate.Adapters.Out.Database.Entities;

public class UserEntity : EfBaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string? BillingAddress { get; set; }

    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public List<AccountEntity> Accounts { get; set; } = [];
}