using WalletMate.DAL.Entities.Enums;

namespace WalletMate.BLL.Domain.DTOs;

public class UpdateAccountDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public Currency Currency { get; set; }
}