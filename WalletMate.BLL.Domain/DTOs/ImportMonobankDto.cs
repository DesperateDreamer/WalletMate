namespace WalletMate.BLL.Domain.DTOs;

public class ImportMonobankDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}