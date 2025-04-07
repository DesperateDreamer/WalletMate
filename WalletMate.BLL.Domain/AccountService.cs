using Microsoft.EntityFrameworkCore;
using WalletMate.BLL.Domain.Abstract;
using WalletMate.BLL.Domain.DTOs;
using WalletMate.BLL.Shared;
using WalletMate.BLL.Shared.CustomExceptions;
using WalletMate.DAL.Context.Abstract;
using WalletMate.DAL.Entities;
using WalletMate.DAL.Entities.Enums;
using WalletMate.External.Monobank;

namespace WalletMate.BLL.Domain;

public class AccountService(IDataContext dataContext, IMonobankClient monobankClient) : IAccountService
{
    public async Task<AccountDto> GetAccountByIdAsync(Guid accountId, CancellationToken cancellationToken = default)
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

    public async Task ImportDataFromMonobank(string token, Guid userId, DateTime startDate, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        ValidateDateRange(startDate, endDate);

        var startDateUnix = DateTimeConverter.ToUnixTimestamp(startDate);
        long? endDateUnix = endDate.HasValue ? DateTimeConverter.ToUnixTimestamp(endDate.Value) : null;

        var clientInfo = await monobankClient.GetClientInfoAsync(token);
        if (clientInfo?.Accounts is null || clientInfo.Accounts.Count == 0)
        {
            throw new NotFoundException("No Monobank accounts found.");
        }

        var newAccounts = new List<Account>();

        foreach (var monobankAccount in clientInfo.Accounts)
        {
            var accountTransactions = await monobankClient
                .GetTransactionsAsync(token, monobankAccount.Id, startDateUnix, endDateUnix);

            var mappedTransactions = accountTransactions?
                .Select(tx => new Transaction
                {
                    Amount = tx.Amount / 100m,
                    Currency = (Currency)tx.CurrencyCode,
                    CreatedOn = DateTimeConverter.FromUnixTimestamp(tx.Time)
                }).ToList() ?? [];

            var internalAccount = new Account
            {
                AccountNumber = monobankAccount.Iban,
                Balance = monobankAccount.Balance / 100m,
                Currency = (Currency)monobankAccount.CurrencyCode,
                UserId = userId,
                Transactions = mappedTransactions
            };

            newAccounts.Add(internalAccount);
        }

        await dataContext.Account.AddRangeAsync(newAccounts, cancellationToken);
        await dataContext.SaveChangesAsync(cancellationToken);
    }
    
    private static void ValidateDateRange(DateTime startDate, DateTime? endDate)
    {
        var now = DateTime.UtcNow;

        if (startDate > now)
            throw new NotAcceptableException("Start date cannot be in the future.");

        if (endDate.HasValue && endDate.Value > now)
            throw new NotAcceptableException("End date cannot be in the future.");
    }
}