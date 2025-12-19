using WalletMate.Application.Models;
using WalletMate.Application.Models.Category;

namespace WalletMate.Application.Ports.In;

public interface ICategoryService
{
    Task<CategoryDto> GetCategoryByIdAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken = default);
    Task<Guid> CreateCategoryAsync(CreateAndUpdateCategoryDto andUpdateCategoryDto, CancellationToken cancellationToken = default);
    Task<Guid> UpdateCategoryAsync(Guid categoryId, CreateAndUpdateCategoryDto categoryDto, CancellationToken cancellationToken = default);
    Task DeleteCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default);
}