using WalletMate.Application.Models;
using WalletMate.Application.Models.Account;

namespace WalletMate.Application.Ports.In;

public interface IAccountService
{
    Task<AccountDto> GetAccountByIdAsync(Guid accountId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AccountDto>> GetAllAccountsAsync(CancellationToken cancellationToken = default);
    Task<Guid> CreateAccountAsync(CreateAccountDto dto, CancellationToken cancellationToken = default);
    Task<Guid> UpdateAccountAsync(Guid accountId, UpdateAccountDto dto, CancellationToken cancellationToken = default);
    Task DeleteAccountAsync(Guid accountId, CancellationToken cancellationToken = default);
    Task ImportFromMonobankAsync(ImportMonobankDto monobankDto, Guid userId,
        CancellationToken cancellationToken = default);
}