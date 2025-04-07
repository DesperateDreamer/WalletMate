using WalletMate.DAL.Entities.BaseEntities;
using WalletMate.DAL.Entities.Enums;

namespace WalletMate.DAL.Entities;

public class Account : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; }  = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public Currency Currency { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public List<Transaction> Transactions { get; set; } = [];
}