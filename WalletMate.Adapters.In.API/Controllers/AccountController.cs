using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletMate.Adapters.In.API.Extensions;
using WalletMate.Application.Models;
using WalletMate.Application.Models.Account;
using WalletMate.Application.Ports.In;

namespace WalletMate.Adapters.In.API.Controllers;

/// <summary>
/// Controller for managing account-related operations.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AccountController(IAccountService accountService) : ControllerBase
{
    /// <summary>
    /// Retrieves all accounts.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A list of all accounts.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountDto>>> GetAllAccounts(CancellationToken cancellationToken)
    {
        var accounts = await accountService.GetAllAccountsAsync(cancellationToken);
        return Ok(accounts);
    }

    /// <summary>
    /// Retrieves an account by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the account.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The account with the specified ID.</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AccountDto>> GetAccount(Guid id, CancellationToken cancellationToken)
    {
        var account = await accountService.GetAccountByIdAsync(id, cancellationToken);
        return Ok(account);
    }

    /// <summary>
    /// Creates a new account.
    /// </summary>
    /// <param name="dto">The data transfer object containing account details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The ID of the newly created account.</returns>
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateAccount([FromBody] CreateAccountDto dto, CancellationToken cancellationToken)
    {
        var newAccountId = await accountService.CreateAccountAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetAccount), new { id = newAccountId }, newAccountId);
    }

    /// <summary>
    /// Updates an existing account.
    /// </summary>
    /// <param name="id">The unique identifier of the account to update.</param>
    /// <param name="dto">The data transfer object containing updated account details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The ID of the updated account.</returns>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Guid>> UpdateAccount(Guid id, [FromBody] UpdateAccountDto dto, CancellationToken cancellationToken)
    {
        var updatedAccountId = await accountService.UpdateAccountAsync(id, dto, cancellationToken);
        return Ok(updatedAccountId);
    }

    /// <summary>
    /// Deletes an account by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the account to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A success message if the account was deleted.</returns>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAccount(Guid id, CancellationToken cancellationToken)
    {
        await accountService.DeleteAccountAsync(id, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Imports data from Monobank for the current user.
    /// </summary>
    /// <param name="monobankDto">The data transfer object containing Monobank import details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A success message if the data was imported successfully.</returns>
    [HttpPost("import/monobank")]
    public async Task<IActionResult> ImportDataFromMonobank([FromBody] ImportMonobankDto monobankDto, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        await accountService.ImportFromMonobankAsync(monobankDto, userId, cancellationToken);
        return Ok();
    }
}