using Microsoft.EntityFrameworkCore;
using WalletMate.BLL.Domain.Abstract;
using WalletMate.BLL.Shared;
using WalletMate.BLL.Shared.CustomExceptions;
using WalletMate.BLL.Shared.DTOs;
using WalletMate.BLL.Shared.Enums;
using WalletMate.DAL.Context.Abstract;
using WalletMate.DAL.Entities;

namespace WalletMate.BLL.Domain;

public class TransactionService(IDataContext dataContext, TransactionSortingStrategyResolver sortResolver)
    : ITransactionService
{
    private readonly CacheManager<IEnumerable<TransactionDto>> _transactionCache =
        CacheManager<IEnumerable<TransactionDto>>.Instance;

    public async Task<TransactionDto> GetTransactionByIdAsync(Guid transactionId,
        CancellationToken cancellationToken = default)
    {
        var transaction = await dataContext.Transaction
            .Include(t => t.TransactionCategories)
            .FirstOrDefaultAsync(t => t.Id == transactionId, cancellationToken);

        if (transaction is null)
        {
            throw new NotFoundException("Transaction not found");
        }

        return new TransactionDto
        {
            Id = transaction.Id,
            Amount = transaction.Amount,
            Currency = transaction.Currency,
            Comment = transaction.Comment,
            Description = transaction.Description,
            CreatedOn = transaction.CreatedOn,
            AccountId = transaction.AccountId,
            CategoryIds = transaction.TransactionCategories.Select(tc => tc.Id).ToList()
        };
    }

    public async Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync(
        CancellationToken cancellationToken = default)
    {
        var transactions = await dataContext.Transaction
            .Include(t => t.TransactionCategories)
            .ToListAsync(cancellationToken);

        return transactions.Select(t => new TransactionDto
        {
            Id = t.Id,
            Amount = t.Amount,
            Description = t.Description,
            Comment = t.Comment,
            Currency = t.Currency,
            CreatedOn = t.CreatedOn,
            AccountId = t.AccountId,
            CategoryIds = t.TransactionCategories.Select(tc => tc.Id).ToList()
        });
    }

    public async Task<Guid> CreateTransactionAsync(CreateTransactionDto dto,
        CancellationToken cancellationToken = default)
    {
        var account = await dataContext.Account
            .FirstOrDefaultAsync(a => a.Id == dto.AccountId, cancellationToken);

        if (account is null)
            throw new NotFoundException("Account not found.");

        var transaction = new Transaction
        {
            Amount = dto.Amount,
            Currency = dto.Currency,
            AccountId = dto.AccountId,
            CreatedOn = DateTime.UtcNow
        };

        if (dto.CategoryIds is { Count: > 0 })
        {
            var categories = await dataContext.TransactionCategory
                .Where(c => dto.CategoryIds.Contains(c.Id))
                .ToListAsync(cancellationToken);

            if (categories.Count != dto.CategoryIds.Count)
                throw new NotAcceptableException("One or more category IDs are invalid.");

            transaction.TransactionCategories.AddRange(categories);
        }

        await dataContext.Transaction.AddAsync(transaction, cancellationToken);
        await dataContext.SaveChangesAsync(cancellationToken);

        // Invalidate cache for this account
        _transactionCache.Remove(dto.AccountId.ToString());

        return transaction.Id;
    }

    public async Task<Guid> UpdateTransactionAsync(Guid transactionId, UpdateTransactionDto dto,
        CancellationToken cancellationToken = default)
    {
        var transaction = await dataContext.Transaction
            .Include(t => t.TransactionCategories)
            .FirstOrDefaultAsync(t => t.Id == transactionId, cancellationToken);

        if (transaction is null)
            throw new NotFoundException("Transaction not found.");

        transaction.Amount = dto.Amount;
        transaction.Currency = dto.Currency;

        if (dto.CategoryIds is not null)
        {
            transaction.TransactionCategories.Clear();

            var categories = await dataContext.TransactionCategory
                .Where(c => dto.CategoryIds.Contains(c.Id))
                .ToListAsync(cancellationToken);

            if (categories.Count != dto.CategoryIds.Count)
                throw new NotAcceptableException("One or more category IDs are invalid.");

            transaction.TransactionCategories.AddRange(categories);
        }

        dataContext.Transaction.Update(transaction);
        await dataContext.SaveChangesAsync(cancellationToken);

        // Invalidate cache for this account
        _transactionCache.Remove(transaction.AccountId.ToString());

        return transaction.Id;
    }

    public async Task DeleteTransactionAsync(Guid transactionId, CancellationToken cancellationToken = default)
    {
        var transaction = await dataContext.Transaction
            .FirstOrDefaultAsync(t => t.Id == transactionId, cancellationToken);

        if (transaction is null)
            throw new NotFoundException("Transaction not found.");

        dataContext.Transaction.Remove(transaction);
        await dataContext.SaveChangesAsync(cancellationToken);

        // Invalidate cache
        _transactionCache.Remove(transaction.AccountId.ToString());
    }

    public async Task<IEnumerable<TransactionDto>> GetTransactionsByAccountAsync(
        Guid accountId,
        TransactionSortOption sortBy = TransactionSortOption.Date,
        CancellationToken cancellationToken = default)
    {
        var transactions = await _transactionCache.GetOrAddAsync(accountId.ToString(), async () =>
        {
            var data = await dataContext.Transaction
                .Where(t => t.AccountId == accountId)
                .Include(t => t.TransactionCategories)
                .ToListAsync(cancellationToken: cancellationToken);

            return data.Select(t => new TransactionDto
            {
                Id = t.Id,
                Amount = t.Amount,
                Description = t.Description,
                Comment = t.Comment,
                Currency = t.Currency,
                CreatedOn = t.CreatedOn,
                AccountId = t.AccountId,
                CategoryIds = t.TransactionCategories.Select(tc => tc.Id).ToList()
            }).ToList();
        }, TimeSpan.FromMinutes(5)); // TTL: 5 minutes

        if (transactions is null)
        {
            throw new NotFoundException("Transactions not found.");
        }

        var strategy = sortResolver.Resolve(sortBy);
        return strategy.Sort(transactions);
    }
}