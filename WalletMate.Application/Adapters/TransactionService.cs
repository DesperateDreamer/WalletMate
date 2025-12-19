using WalletMate.Application.Common;
using WalletMate.Application.Exceptions;
using WalletMate.Application.Models;
using WalletMate.Application.Models.Transaction;
using WalletMate.Application.Ports.In;
using WalletMate.Application.Ports.Out;
using WalletMate.Domain.DomainEntities;
using WalletMate.Domain.DomainEntities.Enums;
using WalletMate.Domain.Ports.Out.Repositories;
using TransactionSortOption = WalletMate.Application.Models.Transaction.TransactionSortOption;

namespace WalletMate.Application.Adapters;

public class TransactionService(
    ITransactionRepository transactionRepository,
    IAccountRepository accountRepository,
    ICacheManagerPort cache,
    TransactionSortingStrategyResolver sortResolver) : ITransactionService
{
    private static string AccountCacheKey(Guid accountId) => $"transactions:account:{accountId:D}";

    public async Task<TransactionDto> GetTransactionByIdAsync(
        Guid transactionId,
        CancellationToken cancellationToken = default)
    {
        var transaction = await transactionRepository.GetByIdAsync(transactionId, cancellationToken);

        if (transaction is null)
            throw new EntityNotFoundException(nameof(Transaction), transactionId);

        return MapToDto(transaction);
    }

    public async Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync(
        CancellationToken cancellationToken = default)
    {
        var transactions = await transactionRepository.GetAllAsync(cancellationToken);
        return transactions.Select(MapToDto).ToList();
    }

    public async Task<Guid> CreateTransactionAsync(
        CreateTransactionDto dto,
        CancellationToken cancellationToken = default)
    {
        var account = await accountRepository.FirstOrDefaultAsync(a => a.Id == dto.AccountId, cancellationToken);
        if (account is null)
            throw new EntityNotFoundException("Account", dto.AccountId);
        
        if (dto.CategoryIds is { Count: > 0 })
        {
            throw new BusinessRuleViolationException(
                "CategoryIds are not supported in the current hexagonal setup. " +
                "Introduce a TransactionCategory port/use-case to manage category assignments.");
        }

        var transaction = new Transaction
        {
            Amount = dto.Amount,
            Currency = (Currency)dto.Currency,
            CreatedOn = DateTime.UtcNow,
            AccountId = dto.AccountId
        };

        await transactionRepository.AddAsync(transaction, cancellationToken);
        await transactionRepository.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync(AccountCacheKey(dto.AccountId));

        return transaction.Id;
    }

    public async Task<Guid> UpdateTransactionAsync(
        Guid transactionId,
        UpdateTransactionDto dto,
        CancellationToken cancellationToken = default)
    {
        var transaction = await transactionRepository.GetByIdAsync(transactionId, cancellationToken);
        if (transaction is null)
            throw new EntityNotFoundException(nameof(Transaction), transactionId);
        
        if (dto.CategoryIds is not null)
        {
            throw new BusinessRuleViolationException(
                "Updating CategoryIds is not supported in the current hexagonal setup. " +
                "Introduce a TransactionCategory port/use-case to manage category assignments.");
        }

        transaction.Amount = dto.Amount;
        transaction.Currency = (Currency)dto.Currency;

        transactionRepository.Update(transaction);
        await transactionRepository.SaveChangesAsync(cancellationToken);
        
        await cache.RemoveAsync(AccountCacheKey(transaction.AccountId));

        return transaction.Id;
    }

    public async Task DeleteTransactionAsync(Guid transactionId, CancellationToken cancellationToken = default)
    {
        var transaction = await transactionRepository.GetByIdAsync(transactionId, cancellationToken);
        if (transaction is null)
            throw new EntityNotFoundException(nameof(Transaction), transactionId);

        transactionRepository.Remove(transaction);
        await transactionRepository.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync(AccountCacheKey(transaction.AccountId));
    }

    public async Task<IEnumerable<TransactionDto>> GetTransactionsByAccountAsync(
        Guid accountId,
        TransactionSortOption sortBy = TransactionSortOption.Date,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = AccountCacheKey(accountId);

        var transactions = await cache.GetOrAddAsync(
            cacheKey,
            async () =>
            {
                var data = await transactionRepository.GetByAccountIdAsync(accountId, cancellationToken);
                return data.Select(MapToDto).ToList();
            },
            ttl: TimeSpan.FromMinutes(5));

        if (transactions is null)
            throw new EntityNotFoundException("Transaction", $"AccountId={accountId:D}");

        var strategy = sortResolver.Resolve((Domain.SortingStrategies.Enums.TransactionSortOption)sortBy);
        var sorted = strategy.Sort(transactions.Select(MapFromDto));
        
        return sorted.Select(MapToDto);
    }

    private static TransactionDto MapToDto(Transaction transaction) =>
        new()
        {
            Id = transaction.Id,
            Amount = transaction.Amount,
            Description = transaction.Description,
            Comment = transaction.Comment,
            Currency = (CurrencyEnum)transaction.Currency,
            CreatedOn = transaction.CreatedOn,
            AccountId = transaction.AccountId,
            CategoryIds = []
        };

    private static Transaction MapFromDto(TransactionDto dto) =>
        new()
        {
            Id = dto.Id,
            Amount = dto.Amount,
            Description = dto.Description,
            Comment = dto.Comment,
            Currency = (Currency)dto.Currency,
            CreatedOn = dto.CreatedOn,
            AccountId = dto.AccountId
        };
}