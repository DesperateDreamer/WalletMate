using Microsoft.EntityFrameworkCore;
using WalletMate.BLL.Domain.Abstract;
using WalletMate.BLL.Domain.DTOs;
using WalletMate.BLL.Shared.CustomExceptions;
using WalletMate.DAL.Context.Abstract;
using WalletMate.DAL.Entities;

namespace WalletMate.BLL.Domain;

public class AccountService(IDataContext dataContext) : IAccountService
{
    public async Task<AccountDto?> GetAccountByIdAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        var account = await dataContext.Account
            .FirstOrDefaultAsync(a => a.Id == accountId, cancellationToken);

        if (account is null)
        {
            throw new NotFoundException("Account not found.");
        }

        return new AccountDto
        {
            Id = account.Id,
            Name = account.Name,
            Description = account.Description,
            Balance = account.Balance,
            Currency = account.Currency,
            UserId = account.UserId
        };
    }

    public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync(CancellationToken cancellationToken = default)
    {
        var accounts = await dataContext.Account.ToListAsync(cancellationToken);

        return accounts.Select(a => new AccountDto
        {
            Id = a.Id,
            Name = a.Name,
            Description = a.Description,
            Balance = a.Balance,
            Currency = a.Currency,
            UserId = a.UserId
        });
    }

    public async Task<Guid> CreateAccountAsync(CreateAccountDto dto, CancellationToken cancellationToken = default)
    {
        var newAccount = new Account
        {
            Name = dto.Name,
            Description = dto.Description,
            Balance = dto.Balance,
            Currency = dto.Currency,
            UserId = dto.UserId
        };

        await dataContext.Account.AddAsync(newAccount, cancellationToken);
        await dataContext.SaveChangesAsync(cancellationToken);

        return newAccount.Id;
    }

    public async Task<Guid> UpdateAccountAsync(Guid accountId, UpdateAccountDto dto,
        CancellationToken cancellationToken = default)
    {
        var account = await dataContext.Account
            .FirstOrDefaultAsync(a => a.Id == accountId, cancellationToken);

        if (account is null)
        {
            throw new NotFoundException("Account not found.");
        }
        
        account.Name = dto.Name;
        account.Description = dto.Description;
        account.Balance = dto.Balance;
        account.Currency = dto.Currency;

        dataContext.Account.Update(account);
        await dataContext.SaveChangesAsync(cancellationToken);

        return account.Id;
    }

    public async Task DeleteAccountAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        var account = await dataContext.Account
            .FirstOrDefaultAsync(a => a.Id == accountId, cancellationToken);

        if (account is null)
        {
            throw new NotFoundException("Account not found.");
        }

        dataContext.Account.Remove(account);
        await dataContext.SaveChangesAsync(cancellationToken);
    }
}