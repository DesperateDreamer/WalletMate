using System.Linq.Expressions;
using WalletMate.Domain.DomainEntities;

namespace WalletMate.Domain.Ports.Out.Repositories;

public interface IAccountRepository
{
    Task<Account?> FirstOrDefaultAsync(
        Expression<Func<Account, bool>> predicate,
        CancellationToken cancellationToken = default);
    Task<List<Account>> ToListAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Account account, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<Account> accounts, CancellationToken cancellationToken = default);
    void Update(Account account);
    void Remove(Account account);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}