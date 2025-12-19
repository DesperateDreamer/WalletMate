using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WalletMate.Adapters.Out.Database.Abstract;
using WalletMate.Adapters.Out.Database.Entities;
using WalletMate.Domain.DomainEntities;
using WalletMate.Domain.Ports.Out.Repositories;

namespace WalletMate.Adapters.Out.Database.RepositoryAdapters;

public class UserRepositoryAdapter(IDataContext dataContext) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var entity = await dataContext.User
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        return entity is null ? null : ToDomain(entity);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var entity = await dataContext.User
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        return entity is null ? null : ToDomain(entity);
    }

    public async Task<bool> ExistsByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await dataContext.User.AnyAsync(u => u.UserName == userName, cancellationToken);
    }

    public async Task<bool> ExistsByEmailOrUserNameAsync(string email, string userName, CancellationToken cancellationToken = default)
    {
        return await dataContext.User.AnyAsync(u => u.Email == email || u.UserName == userName, cancellationToken);
    }

    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await dataContext.User
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return entities.Select(ToDomain).ToList();
    }

    public async Task AddAsync(
        User user,
        string userName,
        string passwordHash,
        string? billingAddress = null,
        CancellationToken cancellationToken = default)
    {
        var entity = ToEntity(user, userName, passwordHash, billingAddress);
        await dataContext.User.AddAsync(entity, cancellationToken);
    }

    public async Task UpdateProfileAsync(
        Guid userId,
        string firstName,
        string lastName,
        string? middleName,
        string email,
        string? billingAddress,
        CancellationToken cancellationToken = default)
    {
        var entity = await dataContext.User.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (entity is null)
            return;

        entity.FirstName = firstName;
        entity.LastName = lastName;
        entity.MiddleName = middleName;
        entity.Email = email;
        entity.BillingAddress = billingAddress;

        dataContext.User.Update(entity);
    }

    public async Task UpdatePasswordHashAsync(Guid userId, string passwordHash, CancellationToken cancellationToken = default)
    {
        var entity = await dataContext.User.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (entity is null)
            return;

        entity.PasswordHash = passwordHash;

        dataContext.User.Update(entity);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dataContext.SaveChangesAsync(cancellationToken);
    }

    private static UserEntity ToEntity(User domain, string userName, string passwordHash, string? billingAddress)
    {
        return new UserEntity
        {
            Id = domain.Id,
            FirstName = domain.FirstName,
            LastName = domain.LastName,
            MiddleName = domain.MiddleName,
            Email = domain.Email,
            UserName = userName,
            PasswordHash = passwordHash,
            BillingAddress = billingAddress
        };
    }

    private static User ToDomain(UserEntity entity)
    {
        return new User
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            MiddleName = entity.MiddleName,
            Email = entity.Email
        };
    }
}