using WalletMate.Application.Exceptions;
using WalletMate.Application.Models.Category;
using WalletMate.Application.Ports.In;
using WalletMate.Domain.DomainEntities;
using WalletMate.Domain.Ports.Out.Repositories;

namespace WalletMate.Application.Adapters;

public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    public async Task<CategoryDto> GetCategoryByIdAsync(
        Guid categoryId,
        CancellationToken cancellationToken = default)
    {
        var category = await categoryRepository.GetByIdAsync(
            categoryId,
            cancellationToken);

        if (category is null)
            throw new EntityNotFoundException(nameof(Category), categoryId);

        return MapToDto(category);
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(
        CancellationToken cancellationToken = default)
    {
        var categories = await categoryRepository.GetAllAsync(cancellationToken);
        return categories.Select(MapToDto).ToList();
    }

    public async Task<Guid> CreateCategoryAsync(
        CreateAndUpdateCategoryDto dto,
        CancellationToken cancellationToken = default)
    {
        var existing = await categoryRepository.GetByNameAsync(
            dto.Name,
            cancellationToken);

        if (existing is not null) 
            throw new BusinessRuleViolationException("Category with this name already exists.");

        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description
        };

        if (dto.TransactionCategories is { Count: > 0 })
        {
            // var transactionCategories =
            //     await LoadTransactionCategoriesAsync(
            //         dto.TransactionCategories,
            //         cancellationToken);

            // category.TransactionCategories.AddRange(transactionCategories);
        }

        await categoryRepository.AddAsync(category, cancellationToken);
        await categoryRepository.SaveChangesAsync(cancellationToken);

        return category.Id;
    }

    public async Task<Guid> UpdateCategoryAsync(
        Guid categoryId,
        CreateAndUpdateCategoryDto dto,
        CancellationToken cancellationToken = default)
    {
        var category = await categoryRepository.GetByIdAsync(
            categoryId,
            cancellationToken);

        if (category is null)
            throw new EntityNotFoundException(nameof(Category), categoryId);

        var duplicate = await categoryRepository.GetByNameAsync(dto.Name, cancellationToken);

        if (duplicate is not null && duplicate.Id != categoryId)
            throw new BusinessRuleViolationException("Another category with the same name already exists.");

        if (dto.TransactionCategories is { Count: > 0 })
        {
            // var transactionCategories =
            //     await LoadTransactionCategoriesAsync(
            //         dto.TransactionCategories,
            //         cancellationToken);

            // category.TransactionCategories.Clear();
            // category.TransactionCategories.AddRange(transactionCategories);
        }

        category.Name = dto.Name;
        category.Description = dto.Description;

        categoryRepository.Update(category);
        await categoryRepository.SaveChangesAsync(cancellationToken);

        return category.Id;
    }

    public async Task DeleteCategoryAsync(
        Guid categoryId,
        CancellationToken cancellationToken = default)
    {
        var category = await categoryRepository.GetByIdAsync(
            categoryId,
            cancellationToken);

        if (category is null)
            throw new EntityNotFoundException(nameof(Category), categoryId);

        categoryRepository.Remove(category);
        await categoryRepository.SaveChangesAsync(cancellationToken);
    }

    // private async Task<IReadOnlyList<TransactionCategory>> LoadTransactionCategoriesAsync(
    //     IReadOnlyCollection<Guid> ids,
    //     CancellationToken ct)
    // {
    //     var categories = await _transactionCategoryRepository
    //         .GetByIdsAsync(ids, ct);
    //
    //     if (categories.Count != ids.Count)
    //         throw new BusinessRuleViolationException(
    //             "One or more transaction category IDs are invalid.");
    //
    //     return categories;
    // }

    private static CategoryDto MapToDto(Category category) =>
        new()
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            // TransactionCategories =
            //     category.TransactionCategories.Select(tc => tc.Id).ToList()
        };

}