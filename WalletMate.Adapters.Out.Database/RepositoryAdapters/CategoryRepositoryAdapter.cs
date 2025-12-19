using Microsoft.EntityFrameworkCore;
using WalletMate.Adapters.Out.Database.Abstract;
using WalletMate.Adapters.Out.Database.Entities;
using WalletMate.Domain.DomainEntities;
using WalletMate.Domain.Ports.Out.Repositories;

namespace WalletMate.Adapters.Out.Database.RepositoryAdapters;

public class CategoryRepositoryAdapter(IDataContext dataContext) : ICategoryRepository
{
    public async Task<Category?> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        var entity = await dataContext.Category
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == categoryId, cancellationToken);

        return entity is null ? null : ToDomain(entity);
    }

    public async Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var entity = await dataContext.Category
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Name == name, cancellationToken);

        return entity is null ? null : ToDomain(entity);
    }

    public async Task<List<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await dataContext.Category
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return entities.Select(ToDomain).ToList();
    }

    public Task AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        var entity = ToEntity(category);
        return dataContext.Category.AddAsync(entity, cancellationToken).AsTask();
    }

    public void Update(Category category)
    {
        var entity = ToEntity(category);
        dataContext.Category.Update(entity);
    }

    public void Remove(Category category)
    {
        var entity = new CategoryEntity { Id = category.Id };
        dataContext.Category.Remove(entity);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dataContext.SaveChangesAsync(cancellationToken);
    }

    private static CategoryEntity ToEntity(Category domain)
    {
        return new CategoryEntity
        {
            Id = domain.Id,
            Name = domain.Name,
            Description = domain.Description
        };
    }

    private static Category ToDomain(CategoryEntity entity)
    {
        return new Category
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description
        };
    }
}