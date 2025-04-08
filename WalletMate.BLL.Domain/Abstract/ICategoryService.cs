using WalletMate.BLL.Shared.DTOs;
using WalletMate.DAL.Entities;

namespace WalletMate.BLL.Domain.Abstract;

public interface ICategoryService
{
    Task<CategoryDto> GetCategoryByIdAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken = default);
    Task<Guid> CreateCategoryAsync(CreateAndUpdateCategoryDto andUpdateCategoryDto, CancellationToken cancellationToken = default);
    Task<Guid> UpdateCategoryAsync(Guid categoryId, CreateAndUpdateCategoryDto categoryDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default);
}