using WalletMate.DAL.Entities.Enums;

namespace WalletMate.BLL.Shared.DTOs;

public class CreateAccountDto
{
    public string AccountNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public Currency Currency { get; set; }
    public Guid UserId { get; set; }
}