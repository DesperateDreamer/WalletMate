using WalletMate.DAL.Entities.BaseEntities;

namespace WalletMate.DAL.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string? BillingAddress { get; set; }

    public string Email { get; set; }  = string.Empty;
    public string UserName { get; set; }  = string.Empty;
    public string PasswordHash { get; set; }  = string.Empty;

    public List<Account> Accounts { get; set; } = [];
}