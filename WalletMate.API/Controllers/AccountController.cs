using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletMate.API.Extensions;
using WalletMate.BLL.Domain.Abstract;
using WalletMate.BLL.Domain.DTOs;

namespace WalletMate.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AccountController(IAccountService accountService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountDto>>> GetAllAccounts(CancellationToken cancellationToken)
    {
        var accounts = await accountService.GetAllAccountsAsync(cancellationToken);
        return Ok(accounts);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AccountDto>> GetAccount(Guid id, CancellationToken cancellationToken)
    {
        var account = await accountService.GetAccountByIdAsync(id, cancellationToken);
        return Ok(account);
    }
    
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateAccount([FromBody] CreateAccountDto dto,
        CancellationToken cancellationToken)
    {
        var newAccountId = await accountService.CreateAccountAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetAccount), new { id = newAccountId }, newAccountId);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Guid>> UpdateAccount(Guid id, [FromBody] UpdateAccountDto dto,
        CancellationToken cancellationToken)
    {
        var updatedAccountId = await accountService.UpdateAccountAsync(id, dto, cancellationToken);
        return Ok(updatedAccountId);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAccount(Guid id, CancellationToken cancellationToken)
    {
        await accountService.DeleteAccountAsync(id, cancellationToken);
        return Ok();
    }

    [HttpPost("import/monobank")]
    public async Task<IActionResult> ImportDataFromMonobank([FromBody] ImportMonobankDto monobankDto, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        await accountService.ImportDataFromMonobank(monobankDto, userId, cancellationToken);
        return Ok();
    }
}