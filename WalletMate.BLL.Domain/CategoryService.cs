using Microsoft.EntityFrameworkCore;
using WalletMate.BLL.Domain.Abstract;
using WalletMate.BLL.Shared.CustomExceptions;
using WalletMate.BLL.Shared.DTOs;
using WalletMate.DAL.Context.Abstract;
using WalletMate.DAL.Entities;

namespace WalletMate.BLL.Domain;

public class CategoryService(IDataContext dataContext) : ICategoryService
{
    public async Task<Category?> GetCategoryByIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await dataContext.Category
            .FirstOrDefaultAsync(c => c.Id == categoryId, cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await dataContext.Category.ToListAsync(cancellationToken);
    }

    public async Task<Guid> CreateCategoryAsync(CreateAndUpdateCategoryDto andUpdateCategoryDto,
        CancellationToken cancellationToken = default)
    {
        var existingCategory = await dataContext.Category
            .FirstOrDefaultAsync(c => c.Name == andUpdateCategoryDto.Name, cancellationToken);
        if (existingCategory is not null)
        {
            throw new NotAcceptableException("Category with this name already exists.");
        }

        var category = new Category
        {
            Name = andUpdateCategoryDto.Name,
            Description = andUpdateCategoryDto.Description
        };

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