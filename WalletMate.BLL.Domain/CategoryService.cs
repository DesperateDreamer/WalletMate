using Microsoft.EntityFrameworkCore;
using WalletMate.BLL.Domain.Abstract;
using WalletMate.BLL.Shared.CustomExceptions;
using WalletMate.BLL.Shared.DTOs;
using WalletMate.DAL.Context.Abstract;
using WalletMate.DAL.Entities;

namespace WalletMate.BLL.Domain;

public class CategoryService(IDataContext dataContext) : ICategoryService
{
    public async Task<CategoryDto> GetCategoryByIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        var category = await dataContext.Category
            .FirstOrDefaultAsync(c => c.Id == categoryId, cancellationToken);
        
        if (category is null)
        {
            throw new NotFoundException("Category not found.");
        }
        
        return new CategoryDto
        {
            Description = category.Description,
            Name = category.Name,
            Id = category.Id,
            TransactionCategories = category.TransactionCategories
                .Select(tc => tc.Id)
                .ToList()
        };
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var categories = await dataContext.Category
            .Include(c => c.TransactionCategories)
            .ToListAsync(cancellationToken);

        return categories.Select(c => new CategoryDto
        {
            Description = c.Description,
            Name = c.Name,
            Id = c.Id,
            TransactionCategories = c.TransactionCategories
                .Select(tc => tc.Id)
                .ToList()
        });
    }

    public async Task<Guid> CreateCategoryAsync(CreateAndUpdateCategoryDto categoryDto,
        CancellationToken cancellationToken = default)
    {
        var existingCategory = await dataContext.Category
            .FirstOrDefaultAsync(c => c.Name == categoryDto.Name, cancellationToken);
        
        if (existingCategory is not null)
        {
            throw new NotAcceptableException("Category with this name already exists.");
        }
        
        var category = new Category
        {
            Name = categoryDto.Name,
            Description = categoryDto.Description
        };
        
        if (categoryDto.TransactionCategories is { Count: > 0 })
        {
            var categories = await dataContext.TransactionCategory
                .Where(c => categoryDto.TransactionCategories.Contains(c.Id))
                .ToListAsync(cancellationToken);

            if (categories.Count != categoryDto.TransactionCategories.Count)
                throw new NotAcceptableException("One or more category IDs are invalid.");

            category.TransactionCategories.AddRange(categories);
        }
        
        await dataContext.Category.AddAsync(category, cancellationToken);
        await dataContext.SaveChangesAsync(cancellationToken);

        return category.Id;
    }

    public async Task<Guid> UpdateCategoryAsync(Guid categoryId, CreateAndUpdateCategoryDto categoryDto,
        CancellationToken cancellationToken = default)
    {
        var category = await dataContext.Category.FirstOrDefaultAsync(c => c.Id == categoryId, cancellationToken);
        
        if (category is null)
        {
            throw new NotFoundException("Category not found.");
        }
        
        var anotherCategoryWithSameName = await dataContext.Category
            .FirstOrDefaultAsync(c => c.Name == categoryDto.Name && c.Id != categoryId, cancellationToken);
        
        if (anotherCategoryWithSameName is not null)
        {
            throw new NotAcceptableException("Another category with the same name already exists.");
        }
        
        if (categoryDto.TransactionCategories is { Count: > 0 })
        {
            var categories = await dataContext.TransactionCategory
                .Where(c => categoryDto.TransactionCategories.Contains(c.Id))
                .ToListAsync(cancellationToken);

            if (categories.Count != categoryDto.TransactionCategories.Count)
                throw new NotAcceptableException("One or more category IDs are invalid.");

            category.TransactionCategories.AddRange(categories);
        }

        category.Name = categoryDto.Name;
        category.Description = categoryDto.Description;

        dataContext.Category.Update(category);
        await dataContext.SaveChangesAsync(cancellationToken);

        return category.Id;
    }

    public async Task<bool> DeleteCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        var category = await dataContext.Category.FirstOrDefaultAsync(c => c.Id == categoryId, cancellationToken);
        if (category is null)
        {
            throw new NotFoundException("Category not found.");
        }

        dataContext.Category.Remove(category);
        await dataContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}