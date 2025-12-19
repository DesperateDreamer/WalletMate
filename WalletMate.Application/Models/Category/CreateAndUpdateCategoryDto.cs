namespace WalletMate.Application.Models.Category;

public class CreateAndUpdateCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public List<Guid> TransactionCategories { get; set; } = [];
}