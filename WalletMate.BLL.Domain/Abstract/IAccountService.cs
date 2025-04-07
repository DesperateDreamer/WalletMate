using WalletMate.BLL.Domain.DTOs;

namespace WalletMate.BLL.Domain.Abstract;

public interface IAccountService
{
    Task<AccountDto> GetAccountByIdAsync(Guid accountId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AccountDto>> GetAllAccountsAsync(CancellationToken cancellationToken = default);
    Task<Guid> CreateAccountAsync(CreateAccountDto dto, CancellationToken cancellationToken = default);
    Task<Guid> UpdateAccountAsync(Guid accountId, UpdateAccountDto dto, CancellationToken cancellationToken = default);
    Task DeleteAccountAsync(Guid accountId, CancellationToken cancellationToken = default);
    Task ImportDataFromMonobank(string token, Guid userId, DateTime startDate, DateTime? endDate = null,
        CancellationToken cancellationToken = default);
}