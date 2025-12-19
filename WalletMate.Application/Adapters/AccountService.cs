using WalletMate.Application.Exceptions;
using WalletMate.Application.Helpers;
using WalletMate.Application.Models;
using WalletMate.Application.Models.Account;
using WalletMate.Application.Ports.In;
using WalletMate.Application.Ports.Out;
using WalletMate.Domain.DomainEntities;
using WalletMate.Domain.DomainEntities.Enums;
using WalletMate.Domain.Ports.Out;
using WalletMate.Domain.Ports.Out.Repositories;

namespace WalletMate.Application.Adapters;

public class AccountService(IAccountRepository accountRepository, IBankProviderPort bankProvider, 
    ICacheManagerPort cache) : IAccountService
{
    public async Task<AccountDto> GetAccountByIdAsync(
        Guid accountId,
        CancellationToken cancellationToken = default)
    {
        var account = await accountRepository.FirstOrDefaultAsync(
            a => a.Id == accountId,
            cancellationToken);

        if (account is null)
            throw new EntityNotFoundException(nameof(Account), accountId);

        return MapToDto(account);
    }

    public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync(
        CancellationToken cancellationToken = default)
    {
        var accounts = await accountRepository.ToListAsync(cancellationToken);
        return accounts.Select(MapToDto).ToList();
    }

    public async Task<Guid> CreateAccountAsync(
        CreateAccountDto dto,
        CancellationToken cancellationToken = default)
    {
        var exists = await accountRepository.FirstOrDefaultAsync(
            a => a.AccountNumber == dto.AccountNumber,
            cancellationToken);

        if (exists is not null)
            throw new BusinessRuleViolationException(
                "Account with this account number already exists.");

        var account = new Account
        {
            AccountNumber = dto.AccountNumber,
            Name = dto.Name,
            Description = dto.Description,
            Balance = dto.Balance,
            Currency = (Currency)dto.Currency,
            UserId = dto.UserId,
            Transactions = []
        };

        await accountRepository.AddAsync(account, cancellationToken);
        await accountRepository.SaveChangesAsync(cancellationToken);

        return account.Id;
    }

    public async Task<Guid> UpdateAccountAsync(
        Guid accountId,
        UpdateAccountDto dto,
        CancellationToken cancellationToken = default)
    {
        var account = await accountRepository.FirstOrDefaultAsync(
            a => a.Id == accountId,
            cancellationToken);

        if (account is null)
            throw new EntityNotFoundException(nameof(Account), accountId);

        account.Name = dto.Name;
        account.Description = dto.Description;
        account.Balance = dto.Balance;
        account.Currency = (Currency)dto.Currency;

        accountRepository.Update(account);
        await accountRepository.SaveChangesAsync(cancellationToken);

        return account.Id;
    }

    public async Task DeleteAccountAsync(
        Guid accountId,
        CancellationToken cancellationToken = default)
    {
        var account = await accountRepository.FirstOrDefaultAsync(
            a => a.Id == accountId,
            cancellationToken);

        if (account is null)
            throw new EntityNotFoundException(nameof(Account), accountId);

        accountRepository.Remove(account);
        await accountRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task ImportFromMonobankAsync(
        ImportMonobankDto dto,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        ValidateDateRange(dto.StartDate, dto.EndDate);

        var fromUnix = DateTimeConverter.ToUnixTimestamp(dto.StartDate);
        var toUnix = dto.EndDate.HasValue
            ? DateTimeConverter.ToUnixTimestamp(dto.EndDate.Value)
            : 0;

        var clientInfo = await bankProvider.GetClientInfoAsync(dto.Token);
        var bankAccounts = clientInfo?.Accounts?.ToList() ?? [];

        if (bankAccounts.Count == 0)
            throw new EntityNotFoundException("BankAccount", "Monobank");

        var newAccounts = new List<Account>();

        foreach (var bankAccount in bankAccounts)
        {
            var exists = await accountRepository.FirstOrDefaultAsync(
                a => a.AccountNumber == bankAccount.Iban,
                cancellationToken);

            if (exists is not null)
                continue;

            var cacheKey = $"monobank:tx:{bankAccount.Id}";

            var bankTransactions = await cache.GetOrAddAsync(
                cacheKey,
                async () =>
                    await bankProvider.GetTransactionsAsync(
                        dto.Token,
                        bankAccount.Id,
                        fromUnix,
                        toUnix) ?? [],
                TimeSpan.FromMinutes(60));

            var transactions = bankTransactions?.Select(tx => new Transaction
            {
                Amount = tx.Amount / 100m,
                Currency = (Currency)tx.CurrencyCode,
                CreatedOn = DateTimeConverter.FromUnixTimestamp(tx.Time),
                Description = tx.Description,
                Comment = tx.Comment,
                AccountId = Guid.Empty
            }).ToList();

            var account = new Account
            {
                AccountNumber = bankAccount.Iban,
                Balance = bankAccount.Balance / 100m,
                Currency = (Currency)bankAccount.CurrencyCode,
                UserId = userId,
                Transactions = transactions ?? []
            };

            newAccounts.Add(account);
        }

        if (newAccounts.Count == 0)
            return;

        await accountRepository.AddRangeAsync(newAccounts, cancellationToken);
        await accountRepository.SaveChangesAsync(cancellationToken);
    }

    private static AccountDto MapToDto(Account account) =>
        new()
        {
            Id = account.Id,
            Name = account.Name,
            Description = account.Description,
            Balance = account.Balance,
            Currency = (CurrencyEnum)account.Currency,
            UserId = account.UserId
        };

    private static void ValidateDateRange(DateTime start, DateTime? end)
    {
        var now = DateTime.UtcNow;

        if (start > now)
            throw new BusinessRuleViolationException(
                "Start date cannot be in the future.");

        if (end.HasValue && end.Value > now)
            throw new BusinessRuleViolationException(
                "End date cannot be in the future.");
    }
}